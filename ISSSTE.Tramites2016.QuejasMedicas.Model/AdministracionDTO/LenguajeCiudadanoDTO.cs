using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class LenguajeCiudadanoDTO : DoDisposeDTO
    {
        public bool EsActivo { get; set; }
        public bool EsDeVista { get; set; }
        public bool EsUsuarioInterno { get; set; }
        public int LenguajeCiudadanoId { get; set; }
        public string Region { get; set; }
        public string IdTecnico { get; set; }
        public string Concepto { get; set; }
    }
}