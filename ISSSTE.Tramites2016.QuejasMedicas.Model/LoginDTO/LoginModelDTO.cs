using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.LoginDTO
{
    public class LoginModelDTO : DoDisposeDTO
    {
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string ReturnUrl { get; set; }
    }
}