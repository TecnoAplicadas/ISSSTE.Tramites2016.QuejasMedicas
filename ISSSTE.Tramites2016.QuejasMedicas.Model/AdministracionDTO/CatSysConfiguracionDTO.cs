using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class CatSysConfiguracionDTO : DoDisposeDTO
    {
        public int CatSysConfiguracionId { get; set; }
        public bool EsActivo { get; set; }
        public bool EsAdministrable { get; set; }
        public string Concepto { get; set; }
        public string Valor { get; set; }
        public string TipoDato { get; set; }
    }
}