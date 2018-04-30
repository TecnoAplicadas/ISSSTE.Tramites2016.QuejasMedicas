using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CuentaDTO
{
    public class UsuarioDTO : DoDisposeDTO
    {
        public string Perfil { get; set; }

        public int SysUsuarioId { get; set; }

        public bool EsActivo { get; set; }

        public string Clave { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
    }
}