namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class CatalogoResultDTO
    {
        public int CatalogoId { get; set; }
        public string Concepto { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
        public string TipoDato { get; set; }
        public bool EsAdministrable { get; set; }
        public bool EsActivo { get; set; }
    }
}