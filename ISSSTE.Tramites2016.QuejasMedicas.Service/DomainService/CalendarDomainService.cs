using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.DAL;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.CalendarioDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.DomainService
{
    public class CalendarDomainService : ICalendarDomainService
    {
        /// <summary>
        ///     Obtiene los horarios programados por tramite/unidad administrativa
        /// </summary>
        /// <param name="SolicitudId"></param>
        /// <param name="UnidadAtencionId"></param>
        /// <returns></returns>
        public async Task<List<HorarioApiDTO>> ObtenerHorariosAsync(int? SolicitudId, int UnidadAtencionId)
        {
            using (var context = new ISSSTEEntities())
            {
                var query = context.Horario.Include(S => S.CatTipoDiaSemana)
                    .AsQueryable();

                if (SolicitudId.HasValue)
                    query = query.Where(S => S.TramiteUnidadAtencion.CatTipoTramiteId == SolicitudId);

                query = query.Where(S => !S.EsDiaEspecial && S.EsActivo &&
                                         S.TramiteUnidadAtencion.UnidadAtencionId.Equals(UnidadAtencionId));

                var result = query
                    .GroupBy(S => new { S.CatTipoDiaSemana.CatTipoDiaSemanaId, S.HoraInicio })
                    .Select(
                        S => new HorarioApiDTO
                        {
                            //ScheduleId = s.HorarioId,
                            DelegationId = UnidadAtencionId,
                            Time = S.Key.HoraInicio,
                            //WeekDay = s.Where(d => d.CatTipoDiaSemana.CatTipoDiaSemanaId==s.Key.CatTipoDiaSemanaId).FirstOrDefault().CatTipoDiaSemana.Dia,
                            WeekdayId = S.Key.CatTipoDiaSemanaId,
                            Capacity = S.FirstOrDefault() != null ? S.FirstOrDefault().Capacidad : default(int)
                        }
                    );

                return await result.OrderBy(R => R.WeekdayId).ThenBy(R => R.Time).ToListAsync();
            }
        }


        /// <summary>
        ///     Obtiene los dias especiales programados por tramite/unidad administrativa
        ///     En caso de no seleccionarse el tramite obtiene todos los horarios de los  tramites en la unidad especificada
        /// </summary>
        /// <param name="SolicitudId"></param>
        /// <param name="UnidadAtencionId"></param>
        /// <returns></returns>
        public async Task<List<HorarioDiaEspecialApiDTO>> ObtenerDiasEspecialesAsync(int? SolicitudId,
            int UnidadAtencionId)
        {
            using (var context = new ISSSTEEntities())
            {
                var diasEspecialesLista = new List<HorarioDiaEspecialApiDTO>();

                var query = context.DiaEspecial
                    .Join(context.Horario,
                        Esp => Esp.DiaEspecialId, Hor => Hor.DiaEspecialId,
                        (Esp, Hor) => new { esp = Esp, hor = Hor }
                    )
                    .AsQueryable();

                if (SolicitudId.HasValue)
                    query = query.Where(H => H.hor.TramiteUnidadAtencion.CatTipoTramiteId == SolicitudId);

                query = query
                    .Where(H => H.hor.EsDiaEspecial && H.esp.EsActivo && H.hor.EsActivo &&
                                H.hor.TramiteUnidadAtencion.UnidadAtencionId.Equals(UnidadAtencionId));

                var result = await query.GroupBy(S => S.esp.Fecha)
                    .Select(D => new
                    {
                        Fecha = D.Key,
                        horarios = D.Where(S => S.hor.EsActivo)
                            .Select(S => new { S.hor.HoraInicio, S.hor.Capacidad })
                            .Distinct()
                            .ToList()
                    })
                    .ToListAsync();


                result.ForEach(Planificacion =>
                {
                    diasEspecialesLista.Add(
                        new HorarioDiaEspecialApiDTO
                        {
                            DiaEspecialDTO = new HorarioDiaEspecialDTO
                            {
                                Date = Planificacion.Fecha,
                                IsOverrided = true,
                                IsNonWorking = false,
                                DelegationId = UnidadAtencionId
                            },
                            Schedules = Planificacion.horarios.Select(S => new HorarioDiaEspecialDTO
                            {
                                Capacity = S.Capacidad,
                                DelegationId = UnidadAtencionId,
                                Time = S.HoraInicio
                                //DiaEspecialDTOScheduleId = s.HorarioId
                            })
                                .ToList()
                        }
                    );
                });


                return diasEspecialesLista;
            }
        }


        /// <summary>
        ///     Obtiene los dias no laborables programados por tramite/unidad administrativa
        /// </summary>
        /// <param name="SolicitudId"></param>
        /// <param name="UnidadAtencionId"></param>
        /// <returns></returns>
        public async Task<List<HorarioDiaEspecialDTO>> ObtenerDiasNoLaborablesAsync(int? SolicitudId,
            int UnidadAtencionId)
        {
            using (var context = new ISSSTEEntities())
            {
                var query = context.DiaNoLaborable.AsQueryable();

                if (SolicitudId.HasValue)
                    query = query.Where(H => H.TramiteUnidadAtencion.CatTipoTramiteId == SolicitudId);

                var resultado = query.Where(R => R.EsActivo &&
                                                 R.TramiteUnidadAtencion.UnidadAtencion.UnidadAtencionId ==
                                                 UnidadAtencionId)
                    .GroupBy(S => new { S.Fecha })
                    .Select(
                        S => new HorarioDiaEspecialDTO
                        {
                            Date = S.Key.Fecha,
                            DelegationId = UnidadAtencionId,
                            IsNonWorking = true
                        }
                    );

                return await resultado.OrderBy(R => R.Date).ToListAsync();
            }
        }


        /// <summary>
        ///     Guarda los horarios por tramite/unidad administrativa
        /// </summary>
        /// <param name="Horarios"></param>
        /// <returns></returns>
        public async Task<int> GuardarHorariosAsync(List<Schedule> Horarios)
        {
            using (var context = new ISSSTEEntities())
            {
                Horarios.ForEach(Horario =>
                {
                    var query = context.TramiteUnidadAtencion
                        .Where(S => S.UnidadAtencionId == Horario.DelegationId)
                        .AsQueryable();

                    // Si no se seleccciona un tramite, el horario sera agregado a todos los trámites existentes
                    if (Horario.RequestTypeId.HasValue)
                        query = query.Where(H => H.CatTipoTramiteId == Horario.RequestTypeId);

                    var tramitesUnidadesAtencion = query.ToList();

                    tramitesUnidadesAtencion.ForEach(Tu =>
                    {
                        var horarioNuevo =
                            new Horario
                            {
                                EsActivo = true,
                                TramiteUnidadAtencionId = Tu.TramiteUnidadAtencionId,
                                CatTipoDiaSemanaId = Horario.WeekdayId,
                                HoraInicio = Horario.Time,
                                EsDiaEspecial = false,
                                Capacidad = Horario.Capacity
                            };


                        var resultadoHorario =
                            context.Horario.FirstOrDefault(
                                S => S.HoraInicio.Equals(Horario.Time) && S.CatTipoDiaSemanaId == Horario.WeekdayId &&
                                     S.TramiteUnidadAtencionId == Tu.TramiteUnidadAtencionId);

                        //Si ya existe el horario no es agregado
                        if (resultadoHorario == null)
                        {
                            context.Horario.Add(horarioNuevo);
                        }
                        else
                        {
                            horarioNuevo.HorarioId = resultadoHorario.HorarioId;
                            context.Horario.AddOrUpdate(horarioNuevo);
                        }
                    });
                });


                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> GuardarHorariosDiasEspecialesAsync(List<HorarioDiaEspecialDTO> HorariosDiasEspeciales)
        {
            using (var context = new ISSSTEEntities())
            {
                HorariosDiasEspeciales.ForEach(NuevoDiaEspecial =>
                {
                    var query = context.TramiteUnidadAtencion
                        .Where(S => S.UnidadAtencionId == NuevoDiaEspecial.DelegationId)
                        .AsQueryable();

                    // Si no se seleccciona un tramite, el horario sera agregado a todos los trámites existentes
                    if (NuevoDiaEspecial.RequestTypeId.HasValue)
                        query = query.Where(H => H.CatTipoTramiteId == NuevoDiaEspecial.RequestTypeId);

                    var tramitesUnidadesAtencion = query.ToList();

                    tramitesUnidadesAtencion.ForEach(Tu =>
                    {
                        var firstOrDefault = context.DiaEspecial.FirstOrDefault(S => S.Fecha == NuevoDiaEspecial.Date);
                        if (firstOrDefault != null)
                        {
                            var horarioNuevo = new Horario
                            {
                                //HorarioId = nuevoDiaEspecial.DiaEspecialDTOScheduleId,
                                Capacidad = NuevoDiaEspecial.Capacity,
                                EsActivo = true,
                                TramiteUnidadAtencionId = Tu.TramiteUnidadAtencionId,
                                CatTipoDiaSemanaId = null,
                                HoraInicio = NuevoDiaEspecial.Time,
                                EsDiaEspecial = true,
                                DiaEspecialId = firstOrDefault.DiaEspecialId
                            };

                            var resultadoHorarioDia =
                                context.Horario.FirstOrDefault(
                                    S => S.HoraInicio.Equals(NuevoDiaEspecial.Time) && S.EsDiaEspecial &&
                                         S.TramiteUnidadAtencionId == Tu.TramiteUnidadAtencionId);

                            //Si ya existe el horario especial no es agregado
                            if (resultadoHorarioDia == null)
                            {
                                context.Horario.Add(horarioNuevo);
                            }
                            else
                            {
                                horarioNuevo.HorarioId = resultadoHorarioDia.HorarioId;
                                context.Horario.AddOrUpdate(horarioNuevo);
                            }
                        }
                    });
                });

                return await context.SaveChangesAsync();
            }
        }


        public async Task<int> GuardaDiasNoLaboralesOEspecialesAsync(List<HorarioDiaEspecialDTO> SpecialDays)
        {
            using (var context = new ISSSTEEntities())
            {
                SpecialDays.ForEach(Dia =>
                {
                    if (!Dia.IsNonWorking && Dia.IsOverrided)
                    {
                        var diasEspecialNuevo = new DiaEspecial
                        {
                            EsActivo = true,
                            Fecha = Dia.Date,
                            Concepto = "sin Concepto"
                        };


                        var resultadoDiaEspecial = context.DiaEspecial.FirstOrDefault(S => S.Fecha == Dia.Date);

                        if (resultadoDiaEspecial == null)
                        {
                            context.DiaEspecial.Add(diasEspecialNuevo);
                        }
                        else
                        {
                            diasEspecialNuevo.DiaEspecialId = resultadoDiaEspecial.DiaEspecialId;
                            context.DiaEspecial.AddOrUpdate(diasEspecialNuevo);
                        }
                    }
                    else if (Dia.IsNonWorking)
                    {
                        var query = context.TramiteUnidadAtencion
                            .Where(S => S.UnidadAtencionId == Dia.DelegationId)
                            .AsQueryable();

                        // Si no se seleccciona un tramite, el horario sera agregado a todos los trámites existentes
                        if (Dia.RequestTypeId.HasValue)
                            query = query.Where(H => H.CatTipoTramiteId == Dia.RequestTypeId);

                        var tramitesUnidadesAtencion = query.ToList();


                        tramitesUnidadesAtencion.ForEach(Tu =>
                        {
                            var diaNoLaborable = new DiaNoLaborable
                            {
                                EsActivo = true,
                                Fecha = Dia.Date,
                                TramiteUnidadAtencionId = Tu.TramiteUnidadAtencionId
                            };

                            var resuldatoDia =
                                context.DiaNoLaborable.FirstOrDefault(
                                    S => S.Fecha == Dia.Date &&
                                         S.TramiteUnidadAtencionId == Tu.TramiteUnidadAtencionId);

                            if (resuldatoDia == null)
                            {
                                context.DiaNoLaborable.Add(diaNoLaborable);
                            }
                            else
                            {
                                diaNoLaborable.DiaNoLaborableId = resuldatoDia.DiaNoLaborableId;
                                context.DiaNoLaborable.AddOrUpdate(diaNoLaborable);
                            }
                        });
                    }
                });


                return await context.SaveChangesAsync();
            }
        }


        /// <summary>
        ///     Elimina el horario que haya sido removido de la lista
        /// </summary>
        /// <param name="SolicitudId"></param>
        /// <param name="UnidadAtencionId"></param>
        /// <param name="HorarioInicio"></param>
        /// <param name="CatTipoDiaSemanaId"></param>
        /// <param name="EsDiaEspecial"></param>
        /// <returns></returns>
        public async Task<int> EliminarHorarios(int? SolicitudId, int UnidadAtencionId, TimeSpan HorarioInicio,
            int? CatTipoDiaSemanaId, bool EsDiaEspecial)
        {
            using (var context = new ISSSTEEntities())
            {
                var query = context.TramiteUnidadAtencion
                    .Where(S => S.UnidadAtencionId == UnidadAtencionId)
                    .AsQueryable();

                // Si no se seleccciona un tramite, el horario sera agregado a todos los trámites existentes
                if (SolicitudId.HasValue)
                    query = query.Where(H => H.CatTipoTramiteId == SolicitudId);

                var tramitesUnidadesAtencion = query.ToList();


                tramitesUnidadesAtencion.ForEach(Tu =>
                {
                    var special =
                        context.Horario.FirstOrDefault(S => S.EsDiaEspecial == EsDiaEspecial &&
                                                            S.HoraInicio == HorarioInicio &&
                                                            S.CatTipoDiaSemanaId == CatTipoDiaSemanaId &&
                                                            S.TramiteUnidadAtencion.TramiteUnidadAtencionId ==
                                                            Tu.TramiteUnidadAtencionId);

                    if (special != null && special.Solicitud.Any())
                    {
                        special.EsActivo = false;

                        context.Horario.AddOrUpdate(special);
                    }
                    else
                    {
                        if (special != null) context.Horario.Remove(special);
                    }
                });

                return await context.SaveChangesAsync();
            }
        }


        public async Task<int> DeleteNonLaboraleOEspecialDaysAsync(HorarioDiaEspecialDTO DiaNoLaborableOEspecial)
        {
            using (var context = new ISSSTEEntities())
            {
                if (DiaNoLaborableOEspecial.IsNonWorking) // Es dia no laborable
                {
                    var query = context.TramiteUnidadAtencion
                        .Where(S => S.UnidadAtencionId == DiaNoLaborableOEspecial.DelegationId)
                        .AsQueryable();

                    // Si no se seleccciona un tramite, el horario sera agregado a todos los trámites existentes
                    if (DiaNoLaborableOEspecial.RequestTypeId.HasValue)
                        query = query.Where(H => H.CatTipoTramiteId == DiaNoLaborableOEspecial.RequestTypeId);

                    var tramitesUnidadesAtencion = query.ToList();

                    tramitesUnidadesAtencion.ForEach(Tu =>
                    {
                        var diaNoLaboral =
                            context.DiaNoLaborable.FirstOrDefault(
                                S => S.Fecha == DiaNoLaborableOEspecial.Date &&
                                     S.TramiteUnidadAtencionId == Tu.TramiteUnidadAtencionId);

                        if (diaNoLaboral != null) context.DiaNoLaborable.Remove(diaNoLaboral);
                    });
                }
                else
                {
                    //Es dia especial
                    var diaEspecial = context.DiaEspecial.FirstOrDefault(S => S.Fecha == DiaNoLaborableOEspecial.Date);

                    if (diaEspecial != null)
                    {
                        if (!diaEspecial.Horario.Any())
                        {
                            context.DiaEspecial.Remove(diaEspecial);
                        }
                        else
                        {
                            diaEspecial.EsActivo = false;
                            context.DiaEspecial.AddOrUpdate(diaEspecial);
                        }

                        diaEspecial.Horario.ToList()
                            .ForEach(HorarioDiaEspecial =>
                            {
                                HorarioDiaEspecial.EsActivo = false;
                                context.Horario.AddOrUpdate(HorarioDiaEspecial);
                            });
                    }
                }

                return await context.SaveChangesAsync();
            }
        }


        public async Task<CalendarDTO> GetAppointments(List<int> Delegations, DatesDTO Fechas, string Query = null,
            int? StatusId = null, int? RequestTypeId = null)
        {
            var calendar = new CalendarDTO();

            using (var context = new ISSSTEEntities())
            {
                var appointmentQuery =
                    context.Solicitud
                        .Join(context.TramiteUnidadAtencion,
                            Req => Req.TramiteUnidadAtencionId, Tua => Tua.TramiteUnidadAtencionId,
                            (Req, Tua) => new { req = Req, tua = Tua })
                        .Select(S => new
                        {
                            Solicitud = S.req,
                            TrammiteUnidadAtencio = S.tua,
                            Paciente = S.req.Involucrado.FirstOrDefault(
                                P => P.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente),
                            Promovente = S.req.Involucrado.FirstOrDefault(
                                P => P.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Promovente)
                        })
                        .AsQueryable();

                if (!string.IsNullOrEmpty(Query))
                    appointmentQuery = appointmentQuery
                        .Where(R => R.Solicitud.NumeroFolio.ToLower().Contains(Query.ToLower())
                                    || R.Paciente.Persona.Nombre.ToLower().Contains(Query.ToLower())
                                    || R.Promovente.Persona.Nombre.ToLower().Contains(Query.ToLower())
                        );

                if (!Delegations.Contains(Enumeracion.EnumVarios.TodasLasDelegaciones))
                    appointmentQuery = appointmentQuery
                        .Where(R => Delegations.Contains(R.TrammiteUnidadAtencio.UnidadAtencionId));

                if (StatusId.HasValue)
                    appointmentQuery = appointmentQuery.Where(R => R.Solicitud.CatTipoEdoCitaId == StatusId);

                if (RequestTypeId.HasValue)
                    appointmentQuery = appointmentQuery.Where(R => R.TrammiteUnidadAtencio.CatTipoTramiteId == RequestTypeId);

                if (Fechas.FechaInicio.HasValue && Fechas.FechaFin.HasValue)
                    appointmentQuery = appointmentQuery.Where(R => R.Solicitud.FechaCita >= Fechas.FechaInicio
                                                               && R.Solicitud.FechaCita <= Fechas.FechaFin);

                var result =
                    appointmentQuery.Select(S => new AppointmentResultDTO
                    {
                        Id = S.Solicitud.SolicitudId,
                        StartDate = S.Solicitud.FechaCita,
                        EndDate = S.Solicitud.FechaCita, //MFP revisar                          
                        SolicitudId = S.Solicitud.SolicitudId,
                        NumeroFolio = S.Solicitud.NumeroFolio,
                        HoraCita = S.Solicitud.Horario != null ? S.Solicitud.Horario.HoraInicio : default(TimeSpan),
                        ColorTramite = S.TrammiteUnidadAtencio.CatTipoTramite.Semaforo ?? string.Empty
                    }
                    );

                calendar.ListAppointments = await result.ToListAsync();

                return calendar;
            }
        }
    }
}