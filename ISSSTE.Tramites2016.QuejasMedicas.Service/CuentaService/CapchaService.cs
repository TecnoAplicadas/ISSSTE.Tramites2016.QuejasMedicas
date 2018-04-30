using ISSSTE.Tramites2016.QuejasMedicas.Domain.Utils;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO;
using Newtonsoft.Json;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService
{
    public class CaptchaService
    {

        /// <summary>
        /// Valida si la respuesta del usuario a la Captcha proporcionada es correcta
        /// </summary>
        /// <param name="RespuestaCaptcha"></param>
        /// <returns></returns>
        public static bool ValidarCaptcha(string RespuestaCaptcha)
        {
            string urlToPost = Enumeracion.EnumCaptcha.urlToPost;
            string secretKey = new ConfiguracionDAO().GetValorConfiguracion(Enumeracion.EnumCaptcha.secretKey); //Enumeracion.EnumCaptcha.secretKey;

            var postData = Enumeracion.EnumCaptcha.cadSecret + secretKey + Enumeracion.EnumCaptcha.cadResponse + RespuestaCaptcha;
            string result = WebUtilService.PeticionHTTP(urlToPost, postData);

            var captChaesponse = JsonConvert.DeserializeObject<CaptchaResuestaDTO>(result);
            return captChaesponse.Success;

        }
    }
}
