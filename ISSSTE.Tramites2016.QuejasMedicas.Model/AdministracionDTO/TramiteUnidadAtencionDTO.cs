using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class TramiteUnidadAtencionDTO : DoDisposeDTO
    {
        public int CatTipoTramiteId { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Trámite", EsEditable = true)]
        public string TramiteUnidadAtencionConcepto { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Unidad atención", EsEditable = false)]
        public string UnidadAtencionConcepto { get; set; }

        [CatalogoAtributoInfo(EsVisible = false)]
        public bool EsActivo { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Estado")]
        public string EsActivoConcepto => EsActivo ? "Activo" : "Inactivo";

        public int TramiteUnidadAtencionId { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Unidad atención", EsVisible = false, EsEditable = true,
            Tipo = TipoElementoEnum.select)]
        public int UnidadAtencionId { get; set; }
    }
}