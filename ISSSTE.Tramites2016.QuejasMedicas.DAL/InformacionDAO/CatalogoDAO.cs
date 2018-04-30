using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO
{
    public class CatalogoDAO : DoDisposeDTO
    {
        /// <summary>
        ///     Catálogo de entidades
        /// </summary>
        /// <returns></returns>
        public List<DuplaValoresDTO> GetUadyCs(int CatTipoTramiteId)
        {
            List<DuplaValoresDTO> duplaValoresDto;
            using (var modelo = new ISSSTEEntities())
            {
                duplaValoresDto = CatTipoTramiteId != 0
                    ? (from a in modelo.UnidadAtencion
                       join b in modelo.TramiteUnidadAtencion on a.UnidadAtencionId equals b.UnidadAtencionId
                       where a.EsActivo && b.EsActivo && b.CatTipoTramiteId == CatTipoTramiteId
                       select new DuplaValoresDTO
                       {
                           Id = b.TramiteUnidadAtencionId,
                           Valor = a.Descripcion
                       }).OrderBy(O => O.Valor)
                    .ToList()
                    : (from a in modelo.UnidadAtencion
                       join b in modelo.TramiteUnidadAtencion on a.UnidadAtencionId equals b.UnidadAtencionId
                       where a.EsActivo
                       select new DuplaValoresDTO
                       {
                           Id = a.UnidadAtencionId,
                           Valor = a.Descripcion
                       }).OrderBy(O => O.Valor)
                    .ToList();

                duplaValoresDto = duplaValoresDto.GroupBy(X => X.Id).Select(Y => Y.FirstOrDefault()).ToList();
            }
            return duplaValoresDto;
        }


        /// <summary>
        ///     Catálogo de entidades
        /// </summary>
        /// <returns></returns>
        public List<DuplaValoresDTO> GetUserUADyCS(int[] UADyCSIds)
        {
            List<DuplaValoresDTO> duplaValoresDto = new List<DuplaValoresDTO>();

            using (var modelo = new ISSSTEEntities())
            {
                var delegationResult = modelo.UnidadAtencion.AsQueryable();

                if (!UADyCSIds.Contains(-1))
                    delegationResult = delegationResult.Where(D => UADyCSIds.Contains(D.UnidadAtencionId));

                duplaValoresDto = delegationResult
                        .Select(D => new DuplaValoresDTO { Id = D.UnidadAtencionId, Valor = D.Descripcion })
                        .ToList();

                duplaValoresDto = duplaValoresDto.GroupBy(X => X.Id).Select(Y => Y.FirstOrDefault()).ToList();
            }

            return duplaValoresDto;
        }

        /// <summary>
        ///     Catálogo de Unidades de Atención por tipo de trámite
        /// </summary>
        /// <param name="CalendarioDto"></param>
        /// <returns></returns>
        public List<DuplaValoresDTO> GetTramiteUnidadMedica(CalendarioDTO CalendarioDto)
        {
            List<DuplaValoresDTO> duplaValoresDto;
            using (var modelo = new ISSSTEEntities())
            {

                var QueryBase = modelo.UnidadMedica
                                .Join(modelo.TramiteUnidadAtencion,
                                    um => um.UnidadAtencionId, tua => tua.UnidadAtencionId,
                                    (um, tua) => new { um, tua })
                                    .Where(s => s.tua.CatTipoTramiteId == CalendarioDto.CatTipoTramiteId)
                                    .AsQueryable();


                if (!string.IsNullOrEmpty(CalendarioDto.PalabraBusqueda))
                {
                    CalendarioDto.PalabraBusqueda = CalendarioDto.PalabraBusqueda.ToUpper();
                    QueryBase = QueryBase
                        .Where(S => (S.um.Descripcion).ToUpper().Contains(CalendarioDto.PalabraBusqueda));
                }

                if (CalendarioDto.TramiteUnidadAtencionId != 0)
                {
                    QueryBase = QueryBase.Where(s =>
                                                    s.tua.TramiteUnidadAtencionId ==
                                                    CalendarioDto.TramiteUnidadAtencionId);

                }

                duplaValoresDto = QueryBase.Select(s => new DuplaValoresDTO
                                {
                                    Id = s.um.UnidadMedicaId,
                                    IdAux = s.tua.TramiteUnidadAtencionId,
                                    Valor = s.um.Descripcion
                                }).ToList();

            }
            return duplaValoresDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TramiteUnidadAtencionId"></param>
        /// <returns></returns>
        public string GetBusquedaHorarioUadyCs(int TramiteUnidadAtencionId)
        {
            var fecha = "";
            var contador = 0;
            using (var modelo = new ISSSTEEntities())
            {
                var fechaComienzo = DateTime.Now;
                var Dias = Enumeracion.EnumVarios.DiasSeleccionables;
                while (contador <= Dias)
                {
                    var diaSemana = (int)fechaComienzo.DayOfWeek;

                    if (diaSemana != Enumeracion.EnumVarios.Sabado && diaSemana != Enumeracion.EnumVarios.Domingo)
                    {
                        var diaNoLaborable = (from a in modelo.DiaNoLaborable
                                              where a.TramiteUnidadAtencionId == TramiteUnidadAtencionId
                                                    && a.Fecha.Day == fechaComienzo.Day
                                                    && a.Fecha.Month == fechaComienzo.Month
                                                    && a.Fecha.Year == fechaComienzo.Year
                                              select a).Count();
                        if (diaNoLaborable == 0)
                            contador++;
                    }
                    if (contador == Dias)
                        fecha = "" + fechaComienzo.Year + "," + fechaComienzo.Month + "," + fechaComienzo.Day;
                    fechaComienzo = fechaComienzo.AddDays(1);
                }
            }
            return fecha;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CalendarioDto"></param>
        /// <returns></returns>
        public List<DuplaValoresDTO> GetTramiteUnidadMedicaEspecial(CalendarioDTO CalendarioDto)
        {
            List<DuplaValoresDTO> duplaValoresDto;
            using (var modelo = new ISSSTEEntities())
            {
                duplaValoresDto =
                    (from a in modelo.UnidadMedica
                     join b in modelo.UnidadAtencion
                     on a.UnidadAtencionId equals b.UnidadAtencionId
                     where a.EsActivo
                           && a.UnidadAtencionId == CalendarioDto.UnidadAtencionId
                     select new DuplaValoresDTO
                     {
                         Id = a.UnidadMedicaId,
                         Valor = a.Descripcion
                     }).Distinct()
                    .ToList();
            }
            return duplaValoresDto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DuplaValoresDTO> GetBusquedaEstadoCita()
        {
            List<DuplaValoresDTO> estadosCita;
            using (var modelo = new ISSSTEEntities())
            {
                estadosCita = (from a in modelo.CatTipoEdoCita
                               where a.EsActivo
                               select new DuplaValoresDTO
                               {
                                   Id = a.CatTipoEdoCitaId,
                                   Valor = a.Concepto
                               }).ToList();
            }
            return estadosCita;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<DuplaValoresDTO> GetBusquedaTipoTramite()
        {
            List<DuplaValoresDTO> estadosTramite;
            using (var modelo = new ISSSTEEntities())
            {
                estadosTramite = (from a in modelo.CatTipoTramite
                                  where a.EsActivo
                                  select new DuplaValoresDTO
                                  {
                                      Id = a.CatTipoTramiteId,
                                      Valor = a.Concepto
                                  }).ToList();
            }
            return estadosTramite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        public string GetHomoClave(int CatTipoTramiteId)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var homoclave = (from c in modelo.CatTipoTramite
                                 where c.CatTipoTramiteId == CatTipoTramiteId
                                 select c.Homoclave
                ).FirstOrDefault();

                return homoclave;
            }
        }

        /// <summary>
        ///     Horarios por mes y unidad UADyCS
        /// </summary>
        /// <param name="CalendarioDto"></param>
        /// <returns></returns>
        public List<HorarioDTO> GetBusquedaHorario(CalendarioDTO CalendarioDto)
        {
            var dias = new List<HorarioDTO>();
            using (var modelo = new ISSSTEEntities())
            {
                var hoy = DateTime.Now;

                var diasNoLaborables = (from a in modelo.TramiteUnidadAtencion
                                        join b in modelo.DiaNoLaborable on a.TramiteUnidadAtencionId equals b.TramiteUnidadAtencionId
                                        where a.TramiteUnidadAtencionId == CalendarioDto.TramiteUnidadAtencionId && b.EsActivo
                                        select b.Fecha).ToList();

                try
                {
                    for (var i = 1; i < 32; i++)
                    {
                        var valores = CalendarioDto.IdFechaLimite.Split(',');
                        var fechaLimite = new DateTime(int.Parse(valores[0]), int.Parse(valores[1]),
                            int.Parse(valores[2]));

                        var fecha = (i < 10 ? "0" + i : "" + i) + "/" + CalendarioDto.MesAnio;
                        var fechaDate = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        var esHoy = fechaDate.Year == DateTime.Now.Year &&
                                    fechaDate.Month == DateTime.Now.Month &&
                                    fechaDate.Day == DateTime.Now.Day;
                        var horarioDto = new HorarioDTO
                        {
                            Fecha = fechaDate,
                            EsFechaAcontecida = fechaDate < hoy && !esHoy,
                            EsDiaNoLaboral = diasNoLaborables.Contains(fechaDate),
                            EsHoy = esHoy
                        };
                        if (horarioDto.Fecha > fechaLimite)
                            horarioDto.EsDiaNoLaboral = true;
                        dias.Add(horarioDto);
                    }
                }
                catch
                {
                    // ignored
                }

                dias = dias.Select(F =>
                    {
                        if (!F.EsFechaAcontecida && !F.EsDiaNoLaboral)
                        {
                            var citasAgendadas = (from b in modelo.Solicitud
                                                  where b.EsActivo && b.FechaCita.Day == F.Fecha.Day &&
                                                        b.FechaCita.Month == F.Fecha.Month &&
                                                        b.FechaCita.Year == F.Fecha.Year &&
                                                        b.TramiteUnidadAtencionId == CalendarioDto.TramiteUnidadAtencionId
                                                  select b).Count();

                            try
                            {
                                F.Capacidad = (from b in modelo.Horario
                                               where b.EsActivo && b.EsDiaEspecial && b.DiaEspecial.Fecha.Day == F.Fecha.Day &&
                                                     b.DiaEspecial.Fecha.Month == F.Fecha.Month &&
                                                     b.DiaEspecial.Fecha.Year == F.Fecha.Year &&
                                                     b.TramiteUnidadAtencionId == CalendarioDto.TramiteUnidadAtencionId
                                               select b.Capacidad).Sum();
                            }
                            catch
                            {
                                // ignored
                            }

                            if (F.Capacidad == 0)
                            {
                                var dia = (int)F.Fecha.DayOfWeek;
                                try
                                {
                                    F.Capacidad = (from b in modelo.Horario
                                                   where b.EsActivo &&
                                                         b.CatTipoDiaSemanaId == dia &&
                                                         b.TramiteUnidadAtencionId == CalendarioDto.TramiteUnidadAtencionId
                                                   select b.Capacidad).Sum();
                                }
                                catch
                                {
                                    // ignored
                                }
                            }
                            F.Capacidad -= citasAgendadas;
							F.EsDiaDisponible = F.Capacidad > 0;
						}
						return F;
                    })
                    .ToList();
            }
            return dias;
        }

        /// <summary>
        ///     Búsqueda de horarios por día
        /// </summary>
        /// <param name="Concepto"></param>
        /// <param name="TramiteUnidadAtencionId"></param>
        /// <returns></returns>
        public List<HorarioDTO> GetBusquedaDia(string Concepto, int TramiteUnidadAtencionId)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var fecha = DateTime.Parse(Concepto);
                var hoy = DateTime.Now;

                var horariosDisponibles = (from b in modelo.Horario
                                           where b.EsActivo && b.EsDiaEspecial &&
                                                 b.DiaEspecial.Fecha.Day == fecha.Day &&
                                                 b.DiaEspecial.Fecha.Month == fecha.Month &&
                                                 b.DiaEspecial.Fecha.Year == fecha.Year &&
                                                 b.TramiteUnidadAtencionId == TramiteUnidadAtencionId && b.EsActivo
                                           select new HorarioDTO
                                           {
                                               HoraInicio = b.HoraInicio,
                                               HorarioId = b.HorarioId,
                                               Capacidad = b.Capacidad
                                           }).ToList();

                if (horariosDisponibles.Count == 0)
                {
                    var dia = (int)fecha.DayOfWeek;
                    horariosDisponibles = (from b in modelo.Horario
                                           where b.EsActivo && !b.EsDiaEspecial &&
                                                 b.CatTipoDiaSemanaId == dia &&
                                                 b.TramiteUnidadAtencionId == TramiteUnidadAtencionId
                                           select new HorarioDTO
                                           {
                                               HoraInicio = b.HoraInicio,
                                               HorarioId = b.HorarioId,
                                               Capacidad = b.Capacidad
                                           }).OrderBy(B => B.HoraInicio)
                        .ToList();
                }

                horariosDisponibles = horariosDisponibles.Select(F =>
                    {
                        F.Capacidad -= (from a in modelo.Solicitud
                                        where a.TramiteUnidadAtencionId == TramiteUnidadAtencionId &&
                                              a.FechaCita.Day == fecha.Day && a.FechaCita.Month == fecha.Month &&
                                              a.FechaCita.Year == fecha.Year
                                              && a.HorarioId == F.HorarioId && a.EsActivo
                                        select a).Count();

                        if (hoy.Day == fecha.Day && hoy.Month == fecha.Month && hoy.Year == fecha.Year &&
                            F.HoraInicio.Hours <= hoy.Hour)
                            F.Capacidad = 0;

                        return F;
                    })
                    .ToList();

                horariosDisponibles = horariosDisponibles.Where(F => F.Capacidad > 0).ToList();

                return horariosDisponibles;
            }
        }

        /// <summary>
        ///     Búsqueda predictiva sobre las UADyCS y unidades médicas
        ///     Tiene prioridad las unidades médicas sobre las UADyCS
        /// </summary>
        /// <param name="Concepto"></param>
        /// <param name="IdCatTipoTramite"></param>
        /// <returns></returns>
        public CalendarioDTO GetBusqueda(string Concepto, int IdCatTipoTramite)
        {
            var calendarioDto = new CalendarioDTO();
            Concepto = Concepto.ToUpper();
            using (var modelo = new ISSSTEEntities())
            {
                var primerOcurrencia = (from a in modelo.UnidadMedica
                                        join b in modelo.TramiteUnidadAtencion
                                        on a.UnidadAtencionId equals b.UnidadAtencionId
                                        where a.Descripcion.ToUpper().Contains(Concepto)
                                              && b.CatTipoTramiteId == IdCatTipoTramite
                                        select new DuplaValoresDTO
                                        {
                                            Id = b.TramiteUnidadAtencionId,
                                            IdAux = a.UnidadMedicaId
                                        }).FirstOrDefault() ?? (from a in modelo.UnidadAtencion
                                                                join b in modelo.TramiteUnidadAtencion
                                                                on a.UnidadAtencionId equals b.UnidadAtencionId
                                                                where a.Descripcion.ToUpper().Contains(Concepto)
                                                                      && b.CatTipoTramiteId == IdCatTipoTramite
                                                                select new DuplaValoresDTO
                                                                {
                                                                    Id = b.TramiteUnidadAtencionId
                                                                }).FirstOrDefault();

                if (primerOcurrencia != null)
                {
                    calendarioDto.TramiteUnidadAtencionId = primerOcurrencia.Id;
                    calendarioDto.UnidadMedicaId = primerOcurrencia.IdAux;
                }
            }
            return calendarioDto;
        }
    }
}