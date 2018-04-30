using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class CatTipoEntidadDTO : DoDisposeDTO
    {
        public int CatTipoEntidadId { get; set; }
        public string Concepto { get; set; }
        public bool EsActivo { get; set; }
        public string Siglas { get; set; }
    }
}