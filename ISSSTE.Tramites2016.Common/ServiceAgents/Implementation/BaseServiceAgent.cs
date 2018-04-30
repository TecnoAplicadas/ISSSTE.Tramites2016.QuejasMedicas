#region

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using ISSSTE.Tramites2016.Common.Model;
using ISSSTE.Tramites2016.Common.Web;
using Newtonsoft.Json;

#endregion

namespace ISSSTE.Tramites2016.Common.ServiceAgents.Implementation
{
    /// <summary>
    ///     Clase abstarcta que contiene metodos y propiedades comune para el consumo de los WS de Informix
    /// </summary>
    public abstract class BaseServiceAgent
    {
        #region Constants

        /// <summary>
        /// plantilla a utilizar para obtener un token de acceso
        /// </summary>
        private const string TokenDataTemplate = "grant_type=password&username={0}&password={1}";

        #endregion

        #region Static Properties

        /// <summary>
        ///     Obtiene la URL base para consumir los servicios web del app.config
        /// </summary>
        protected string ServiceBaseUrl
        {
            get { return ConfigurationManager.AppSettings["InformixWSBaseUrl"]; }
        }

        /// <summary>
        ///     Contiene la URL relativa donde obtener el token de autenticación del app.config
        /// </summary>
        private string TokenPath
        {
            get { return ConfigurationManager.AppSettings["InformixWSTokenPath"]; }
        }


        /// <summary>
        ///     Obtiene el nombre de usuario utilizado para obtener el token del app.config
        /// </summary>
        private string UserName
        {
            get { return ConfigurationManager.AppSettings["InformixWSUserName"]; }
        }

        /// <summary>
        ///     Obtiene la contraseña utilizada para obtener el token del app.config
        /// </summary>
        private string Password
        {
            get { return ConfigurationManager.AppSettings["InformixWSPassword"]; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///     Genera una peticion con el token.
        /// </summary>
        /// <param name="token">Token a usar</param>
        /// <returns>La respuesta del servicio</returns>
        protected HttpClient BuildHttpClient(Token token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HttpContants.ContentTypes.Json));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(HttpContants.Headers.Authorization.Values.Bearer, token.access_token);

            return client;
        }

        /// <summary>
        ///     Obtiene el Token del servicio de informix.
        /// </summary>
        /// <returns>Regresa el token</returns>
        protected Token GetToken()
        {
            try
            {
                var postData = String.Format(TokenDataTemplate, UserName, Password);
                var dataBytes = Encoding.UTF8.GetBytes(postData);
                var baseAddress = ServiceBaseUrl + TokenPath;
                var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));

                http.ContentType = HttpContants.ContentTypes.FormUrlEncode;
                http.Method = HttpMethod.Post.Method;

                using (var postStream = http.GetRequestStream())
                {
                    postStream.Write(dataBytes, 0, dataBytes.Length);
                }

                string json;

                using (var response = (HttpWebResponse)http.GetResponse())
                {
                    using (var sr = new StreamReader(response.GetResponseStream()))
                        json = sr.ReadToEnd();
                }

                var token = JsonConvert.DeserializeObject<Token>(json);

                return token;
            }
            catch (Exception)
            {
                return null;
            }

            #endregion
        }
    }
}