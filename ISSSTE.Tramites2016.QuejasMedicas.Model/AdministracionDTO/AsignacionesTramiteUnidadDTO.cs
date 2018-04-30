using System.Collections.Generic;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class AsignacionesTramiteUnidadDTO
    {
        public AsignacionesTramiteUnidadDTO()
        {
            UnidadesAsignadas = new List<TramiteUnidadAtencionDTO>();
            UnidadesNoAsignadas = new List<TramiteUnidadAtencionDTO>();
        }

        public int CatTipoTramiteId { get; set; }
        public List<TramiteUnidadAtencionDTO> UnidadesAsignadas { get; set; }
        public List<TramiteUnidadAtencionDTO> UnidadesNoAsignadas { get; set; }
    }
}