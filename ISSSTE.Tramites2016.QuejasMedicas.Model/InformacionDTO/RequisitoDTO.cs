using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class RequisitoDTO : DoDisposeDTO
    {
        public int RequisitoId { get; set; }
        public int CatTipoTramiteId { get; set; }
        public string NombreDocumento { get; set; }
        public string Descripcion { get; set; }
        public bool EsObligatorio { get; set; }
        public bool EsActivo { get; set; }
    }
}