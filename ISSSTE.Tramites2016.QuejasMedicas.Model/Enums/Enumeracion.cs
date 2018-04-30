using System.Collections.Generic;
using System.ComponentModel;


namespace ISSSTE.Tramites2016.QuejasMedicas.Model.Enums
{
    public static class Enumeracion
    {
        public enum CatalogoEnum
        {
            TipoTramite = 1,
            EstadoCita = 2,
            UnidadAtencion = 3,
            TramiteUnidadAtencion = 4,
            UnidadMedica = 5
        }

        /// <summary>
        ///     Enum para identificar los roles del sistema
        /// </summary>
        public enum RolEnum
        {
            [Description(EnumVarios.Paciente)] Derechohabiente = 1,
            [Description(EnumVarios.Promovente)] Promovente = 2,
            [Description(EnumVarios.AdmGral)] Administrador = 3,
            [Description(EnumVarios.OpUadyCs)] Operador = 4
        }

        public static Dictionary<string, string> MensajesServidor { get; set; }

        public static int TotalRegistros { get; set; }

        public static class EnumSysConfiguracion
        {
            public const string LlaveCifradoUsuario = "LlaveCifradoUsuario";
            public const string RutaLog = "RutaLog";
            public const string RutaArchivos = "RutaArchivos";
            public const string ContenedorEjecutables = "ContenedorEjecutables";
            public const string IntentosCita = "IntentosCita";
            public const string LimiteCancelacionCita = "LimiteCancelacionCita";           
            public const string CuerpoDelCorreo = "BodyContentPath";
        }

        public static class EnumTipoInvolucrado
        {
            public const int Promovente = 1;
            public const int Paciente = 2;
        }

        public static class EnumTipoEdoCita
        {
            public const int Pendiente = 1;
            public const int Cancelado = 2;
            public const int Atendido = 3;
            public const int NoAsistio = 4;
            public const int SinDocumentos = 5;
            public const int PorCausasDeFuerzaMayor = 6;
        }

        public static class EnumVarios
        {
            public const string Contexto = "ISSSTEEntities";
            public const string Femenino = "Femenino";
            public const string Masculino = "Masculino";
            public const string Paciente = "Paciente";
            public const string Promovente = "Promovente";
            public const string AdmGral = "Administrador General";
            public const string OpUadyCs = "Operador UADyCS";
            public const int TodasLasDelegaciones = -1;
            public const int DiasSeleccionables = 30;
            public const string Prefijo = "ERROR: ";
            public const int Sabado = 6;
            public const int Domingo = 0;
            public const int ConEliminacion = 1;
        }

        public static class EnumCaptcha
        {
            public const string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
            public static string secretKey = "ClaveSecretaCaptcha";//tab
            public const string cadSecret = "secret=";
            public const string cadResponse = "&response=";
            public const string urlRecaptcha = "https://www.google.com/recaptcha/api.js?hl=es";
            public static string siteKey = "ClaveSitioCaptcha";//tab
        }

        public static class EnumConsCorreo
        {
            public const string TituloCorreoCitaAgendada = "TituloCorreoCitaAgendada";
            public const string CuerpoCorreoCitaAgendada = "CuerpoCorreoCitaAgendada";
            public const string TituloCorreoCitaCancelada = "TituloCorreoCancelada";
            public const string CuerpoCorreoCitaCancelada = "CuerpoCorreoCitaCancelada";
            public const string TituloCorreoCitaInasistencia = "TituloCorreoCitaInasistencia";
            public const string CuerpoCorreoCitaInasistencia = "CuerpoCorreoCitaInasistencia";
            public const string NotaBloqueoCitas = "NotaBloqueoCitas";
            public const string NotaCitaAgendada = "NotaCitaAgendada";
            public const string NotaCitaCancelada = "NotaCitaCancelada";
        }

        public static class EnumConstErrores
        {
            public const string SolicitudNoEncontrada = "SolicitudNoEncontrada";
            public const string RegistroYaEliminado = "RegistroYaEliminado";
            public const string RegistroYaActivo = "RegistroYaActivo";
            public const string RegistroActivado = "RegistroActivado";
            public const string RegistroEliminado = "RegistroEliminado";
            public const string CitaCancelada = "CitaCancelada";
            public const string CitaNoEncontrada = "CitaNoEncontrada";
            public const string CitaEliminada = "CitaEliminada";
            public const string CupoExcedido = "CupoExcedido";
            public const string CitaNoEliminada = "CitaNoEliminada";
            public const string UsuarioBloquedo = "UsuarioBloquedo";
            public const string CitaPendiente = "CitaPendiente";
            public const string ExePdf = "ExePDF";
            public const string ErrorGeneral = "ErrorGeneral";
            public const string ErrorCaptchaNoProporcionada = "CaptchaNoProporcionada";
            public const string ErrorCaptchaNoValido = "CaptchaNovalida";
            public const string ErrorExpiracionTiempoCancelacion = "ErrorExpiracionTiempoCancelacion";
            public const string ErrorConceptoYaExiste =
                "Ocurrió un error al guardar los cambios. El concepto para el {0} ya existe.";
        }
    }
}