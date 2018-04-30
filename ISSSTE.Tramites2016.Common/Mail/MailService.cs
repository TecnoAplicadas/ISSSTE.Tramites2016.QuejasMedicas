#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion

namespace ISSSTE.Tramites2016.Common.Mail
{
    /// <summary>
    ///     Servicio para envio de correos
    /// </summary>
    public class MailService : IMailService
    {
        #region Fields

        /// <summary>
        ///     Plantailla a utilizar al momento de enviar un correo
        /// </summary>
        private MasterPageParameters _masterpage;

        #endregion

        #region Properties

        /// <summary>
        ///     Cliente creado
        /// </summary>
        public SmtpClient Client { get; set; }

        /// <summary>
        ///     El from puede estar compuesto por un nombre opcional, pero siempre debe incluir el correo desde el cual se manda.
        ///     Ejemplo
        ///     ISSSTE. Inscripciones a talleres dig.tramites0x40issste.gob.mx
        ///     De lo contrario, el mensaje no se envirará.
        /// </summary>
        public string From { get; set; }

        #endregion

        #region Constructors 

        /// <summary>
        ///     Instancia de servicios de correo electronicos. por default con los parametros del config.
        /// </summary>
        public MailService()
        {
            Client = new SmtpClient(ConfigurationManager.AppSettings["MailHost"])
            {
                //Port = 25,
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]),
                Credentials =
                    new NetworkCredential(ConfigurationManager.AppSettings["MailUsername"],
                        ConfigurationManager.AppSettings["MailPassword"]),
                //EnableSsl = true
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["MailUseSSL"])
            };

            From = ConfigurationManager.AppSettings["MailFromAddress"];

            //TODO: Validar y descomentar
            //this._masterpage = MasterPageParameters.LoadFromFile(ConfigurationManager.AppSettings["MailMasterPagePath"], ConfigurationManager.AppSettings["MailMasterPageLogoPath"]);
        }

        /// <summary>
        ///     Instancia de servicios de correo electronicos. por default con los parametros del config.
        /// </summary>
        /// <param name="masterPageAbsolutePath">Ruta de donde cargar el template del correo</param>
        /// <param name="logoAbsolutePath">Ruta de donde cargar la imagen del logo del correo</param>
        public MailService(string masterPageAbsolutePath, string logoAbsolutePath)
            : this()
        {
            SetMasterPage(masterPageAbsolutePath, logoAbsolutePath);
        }

        #endregion

        #region IMailService Implementation

        /// <summary>
        ///     Carga el template html y la imagen a utilizar como logo al momento de enviar un correo
        /// </summary>
        /// <param name="masterPageAbsolutePath">Ruta de donde cargar el template del correo</param>
        /// <param name="logoAbsolutePath">Ruta de donde cargar la imagen del logo del correo</param>
        public void SetMasterPage(string masterPageAbsolutePath, string logoAbsolutePath)
        {
            _masterpage = MasterPageParameters.LoadFromFile(masterPageAbsolutePath, logoAbsolutePath);
        }

        /// <summary>
        ///     Envía un correo electrónico con la información proporcionada
        /// </summary>
        /// <param name="senTomail">Correo destino</param>
        /// <param name="subject">Titulo del mensaje</param>
        /// <param name="body">Cuerpo del correo</param>
        /// <returns><code>true</code> cuando se envio correctamente.</returns>
        public async Task SendMailAsync(string senTomail, string subject, string body)
        {
            await SendMailAsync(new List<string> {senTomail}, null, null, subject, body, null, null, _masterpage);
        }

        /// <summary>
        ///     Envía un correo electrónico con la información proporcionada
        /// </summary>
        public async Task SendMailAsync(List<string> recipient, List<string> carbonCopy, List<string> blackCarbonCopy,
            string subject, string htmlBodyMessage, Dictionary<string, string> imagesAppendToView,
            IList<string> fileNames, MasterPageParameters masterpage)
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["MailSendEnable"]))
            {
                var message = CreateMessage(recipient, carbonCopy, blackCarbonCopy, subject, htmlBodyMessage,
                    imagesAppendToView, fileNames, masterpage);

                await Client.SendMailAsync(message);
            }
        }

        /// <summary>
        ///     Envía un correo electrónico con la información proporcionada
        /// </summary>
        public MailMessage CreateMessage(List<string> recipient, List<string> carbonCopy, List<string> blackCarbonCopy,
            string subject, string htmlBodyMessage, Dictionary<string, string> imagesAppendToView,
            IList<string> fileNames, MasterPageParameters masterpage)
        {
            var mailMessage = new MailMessage();
            var resources = imagesAppendToView ?? new Dictionary<string, string>();

            if (masterpage?.Resources != null && masterpage.Resources.Count > 0)
                foreach (var x in masterpage.Resources)
                    resources.Add(x.Key, x.Value);

            var destinyListCleaned = SplitAndCleanRecipients(recipient);
            foreach (var destiny in destinyListCleaned)
                mailMessage.To.Add(new MailAddress(destiny));

            var sestinyCarbonCopyCleande = SplitAndCleanRecipients(carbonCopy);
            foreach (var destiny in sestinyCarbonCopyCleande)
                mailMessage.CC.Add(new MailAddress(destiny));

            var bcc = SplitAndCleanRecipients(blackCarbonCopy);
            foreach (var destiny in bcc)
                mailMessage.Bcc.Add(new MailAddress(destiny));

            var titleinside = GetTagInfo(htmlBodyMessage, "title");
            var bodyhtml = masterpage == null
                ? htmlBodyMessage
                : MergeMasterPageMail(masterpage.Masterpage, htmlBodyMessage);

            mailMessage.Subject = string.IsNullOrEmpty(titleinside) ? subject : titleinside;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = bodyhtml;
            mailMessage.From = new MailAddress(From);

            if (fileNames != null)
                AttachFiles(mailMessage, fileNames);

            if (resources.Count > 0)
                mailMessage.AlternateViews.Add(LinkedFiles(bodyhtml, resources));

            return mailMessage;
        }

        #endregion

        #region Helper Methods

        private string MergeMasterPageMail(string master, string content)
        {
            var body = GetTagInfo(content, "body");
            var footer = GetTagInfo(content, "footer");

            if (string.IsNullOrEmpty(body) && string.IsNullOrEmpty(footer) && !string.IsNullOrEmpty(content))
                body = content.Replace("\n", @"<br />");

            var result = master.Replace("[HTMLBODY]", body).Replace("[HTMLFOOTER]", footer);
            return result;
        }

        private string GetTagInfo(string content, string tag)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            var patternBody = "<" + tag + ".*?>(.*?)<\\/" + tag + ">";

            var body = string.Empty;

            var matchesbody =
                Regex.Matches(content.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("  ", " "),
                    patternBody);
            if (matchesbody.Count > 0)
                foreach (Match m in matchesbody)
                    body += m.Groups[1];

            return body;
        }

        private void AttachFiles(MailMessage msg, IList<string> fileNames)
        {
            foreach (var file in fileNames)
                if (!string.IsNullOrEmpty(file))
                {
                    var data = new Attachment(file, MediaTypeNames.Application.Octet);
                    msg.Attachments.Add(data);
                }
        }

        private List<string> SplitAndCleanRecipients(List<string> recipients)
        {
            var result = new List<string>();

            if (recipients == null)
                return result;

            for (var i = 0; i < recipients.Count; i++)
                if (!string.IsNullOrEmpty(recipients[i]))
                {
                    var mails = recipients[i].Replace(',', ';');
                    if (mails.Contains(";"))
                    {
                        recipients.AddRange(from u in mails.Split(';') select u.Trim());
                        recipients[i] = string.Empty;
                    }
                    else if (!string.IsNullOrEmpty(recipients[i]) && !string.IsNullOrEmpty(recipients[i].Trim()))
                    {
                        result.Add(recipients[i].Trim());
                    }
                }

            return result;
        }

        private AlternateView LinkedFiles(string bodyHtml, Dictionary<string, string> files)
        {
            var view = AlternateView.CreateAlternateViewFromString(bodyHtml, null, MediaTypeNames.Text.Html);
            foreach (var x in files)
            {
                var linked = new LinkedResource(x.Value, MediaTypeNames.Application.Octet) {ContentId = x.Key};
                view.LinkedResources.Add(linked);
            }

            return view;
        }

        #endregion
    }
}