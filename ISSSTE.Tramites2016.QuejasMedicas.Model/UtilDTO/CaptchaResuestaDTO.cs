using Newtonsoft.Json;
using System.Collections.Generic;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO
{
    public class CaptchaResuestaDTO
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public string ValidatedDateTime { get; set; }

        [JsonProperty("hostname")]
        public string HostName { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }

    }
}
