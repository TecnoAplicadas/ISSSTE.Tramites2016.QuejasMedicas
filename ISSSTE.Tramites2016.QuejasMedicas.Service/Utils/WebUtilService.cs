using System.IO;
using System.Net;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class WebUtilService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UrlToPost"></param>
        /// <param name="PostData"></param>
        /// <returns></returns>
        public static string PeticionHTTP(string UrlToPost, string PostData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlToPost);
            request.Method = "POST";
            request.ContentLength = PostData.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(PostData);
            }

            string result = string.Empty;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
    }
}
