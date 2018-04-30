using System;
using System.Collections.Generic;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class GeneralCitaDTO : CitaDTO
    {
        public List<string> ListaEncabezados { get; set; }
        public List<string> ListaLlaves { get; set; }

        public string Encabezados { get; set; }
        public string Table { get; set; }
        public string Llaves { get; set; }

        public string NombrePromovente { get; set; }
        public string Apellido1Promovente { get; set; }
        public string Apellido2Promovente { get; set; }
        public string CURPPromovente { get; set; }
        public string LadaPromovente { get; set; }
        public string TelefonoPromovente { get; set; }
        public string CelularPromovente { get; set; }
        public string CorreoPromovente { get; set; }
        public string NombrePaciente { get; set; }
        public string Apellido1Paciente { get; set; }
        public string Apellido2Paciente { get; set; }
        public string CURPPaciente { get; set; }
        public string LadaPaciente { get; set; }
        public string TelefonoPaciente { get; set; }
        public string CelularPaciente { get; set; }
        public string CorreoPaciente { get; set; }
        public string FechaFinal { get; set; }
        public string HorarioFinal { get; set; }

        public string FechaInicioRegistro { get; set; }
        public string FechaFinRegistro { get; set; }
        public string FechaInicioCita { get; set; }
        public string FechaFinCita { get; set; }

        public string HoraInicioRegistro { get; set; }
        public string HoraFinRegistro { get; set; }
        public string HoraInicioCita { get; set; }
        public string HoraFinCita { get; set; }

        public string EdadInicioPromovente { get; set; }
        public string EdadFinPromovente { get; set; }
        public string EdadInicioPaciente { get; set; }
        public string EdadFinPaciente { get; set; }

        public string CatTipoEdoCita { get; set; }
        public string CatTipoTramite { get; set; }

        public int EdadPromovente { get; set; }
        public string ConceptoUnidad { get; set; }
        public string EstadoCita { get; set; }
        public string GeneroPaciente { get; set; }
        public string GeneroPromovente { get; set; }
        public string EstadoTramite { get; set; }

        public bool? EsUsuarioBloqueadoPaciente { get; set; }
        public bool? EsMasculinoPaciente { get; set; }
        public bool? EsMasculinoPromovente { get; set; }
        public bool EsRepresentante { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime FechaCita { get; set; }

        public TimeSpan HoraCita { get; set; }
        public TimeSpan HoraSolicitud { get; set; }

        public int EdadPaciente { get; set; }
        public int HorarioId { get; set; }

        public long HoraSolicitudTicks { get; set; }
        public long HoraCitaTicks { get; set; }
        public long FechaCitaTicks { get; set; }
        public long FechaRegistroTicks { get; set; }

        public string FechaRegistroCad { get; set; }
        public string FechaCitaCad { get; set; }
        public string ColorTramite { get; set; }
        public string ColorCita { get; set; }
        public List<GeneralCitaDTO> ListaResultados { get; set; }
        public List<IDictionary<string, object>> ListaRefleccion { get; set; }

        /// <summary>
        ///     ////////////////////////////////////////// Paginador
        /// </summary>

        public int TotalRegistros { get; set; }

        public int UnidadAtencionId { get; set; }

        public int PaginaSolicitada { get; set; }
        public int RegistrosPorPagina { get; set; }
        public bool ConPaginadorController { get; set; }
        public string TextoBusqueda { get; set; }

        /// <summary>
        /// Respuesta del Captcha realizado por el usuario
        /// </summary>
        public string RespuestaCaptcha { get; set; }

    }
}