using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO
{
    public class CitaDAO
    {
        /// <summary>
        ///     Obtiene el detalle de una cita
        /// </summary>
        /// <param name="CitaDto"></param>
        /// <returns></returns>
        public CitaDTO GetCita(CitaDTO CitaDto,bool CualquierEstado=false)
        {
            using (var modelo = new ISSSTEEntities())
            {
        
                if (!string.IsNullOrEmpty(CitaDto.NumeroFolio) && CitaDto.SolicitudId == 0)
                {
                    CancelacionAutomatica(CitaDto.NumeroFolio, modelo, CitaDto.SolicitudId);

                    var consulta= (from c in modelo.Solicitud
                        where c.EsActivo && c.NumeroFolio == CitaDto.NumeroFolio
                              && c.EsActivo 
                        select c).AsQueryable();

                    if (!CualquierEstado)
                        consulta =consulta.Where(c => c.CatTipoEdoCitaId == Enumeracion.EnumTipoEdoCita.Pendiente);

                    var resultado = consulta.FirstOrDefault();

                    if (resultado != null) {
                        CitaDto.SolicitudId = consulta.FirstOrDefault().SolicitudId;
                    }

                }

                CitaDto =
                (from c in modelo.Solicitud
                    join tu in modelo.TramiteUnidadAtencion on c.TramiteUnidadAtencionId equals tu
                        .TramiteUnidadAtencionId
                    join ua in modelo.UnidadAtencion on tu.UnidadAtencionId equals ua.UnidadAtencionId
                    join sd in modelo.CatTipoEntidad on ua.CatTipoEntidadId equals sd.CatTipoEntidadId                    
                    where c.SolicitudId == CitaDto.SolicitudId && c.EsActivo
                    select new CitaDTO
                    {
                        NumeroFolio = c.NumeroFolio,
                        Fecha = c.FechaCita,
                        HoraInicio = c.Horario.HoraInicio.ToString(),
                        Unidad_Atencion = ua.Descripcion,
                        SolicitudId = c.SolicitudId,
                        Unidad_Medica = c.UnidadMedica.Descripcion,                        
                        DomicilioId = ua.DomicilioId,
                        CatTipoTramiteId = tu.CatTipoTramiteId,
                        Estado = sd.Concepto
                    }).FirstOrDefault();

                if (CitaDto != null)
                {
                    GetDomicilio(CitaDto.DomicilioId, CitaDto);
                    CitaDto.Promovente = ConsultaPersona(CitaDto.SolicitudId, modelo,
                        Enumeracion.EnumTipoInvolucrado.Promovente);
                    CitaDto.Paciente = ConsultaPersona(CitaDto.SolicitudId, modelo,
                        Enumeracion.EnumTipoInvolucrado.Paciente);
                    var requisitoDao = new RequisitoDAO();
                    CitaDto.Requisitos = requisitoDao.ConsultaRequisitos(CitaDto.CatTipoTramiteId);
                }

                return CitaDto;
            }
        }


        //PEER_REV ver si puede reducirse la consulta
        /// <summary>
        ///     Bloqueo de una persona cuando se excede un número de intentos
        /// </summary>
        /// <param name="PersonaDto"></param>
        /// <returns></returns>
        public bool BloqueoPersona(PersonaDTO PersonaDto)
        {
            using (var modelo = new ISSSTEEntities())
            {

                var BaseQuery =
                    (from a in modelo.Persona
                     join b in modelo.Involucrado on a.PersonaId equals b.PersonaId
                     join s in modelo.Solicitud on b.SolicitudId equals s.SolicitudId
                     where s.TramiteUnidadAtencion.CatTipoTramiteId == PersonaDto.CatTipoTramiteId &&
                           a.CURP.Equals(PersonaDto.CURP) && a.Nombre.Equals(PersonaDto.Nombre) && a.Paterno.Equals(PersonaDto.Paterno)
                     select new { persona = a, involucrado = b, solicitud = s }).AsQueryable();

                var fechaDesbloqueo = BaseQuery
                    .Select(A => A.persona.FechaDesbloqueo)
                    .Max();

                BaseQuery = BaseQuery.Where(s =>
                        s.involucrado.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente &&
                        s.involucrado.Solicitud.CatTipoEdoCitaId == Enumeracion.EnumTipoEdoCita.NoAsistio
                        && s.involucrado.Solicitud.EsActivo);

                if (fechaDesbloqueo.HasValue)
                    BaseQuery = BaseQuery.Where(s =>s.involucrado.Solicitud.FechaRegistro > fechaDesbloqueo);

                var numeroIntentos = GetNumeroIntentos();
                var numeroCitasPersona = BaseQuery.Count();

                if (numeroCitasPersona >= numeroIntentos)
                {
                    var personaInvolucrada = (from a in modelo.Persona join b in modelo.Involucrado
                                   on a.PersonaId equals b.PersonaId 
                                   where a.CURP == PersonaDto.CURP select b).FirstOrDefault();

                    if (personaInvolucrada != null) personaInvolucrada.Persona.EsUsuarioBloqueado = true;

                    modelo.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        private int GetNumeroIntentos()
        {
            int numeroIntentos;
            var configuracionDao = new ConfiguracionDAO();
            var valor = configuracionDao.GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.IntentosCita);
            var result = int.TryParse(valor, out numeroIntentos);
            if (!result)
                numeroIntentos = 3;
            return numeroIntentos;
        }

        /// <summary>
        ///     Citas vigentes por trámite
        /// </summary>
        /// <param name="PersonaDto"></param>
        /// <returns></returns>
        public int CitasVigentesPorTramite(PersonaDTO PersonaDto)
        {
            using (var modelo = new ISSSTEEntities())
            {
                return (from a in modelo.Persona
                    join b in modelo.Involucrado on a.PersonaId equals b.PersonaId
                    join s in modelo.Solicitud on b.SolicitudId equals s.SolicitudId
                    where b.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente &&
                          s.CatTipoEdoCitaId == Enumeracion.EnumTipoEdoCita.Pendiente &&
                          a.CURP.Equals(PersonaDto.CURP) && a.Nombre.Equals(PersonaDto.Nombre) && a.Paterno.Equals(PersonaDto.Paterno)
                          && s.EsActivo && s.TramiteUnidadAtencion.CatTipoTramiteId == PersonaDto.CatTipoTramiteId
                    select s).Count();
            }
        }

        /// <summary>
        ///     Cancelación automática de las citas que no se llevaron a cabo
        ///     Idealmente, el día anterior
        ///     Caso 1. Citas del dia anterior
        ///     Caso 2. Citas del dia, anteriores a la hora actual
        ///     Caso 3. Citas del dia, iguales a la hora actual y menores al minuto actual
        /// </summary>
        public async Task CancelacionAutomaticaDelDia()
        {
            using (var modelo = new ISSSTEEntities())
            {
                var fechaHoy = DateTime.Now;

                var citas = await (from c in modelo.Solicitud
                    where(((DbFunctions.TruncateTime(c.FechaCita).Value < DbFunctions.TruncateTime(fechaHoy).Value) ||
                          (DbFunctions.TruncateTime(c.FechaCita).Value == DbFunctions.TruncateTime(fechaHoy).Value
                           && c.Horario.HoraInicio.Hours < fechaHoy.Hour) ||
                          (DbFunctions.TruncateTime(c.FechaCita).Value == DbFunctions.TruncateTime(fechaHoy).Value 
                          && c.Horario.HoraInicio.Hours == fechaHoy.Hour 
                          && c.Horario.HoraInicio.Minutes <= fechaHoy.Minute))
                          && c.EsActivo
                          && c.CatTipoEdoCitaId == Enumeracion.EnumTipoEdoCita.Pendiente)
                    select c).ToListAsync();


                foreach (var cita in citas)
                    cita.CatTipoEdoCitaId = Enumeracion.EnumTipoEdoCita.NoAsistio;

                await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        ///     Cancelación automática de una cita vencida a no asistida
        /// </summary>
        /// <param name="NumeroFolio"></param>
        /// <param name="Modelo"></param>
        /// <param name="SolicitudId"></param>
        /// <returns></returns>
        private void CancelacionAutomatica(string NumeroFolio, ISSSTEEntities Modelo, int SolicitudId)
        {
            var solicitud = SolicitudId == 0
                ? (from c in Modelo.Solicitud
                    where c.EsActivo && c.NumeroFolio == NumeroFolio
                          && c.EsActivo
                    select c).FirstOrDefault()
                : (from c in Modelo.Solicitud
                    where c.EsActivo && c.SolicitudId == SolicitudId
                          && c.EsActivo
                    select c).FirstOrDefault();

            if (solicitud == null)
                return;

            if (solicitud.FechaCita.Date <=DateTime.Now.Date && solicitud.Horario.HoraInicio.Hours <= DateTime.Now.Hour &&
                solicitud.CatTipoEdoCitaId == Enumeracion.EnumTipoEdoCita.Pendiente)
            {
                solicitud.CatTipoEdoCitaId = Enumeracion.EnumTipoEdoCita.NoAsistio;
                Modelo.SaveChanges();
            }
        }

        /// <summary>
        ///     Cancelación de una cita manualmente
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <returns></returns>
        public string CancelarCita(GeneralCitaDTO GeneralCitaDto)
        {
            using (var modelo = new ISSSTEEntities())
            {
                CancelacionAutomatica(GeneralCitaDto.NumeroFolio, modelo, GeneralCitaDto.SolicitudId);

                var horarioDto = (from a in modelo.Solicitud
                    where a.SolicitudId == GeneralCitaDto.SolicitudId
                          && a.CatTipoEdoCitaId == Enumeracion.EnumTipoEdoCita.Pendiente
                          && a.EsActivo
                    select new HorarioDTO
                    {
                        Fecha = a.FechaCita,
                        HoraInicio = a.Horario.HoraInicio,
                        EsActivo = a.EsActivo
                    }).FirstOrDefault();

                if (horarioDto != null && horarioDto.EsActivo)
                {
                    double horasPrevias, horas;
                    DiferenciaHorario(horarioDto, out horasPrevias, out horas);
                    if (horasPrevias <= horas)
                    {
                        var citaAgendada = (from a in modelo.Solicitud
                            where a.SolicitudId == GeneralCitaDto.SolicitudId
                            select a).FirstOrDefault();
                        if (citaAgendada != null)
                        {
                            citaAgendada.CatTipoEdoCitaId = Enumeracion.EnumTipoEdoCita.Cancelado;
                            citaAgendada.EsActivo = false;
                        }
                        modelo.SaveChanges();
                        return Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CitaCancelada];
                    }
                    return Enumeracion.EnumVarios.Prefijo +
                           Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CitaNoEliminada];
                }
                return horarioDto == null
                    ? Enumeracion.EnumVarios.Prefijo +
                      Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CitaEliminada]
                    : Enumeracion.EnumVarios.Prefijo +
                      Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CitaNoEncontrada];
            }
        }

        /// <summary>
        ///     Reinicio de contador - Número de Folio
        /// </summary>
        public async Task ReinicioContador()
        {
            if (DateTime.Now.Month != 1 && DateTime.Now.Month != 2 &&
                DateTime.Now.Month != 12 && DateTime.Now.Month != 11) return;

            using (var modelo = new ISSSTEEntities())
            {
                if (DateTime.Now.Month == 12 || DateTime.Now.Month == 11)
                {
                    var unidades = await (from a in modelo.UnidadAtencion
                        where a.ReiniciarContador == false
                        select a).ToListAsync();

                    foreach (var unidad in unidades)
                        unidad.ReiniciarContador = true;

                    await modelo.SaveChangesAsync();
                }
                else if (DateTime.Now.Month == 1 || DateTime.Now.Month == 2)
                {
                    var unidades = (from a in modelo.UnidadAtencion
                        where a.ReiniciarContador
                        select a).ToList();

                    foreach (var unidad in unidades)
                    {
                        unidad.ReiniciarContador = false;
                        unidad.Contador = 1;
                    }

                    await modelo.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        ///     Diferencia en horas de una cita para poder ser cancelada
        /// </summary>
        /// <param name="HorarioDto"></param>
        /// <param name="HorasAntelacion"></param>
        /// <param name="Horas"></param>
        private void DiferenciaHorario(HorarioDTO HorarioDto, out double HorasAntelacion, out double Horas)
        {
            HorarioDto.Fecha = HorarioDto.Fecha.Date.Add(HorarioDto.HoraInicio);
            var fecha = DateTime.Today;

            var diferencia = HorarioDto.Fecha - fecha;

            Horas = diferencia.TotalHours;
            var configuracionDao = new ConfiguracionDAO();
            var valor = configuracionDao.GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.LimiteCancelacionCita);
            var result = double.TryParse(valor, out HorasAntelacion);
            if (!result)
                HorasAntelacion = 24;
        }

        /// <summary>
        ///     Agenda de una cita
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <returns></returns>
        public string AgendarCita(GeneralCitaDTO GeneralCitaDto)
        {
            using (var modelo = new ISSSTEEntities())
            {
                using (var modeloTransaction = modelo.Database.BeginTransaction())
                {
                    try
                    {
                        var fecha = Convert.ToDateTime(GeneralCitaDto.FechaFinal);
                        var horarioId = GeneralCitaDto.HorarioId;

                        var conteoCitas = (from a in modelo.Solicitud
                            where a.TramiteUnidadAtencionId ==
                                  GeneralCitaDto.TramiteUnidadAtencionId
                                  && a.FechaCita.Year == fecha.Year
                                  && a.FechaCita.Month == fecha.Month
                                  && a.FechaCita.Day == fecha.Day
                                  && a.HorarioId == horarioId
                                  && a.EsActivo
                            select a).Count();

                        var capacidadCita = (from a in modelo.Horario
                            where a.TramiteUnidadAtencionId ==
                                  GeneralCitaDto.TramiteUnidadAtencionId
                                  && a.HorarioId == horarioId
                            select a.Capacidad).FirstOrDefault();

                        if (capacidadCita > conteoCitas)
                        {
                            var numeroFolio = GetNumeroFolio(GeneralCitaDto);

                            var solicitud = AddSolicitud(numeroFolio, horarioId, fecha, GeneralCitaDto, modelo);
                            var persona1 = GeneralCitaDto.EsRepresentante
                                ? AddPaciente(GeneralCitaDto, modelo)
                                : AddPromovente(GeneralCitaDto, modelo);
                            var persona2 = AddPromovente(GeneralCitaDto, modelo);
                            AddInvolucrado(modelo, solicitud.SolicitudId, persona1.PersonaId,
                                Enumeracion.EnumTipoInvolucrado.Paciente);
                            AddInvolucrado(modelo, solicitud.SolicitudId, persona2.PersonaId,
                                Enumeracion.EnumTipoInvolucrado.Promovente);

                            modeloTransaction.Commit();
                            return solicitud.NumeroFolio;
                        }
                        return Enumeracion.EnumVarios.Prefijo +
                               Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.CupoExcedido];
                    }
                    catch (Exception ex)
                    {
                        modeloTransaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        ///     Cálculo del número de folio
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <returns></returns>
        private string GetNumeroFolio(GeneralCitaDTO GeneralCitaDto)
        {
            var numeroFolio = "Parte1/Parte2/Parte3/Parte4";
            var tua = GetContadorUA(GeneralCitaDto.TramiteUnidadAtencionId);

            var hora = "" + DateTime.Now.Hour;
            if (hora.Length < 2) hora = "0" + hora;

            var minutos = "" + DateTime.Now.Minute;
            if (minutos.Length < 2) minutos = "0" + minutos;

            var parte3 = hora + ":" + minutos;
            var parte4 = "" + tua.Contador;
            string parte2;
            try
            {
                parte2 = DateTime.Now.ToString(tua.Mascara, CultureInfo.InvariantCulture);
            }
            catch
            {
                parte2 = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);
            }
            var parte1 = string.IsNullOrEmpty(tua.Prefijo)
                ? "TUA_" + GeneralCitaDto.TramiteUnidadAtencionId
                : tua.Prefijo;
            numeroFolio = numeroFolio.Replace("Parte1", parte1);
            numeroFolio = numeroFolio.Replace("Parte2", parte2);
            numeroFolio = numeroFolio.Replace("Parte3", parte3);
            numeroFolio = numeroFolio.Replace("Parte4", parte4);
            return numeroFolio;
        }

        /// <summary>
        ///     Contador por Unidad Administrativa y tipo de trámite
        /// </summary>
        /// <param name="TramiteUnidadAtencionId"></param>
        /// <returns></returns>
        private UnidadAtencion GetContadorUA(int TramiteUnidadAtencionId)
        {
            UnidadAtencion auxTua;
            using (var modelo = new ISSSTEEntities())
            {
                auxTua = (from a in modelo.TramiteUnidadAtencion
                    where a.TramiteUnidadAtencionId == TramiteUnidadAtencionId
                    select a.UnidadAtencion).FirstOrDefault();

                if (auxTua != null) auxTua.Contador++;
                modelo.SaveChanges();
            }
            if (auxTua != null)
                auxTua.Contador--;
            return auxTua;
        }

        /// <summary>
        ///     Agregar involucrados
        /// </summary>
        /// <param name="Modelo"></param>
        /// <param name="SolicitudId"></param>
        /// <param name="PersonaId"></param>
        /// <param name="CatTipoPersona"></param>
        private void AddInvolucrado(ISSSTEEntities Modelo, int SolicitudId, int PersonaId, int CatTipoPersona)
        {
            var involucrado = new Involucrado
            {
                EsActivo = true,
                SolicitudId = SolicitudId,
                CatTipoInvolucradoId = CatTipoPersona,
                PersonaId = PersonaId
            };
            Modelo.Involucrado.Add(involucrado);
            Modelo.SaveChanges();
        }

        /// <summary>
        ///     Se agrega al promovente
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        private Persona AddPromovente(GeneralCitaDTO GeneralCitaDto, ISSSTEEntities Modelo)
        {
            var persona = (from a in Modelo.Persona
                              where a.CURP == GeneralCitaDto.CURPPromovente &&
                                    a.Nombre == GeneralCitaDto.NombrePromovente &&
                                    a.Paterno == GeneralCitaDto.Apellido1Promovente 
                                    //&& a.Materno == GeneralCitaDto.Apellido2Promovente
                              select a).FirstOrDefault() ?? new Persona();

            persona.Nombre = GeneralCitaDto.NombrePromovente;
            persona.Paterno = GeneralCitaDto.Apellido1Promovente;
            persona.Materno = GeneralCitaDto.Apellido2Promovente;
            persona.CURP = GeneralCitaDto.CURPPromovente;
            persona.Correo = GeneralCitaDto.CorreoPromovente;
            persona.Telefono = GeneralCitaDto.TelefonoPromovente;
            persona.TelefonoMovil = GeneralCitaDto.CelularPromovente;
            persona.EsActivo = true;
            persona.Lada = GeneralCitaDto.LadaPromovente;
            persona.Edad = Utilerias.GetEdad(GeneralCitaDto.CURPPromovente);
            persona.EsMasculino = Utilerias.GetSexo(GeneralCitaDto.CURPPromovente);
            persona.FechaNacimiento = Utilerias.GetFechaNacimiento(GeneralCitaDto.CURPPromovente);

            if (persona.PersonaId == 0)
                Modelo.Persona.Add(persona);
            Modelo.SaveChanges();
            return persona;
        }

        /// <summary>
        ///     Se agrega al paciente
        /// </summary>
        /// <param name="GeneralCitaDto"></param>
        /// <param name="Modelo"></param>
        /// <returns></returns>
        private Persona AddPaciente(GeneralCitaDTO GeneralCitaDto, ISSSTEEntities Modelo)
        {
            var persona = (from a in Modelo.Persona
                              where a.CURP == GeneralCitaDto.CURPPaciente &&
                                    a.Nombre == GeneralCitaDto.NombrePaciente &&
                                    a.Paterno == GeneralCitaDto.Apellido1Paciente 
                                    //&& a.Materno == GeneralCitaDto.Apellido2Paciente
                              select a).FirstOrDefault() ?? new Persona();

            persona.Nombre = GeneralCitaDto.NombrePaciente;
            persona.Paterno = GeneralCitaDto.Apellido1Paciente;
            persona.Materno = GeneralCitaDto.Apellido2Paciente;
            persona.CURP = GeneralCitaDto.CURPPaciente;
            persona.Correo = GeneralCitaDto.CorreoPaciente;
            persona.Telefono = GeneralCitaDto.TelefonoPaciente;
            persona.TelefonoMovil = GeneralCitaDto.CelularPaciente;
            persona.EsActivo = true;
            persona.Lada = GeneralCitaDto.LadaPaciente;
            persona.Edad = Utilerias.GetEdad(GeneralCitaDto.CURPPaciente);
            persona.EsMasculino = Utilerias.GetSexo(GeneralCitaDto.CURPPaciente);
            persona.FechaNacimiento = Utilerias.GetFechaNacimiento(GeneralCitaDto.CURPPaciente);

            if (persona.PersonaId == 0)
                Modelo.Persona.Add(persona);
            Modelo.SaveChanges();
            return persona;
        }

        /// <summary>
        ///     Se agrega una nueva solicitud
        /// </summary>
        /// <param name="FechaCita"></param>
        /// <param name="GeneralCitaDto"></param>
        /// <param name="Modelo"></param>
        /// <param name="NumeroFolio"></param>
        /// <param name="HorarioId"></param>
        /// <returns></returns>
        private Solicitud AddSolicitud(
            string NumeroFolio, int HorarioId, DateTime FechaCita,
            GeneralCitaDTO GeneralCitaDto, ISSSTEEntities Modelo)
        {
            var solicitud = new Solicitud
            {
                EsActivo = true,
                TramiteUnidadAtencionId = GeneralCitaDto.TramiteUnidadAtencionId,
                NumeroFolio = NumeroFolio,
                CatTipoEdoCitaId = Enumeracion.EnumTipoEdoCita.Pendiente,
                HorarioId = HorarioId,
                FechaCita = FechaCita,
                FechaRegistro = DateTime.Now,
                UnidadMedicaId = GeneralCitaDto.UnidadMedicaId
            };
            Modelo.Solicitud.Add(solicitud);
            Modelo.SaveChanges();
            return solicitud;
        }

        /// <summary>
        ///     Obtención del domicilio asociado a una TUA
        /// </summary>
        public void GetDomicilio(int? DomicilioId, CitaDTO CitaDto)
        {
            int tramiteUnidadAtencionId = CitaDto.TramiteUnidadAtencionId;
            using (var modelo = new ISSSTEEntities())
            {
                if (DomicilioId != null && DomicilioId == 0)
                    DomicilioId =
                    (from tu in modelo.TramiteUnidadAtencion
                        join ua in modelo.UnidadAtencion on tu.UnidadAtencionId equals ua.UnidadAtencionId
                        where tu.TramiteUnidadAtencionId == tramiteUnidadAtencionId
                        select ua.DomicilioId).FirstOrDefault();

                if (DomicilioId != null && DomicilioId != 0)
                {
                    var domicilio = (from a in modelo.Domicilio
                        where a.DomicilioId == DomicilioId
                        select a).FirstOrDefault();

                    if (domicilio != null)
                    {
                        CitaDto.CodigoPostal = domicilio.CodigoPostal ?? 0;
                        CitaDto.Colonia = domicilio.Colonia;
                        CitaDto.Municipio = domicilio.Municipio;
                        CitaDto.Calle = domicilio.Calle;
                        CitaDto.NumeroExterior = domicilio.NumeroExterior;
                        CitaDto.NumeroInterior = domicilio.NumeroInterior;

                        string valor = ""+CitaDto.CodigoPostal;
                        switch (valor.Length)
                        {
                            case 1: valor = "0000" + valor; break;
                            case 2: valor = "000" + valor; break;
                            case 3: valor = "00" + valor; break;
                            case 4: valor = "0" + valor; break;
                        }
                        CitaDto.CodPostal = valor;

                        var entidad = (from tu in modelo.TramiteUnidadAtencion
                            join ua in modelo.UnidadAtencion on tu.UnidadAtencionId equals ua.UnidadAtencionId
                            where tu.TramiteUnidadAtencionId == tramiteUnidadAtencionId
                            select ua.CatTipoEntidad.Concepto).FirstOrDefault();

                        if (!string.IsNullOrEmpty(entidad))
                        {
                            CitaDto.Estado = entidad;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Consulta de una persona
        /// </summary>
        /// <param name="SolicitudId"></param>
        /// <param name="Modelo"></param>
        /// <param name="CatTipoInvolucradoId"></param>
        /// <returns></returns>
        private PersonaDTO ConsultaPersona(int SolicitudId, ISSSTEEntities Modelo, int CatTipoInvolucradoId)
        {
            var personaDto = (from c in Modelo.Solicitud
                join I in Modelo.Involucrado on c.SolicitudId equals I.SolicitudId
                where I.CatTipoInvolucradoId == CatTipoInvolucradoId
                      && c.SolicitudId == SolicitudId && I.EsActivo
                select new PersonaDTO
                {
                    Nombre = I.Persona.Nombre,
                    Paterno = I.Persona.Paterno,
                    Materno = I.Persona.Materno,
                    CURP = I.Persona.CURP,
                    EsMasculino = I.Persona.EsMasculino,
                    FechaNacimiento = I.Persona.FechaNacimiento,
                    PersonaId = I.Persona.PersonaId,
                    Lada = I.Persona.Lada,
                    TelFijo = I.Persona.Telefono,
                    TelMovil = I.Persona.TelefonoMovil,
                    CorreoElectronico = I.Persona.Correo
                }).FirstOrDefault();

            if (personaDto != null)
            {
                personaDto.EsMasculino = Utilerias.GetSexo(personaDto.CURP);
                personaDto.FechaNacimiento = Utilerias.GetFechaNacimiento(personaDto.CURP);
            }
            return personaDto;
        }
    }
}