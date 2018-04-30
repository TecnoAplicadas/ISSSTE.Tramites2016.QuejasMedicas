using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class UnidadMedicaDTO
    {
        public int UnidadMedicaId { set; get; }

        [CatalogoAtributoInfo(NombreVista = "Unidad de atención", EsVisible = false, EsEditable = true,
            Tipo = TipoElementoEnum.select)]
        public int UnidadAtencionId { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Unidad de atención", EsEditable = false)]
        public string UnidadAtencionConcepto { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Nombre de la unidad médica", EsEditable = true, TamanioMaximo = 150)]
        public string Descripcion { set; get; }

        [CatalogoAtributoInfo(EsVisible = false)]
        public bool EsActivo { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Estado")]
        public string EsActivoConcepto => EsActivo ? "Activo" : "Inactivo";
    }
}