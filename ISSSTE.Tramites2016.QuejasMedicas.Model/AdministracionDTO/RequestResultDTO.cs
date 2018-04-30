using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class RequestResultDTO : CitaDTO
    {
        public RequestResultDTO()
        {
            Paciente = new PersonaDTO();
            Promovente = new PersonaDTO();
            Cita = new CitaDTO();
            UnidadAtencion = new UnidadAtencionDTO();
            EsUsuarioBloqueado = false;
            Folio = string.Empty;
            SolicitudId = default(int);
        }

        /// <summary>
        ///     Cita, BUscar DTO que contenga unicamente datos de la cita
        /// </summary>
        public CitaDTO Cita { get; set; }

        public UnidadAtencionDTO UnidadAtencion { get; set; }

        /// <summary>
        ///     Folio de la solicitud
        /// </summary>
        public string Folio { get; set; }

        public bool EsUsuarioBloqueado { get; set; }
    }
}