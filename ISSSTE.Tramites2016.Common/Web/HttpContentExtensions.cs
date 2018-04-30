#region

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

namespace ISSSTE.Tramites2016.Common.Web
{
    /// <summary>
    ///     COntiene métodos extención para <see cref="HttpContent" />
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        ///     Interpreta el contenido multipart y lo estructura para un más facil acceso
        /// </summary>
        /// <param name="postedContent">Contenido el cual interpretar</param>
        /// <returns>Información envíada como multipart</returns>
        public static async Task<HttpPostedData> ParseMultipartAsync(this HttpContent postedContent)
        {
            var provider = await postedContent.ReadAsMultipartAsync();

            var files = new Dictionary<string, HttpPostedFile>(StringComparer.InvariantCultureIgnoreCase);
            var fields = new Dictionary<string, HttpPostedField>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var content in provider.Contents)
            {
                var fieldName = content.Headers.ContentDisposition.Name?.Trim('"') ?? string.Empty;
                if (!string.IsNullOrEmpty(content.Headers.ContentDisposition.FileName))
                {
                    var file = await content.ReadAsByteArrayAsync();
                    var fileName = content.Headers.ContentDisposition.FileName.Trim('"');
                    files.Add(fieldName, new HttpPostedFile(fieldName, fileName, file));
                }
                else
                {
                    var data = await content.ReadAsStringAsync();
                    fields.Add(fieldName, new HttpPostedField(fieldName, data));
                }
            }

            return new HttpPostedData(fields, files);
        }
    }
}