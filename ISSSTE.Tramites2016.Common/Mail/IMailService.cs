#region

using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

#endregion

namespace ISSSTE.Tramites2016.Common.Mail
{
    public interface IMailService
    {
        void SetMasterPage(string filePathMasterPage, string filePathLogo);
        Task SendMailAsync(string senTomail, string subject, string body);

        Task SendMailAsync(List<string> recipient, List<string> carbonCopy, List<string> blackCarbonCopy,
            string subject,
            string htmlBodyMessage, Dictionary<string, string> imagesAppendToView, IList<string> fileNames,
            MasterPageParameters masterpage);

        MailMessage CreateMessage(List<string> recipient, List<string> carbonCopy, List<string> blackCarbonCopy,
            string subject, string htmlBodyMessage, Dictionary<string, string> imagesAppendToView,
            IList<string> fileNames, MasterPageParameters masterpage);
    }
}