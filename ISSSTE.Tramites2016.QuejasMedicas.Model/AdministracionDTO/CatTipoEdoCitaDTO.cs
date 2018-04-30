using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class CatTipoEdoCitaDTO
    {
        [CatalogoAtributoInfo(EsVisible = false)]
        public int CatTipoEdoCitaId { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Descripción", EsEditable = true, TamanioMaximo = 90)]
        public string Concepto { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Semáforo", EsEditable = true, Tipo = TipoElementoEnum.color)]
        public string Semaforo { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Estado", EsVisible = false, EsEditable = false,
            Tipo = TipoElementoEnum.checkbox)]
        public bool EsActivo { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Estado")]
        public string EsActivoConcepto => EsActivo ? "Activo" : "Inactivo";
    }
}