namespace ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO
{
    public class DuplaValoresDTO : DoDisposeDTO
    {
        public string Valor { get; set; }
        public string Informacion { get; set; }
        public int Id { get; set; }
        public int IdAux { get; set; }
        public bool Selected { get; set; }
        public bool Reactivar { get; set; }
    }
}