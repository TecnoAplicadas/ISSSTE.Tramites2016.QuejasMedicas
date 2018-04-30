using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class CatTipoTramiteDTO : DoDisposeDTO
    {
        [CatalogoAtributoInfo(EsVisible = false)]
        public int CatTipoTramiteId { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Nombre trámite", EsEditable = true, TamanioMaximo = 100)]
        public string Concepto { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Homoclave", EsEditable = true, Tipo = TipoElementoEnum.text,
            EsRequerido = true,TamanioMaximo =20)]
        public string HomoClave { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Semáforo", EsEditable = true, Tipo = TipoElementoEnum.color)]
        public string Semaforo { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Estado", EsVisible = false, EsEditable = false,
            Tipo = TipoElementoEnum.checkbox)]
        public bool EsActivo { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Estado")]
        public string EsActivoConcepto => EsActivo ? "Activo" : "Inactivo";
    }
}