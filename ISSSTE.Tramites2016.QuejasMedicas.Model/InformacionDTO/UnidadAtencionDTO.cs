using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class UnidadAtencionDTO : DomicilioDTO
    {
        [CatalogoAtributoInfo(EsVisible = false)]
        public int UnidadAtencionId { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Entidad", EsVisible = false, EsEditable = true,
            Tipo = TipoElementoEnum.select)]
        public int CatTipoEntidadId { get; set; }

        [CatalogoAtributoInfo(NombreVista = "UADyCS", EsEditable = true, TamanioMaximo = 50)]
        public string Descripcion { get; set; }

        public int Contador { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Prefijo", EsEditable = true, EsVisible = false, EsRequerido = false, TamanioMaximo = 6)]
        public string Prefijo { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Mascara", EsVisible = false, EsEditable = true, EsRequerido = false, TamanioMaximo = 20)]
        public string Mascara { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Estado", EsEditable = false, EsVisible = false,
            Tipo = TipoElementoEnum.checkbox)]
        public bool EsActivo { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Estado", Orden = 1)]
        public string EsActivoConcepto => EsActivo ? "Activo" : "Inactivo";

        public bool ReiniciarContador { get; set; }
    }
}