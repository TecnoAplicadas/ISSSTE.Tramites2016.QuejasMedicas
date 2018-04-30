namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class CalendarioDTO : CitaDTO
    {
        public int UnidadAtencionId { get; set; }
        public string TramiteUnidadAtencion { get; set; }
        public string MesAnio { get; set; }
        public string SinDisponibilidad { get; set; }
        public string Disponible { get; set; }
        public string DiaActual { get; set; }
        public string PalabraBusqueda { get; set; }
        public string IdFechaLimite { get; set; }
    }
}