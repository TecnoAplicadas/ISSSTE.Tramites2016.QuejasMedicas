using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.ReportesDAO
{
    public class ColumnasDAO : DoDisposeDTO
    {
        private void FechaRangoDate(ref DateTime? Inicio, ref DateTime? Fin, string ValorInicio, string ValorFin)
        {
            if (string.IsNullOrEmpty(ValorInicio) || string.IsNullOrEmpty(ValorFin))
            {
                if (!string.IsNullOrEmpty(ValorFin))
                    ValorInicio = ValorFin;
                if (!string.IsNullOrEmpty(ValorInicio))
                    Inicio = DateTime.Parse(ValorInicio);
            }
            else
            {
                Inicio = DateTime.Parse(ValorInicio);
                Fin = DateTime.Parse(ValorFin);
            }
        }

        private List<DuplaValoresDTO> ListaConsultaEstados(string CatTipoEdoCita, int CatTipoEdoCitaId, ISSSTEEntities Modelo)
        {
            List<DuplaValoresDTO> lista;
            if (string.IsNullOrEmpty(CatTipoEdoCita) && CatTipoEdoCitaId==0)
            {
                lista = (from a in Modelo.CatTipoEdoCita
                    where a.EsActivo
                    select new DuplaValoresDTO
                    {
                        Id = a.CatTipoEdoCitaId,
                        Valor = a.Semaforo,
                        Informacion = a.Concepto
                    }).ToList();
            }
            else
            {
                if (!string.IsNullOrEmpty(CatTipoEdoCita))
                {
                    var estados = Array.ConvertAll(CatTipoEdoCita.Split(','), int.Parse).ToList();

                    lista = (from a in Modelo.CatTipoEdoCita
                        where a.EsActivo && estados.Contains(a.CatTipoEdoCitaId)
                        select new DuplaValoresDTO
                        {
                            Id = a.CatTipoEdoCitaId,
                            Valor = a.Semaforo,
                            Informacion = a.Concepto
                        }).ToList();
                }
                else
                {
                    lista = (from a in Modelo.CatTipoEdoCita
                        where a.EsActivo && a.CatTipoEdoCitaId==CatTipoEdoCitaId
                        select new DuplaValoresDTO
                        {
                            Id = a.CatTipoEdoCitaId,
                            Valor = a.Semaforo,
                            Informacion = a.Concepto
                        }).ToList();
                }
            }
            return lista;
        }

        private string FiltroContains(string Valor, string Campo)
        {
            return !string.IsNullOrEmpty(Valor) ? " and " + Campo + ".Contains(\"" + Valor + "\") " : "";
        }

        private string ListaConsulta(string Valores, string Campo)
        {
            var consulta = "";

            if (string.IsNullOrEmpty(Valores)) return consulta;
            var enteros = Valores.Split(',').ToList();

            foreach (var item in enteros)
                if (string.Equals(consulta, "", StringComparison.Ordinal)) consulta = Campo + " = " + item;
                else consulta += " Or " + Campo + " = " + item;
            if (!string.Equals(consulta, "", StringComparison.Ordinal)) consulta = " and ( " + consulta + " ) ";
            return consulta;
        }

        private void Swap<T>(ref T X, ref T Y)
        {
            //int a = 4, b = 6;
            //a ^= b ^= a ^= b;
            var t = Y;
            Y = X;
            X = t;
        }

        private string FiltroNullable(bool? Valor, string Campo)
        {
            return Valor == null ? "" : " and " + Campo + "=" + Valor;
        }

        private string FechaRango(string ValorInicio, string ValorFin, string Campo)
        {
            var consulta = "";
            DateTime h1;

            if (string.IsNullOrEmpty(ValorInicio) || string.IsNullOrEmpty(ValorFin))
            {
                if (!string.IsNullOrEmpty(ValorFin))
                    ValorInicio = ValorFin;
                if (!string.IsNullOrEmpty(ValorInicio))
                {
                    h1 = DateTime.Parse(ValorInicio);
                    consulta += " and " + Campo + ">=" + h1.Ticks;
                }
            }
            else
            {
                h1 = DateTime.Parse(ValorInicio);
                var h2 = DateTime.Parse(ValorFin);
                if (h1.Ticks > h2.Ticks)
                    Swap(ref h1, ref h2);
                h2 = h2.AddHours(23);
                h2 = h2.AddMinutes(59);
                consulta += " and " + Campo + ">=" + h1.Ticks + " and " + Campo + "<=" + h2.Ticks;
            }

            return consulta;
        }

        private string HoraRango(string ValorInicio, string ValorFin, string Campo)
        {
            var consulta = "";
            TimeSpan h1;

            if (string.IsNullOrEmpty(ValorInicio) || string.IsNullOrEmpty(ValorFin))
            {
                if (!string.IsNullOrEmpty(ValorFin))
                    ValorInicio = ValorFin;
                if (!string.IsNullOrEmpty(ValorInicio))
                {
                    h1 = TimeSpan.Parse(ValorInicio);
                    consulta += " and " + Campo + ">=" + h1.Ticks;
                }
            }
            else
            {
                h1 = TimeSpan.Parse(ValorInicio);
                var h2 = TimeSpan.Parse(ValorFin);
                if (h1.Ticks > h2.Ticks)
                    Swap(ref h1, ref h2);
                consulta += " and " + Campo + ">=" + h1.Ticks + " and " + Campo + "<=" + h2.Ticks;
            }
            return consulta;
        }

        private string EdadRango(string ValorInicio, string ValorFin, string Campo)
        {
            var consulta = "";
            int h1;

            if (string.IsNullOrEmpty(ValorInicio) || string.IsNullOrEmpty(ValorFin))
            {
                if (!string.IsNullOrEmpty(ValorFin))
                    ValorInicio = ValorFin;
                if (!string.IsNullOrEmpty(ValorInicio))
                {
                    h1 = int.Parse(ValorInicio) - 1;
                    if (h1 > 0)
                        consulta += " and " + Campo + ">" + h1;
                }
            }
            else
            {
                h1 = int.Parse(ValorInicio);
                var h2 = int.Parse(ValorFin);

                if (h1 > h2)
                    Swap(ref h1, ref h2);

                h1 = h1 - 1;
                h2 = h2 + 1;

                consulta += " and " + Campo + ">" + h1 + " and " + Campo + "<" + h2;
            }
            return consulta;
        }

        private string QueryStringReporte2(GeneralCitaDTO Filtros)
        {
            var consulta = " 1=1 ";

            consulta += HoraRango(Filtros.HoraInicioCita, Filtros.HoraFinCita, "HoraCitaTicks");
            consulta += HoraRango(Filtros.HoraInicioRegistro, Filtros.HoraFinRegistro, "HoraSolicitudTicks");

            consulta += FechaRango(Filtros.FechaInicioCita, Filtros.FechaFinCita, "FechaCitaTicks");
            consulta += FechaRango(Filtros.FechaInicioRegistro, Filtros.FechaFinRegistro, "FechaRegistroTicks");

            return consulta;
        }

        private string QueryStringReporte1(GeneralCitaDTO Filtros)
        {
            var consulta = " 1=1 ";

            consulta += FiltroContains(Filtros.NombrePaciente, "NombrePaciente");
            consulta += FiltroContains(Filtros.Apellido1Paciente, "Apellido1Paciente");
            consulta += FiltroContains(Filtros.Apellido2Paciente, "Apellido2Paciente");
            consulta += FiltroContains(Filtros.CURPPaciente, "CURPPaciente");
            consulta += FiltroContains(Filtros.CorreoPaciente, "CorreoPaciente");
            consulta += FiltroContains(Filtros.TelefonoPaciente, "TelefonoPaciente");
            consulta += FiltroContains(Filtros.CelularPaciente, "CelularPaciente");

            consulta += FiltroContains(Filtros.NombrePromovente, "NombrePromovente");
            consulta += FiltroContains(Filtros.Apellido1Promovente, "Apellido1Promovente");
            consulta += FiltroContains(Filtros.Apellido2Promovente, "Apellido2Promovente");
            consulta += FiltroContains(Filtros.CURPPromovente, "CURPPromovente");
            consulta += FiltroContains(Filtros.CorreoPromovente, "CorreoPromovente");
            consulta += FiltroContains(Filtros.TelefonoPromovente, "TelefonoPromovente");
            consulta += FiltroContains(Filtros.CelularPromovente, "CelularPromovente");

            consulta += FiltroNullable(Filtros.EsMasculinoPaciente, "EsMasculinoPaciente");
            consulta += FiltroNullable(Filtros.EsMasculinoPromovente, "EsMasculinoPromovente");
            consulta += FiltroNullable(Filtros.EsUsuarioBloqueadoPaciente, "EsUsuarioBloqueadoPaciente ");

            consulta += ListaConsulta(Filtros.CatTipoEdoCita, "CatTipoEdoCitaId");
            consulta += ListaConsulta(Filtros.CatTipoTramite, "CatTipoTramiteId");
            if (Filtros.UnidadAtencionId != 0)
                consulta += " and UnidadAtencionId=" + Filtros.UnidadAtencionId + " ";

            consulta += EdadRango(Filtros.EdadInicioPaciente, Filtros.EdadFinPaciente, "EdadPaciente");
            consulta += EdadRango(Filtros.EdadInicioPromovente, Filtros.EdadFinPromovente, "EdadPromovente");

            return consulta;
        }

        /// <summary>
        /// Consulta del reporte gráfico
        /// </summary>
        /// <param name="Filtros"></param>
        /// <returns></returns>
        public List<EstadoCitaCortoDTO> ListaReporte2(GeneralCitaDTO Filtros)
        {
            List<int> listaAuxiliar;

            using (var modelo = new ISSSTEEntities())
            {
                var res = (from c in modelo.UnidadAtencion
                    join T in modelo.TramiteUnidadAtencion on c.UnidadAtencionId equals T.UnidadAtencionId
                    where c.EsActivo
                    select new EstadoCitaCortoDTO
                    {
                        UnidadAtencionId = c.UnidadAtencionId,
                        Descripcion = c.Descripcion
                    }).AsQueryable();

                //// Selección de Unidades de atención
                if (Filtros.UnidadAtencionId != 0)
                    res = res.Where(R => R.UnidadAtencionId == Filtros.UnidadAtencionId);
                var resultado = res.ToList().GroupBy(X => X.UnidadAtencionId).Select(Y => Y.FirstOrDefault()).ToList();

                //// Sí se quisiera opción multiple por tipo de trámite
                /*listaAuxiliar = !string.IsNullOrEmpty(Filtros.CatTipoTramite)
                    ? Array.ConvertAll(Filtros.CatTipoTramite.Split(','), int.Parse).ToList()
                    : modelo.CatTipoTramite.Select(S => S.CatTipoTramiteId).ToList();*/

                listaAuxiliar = Filtros.CatTipoTramiteId != 0
                    ? new List<int> () { Filtros.CatTipoTramiteId }
                    : modelo.CatTipoTramite.Select(S => S.CatTipoTramiteId).ToList();

                //// Sí se quisiera opción multiple por tipo de trámite
                //// var estadosCita = ListaConsultaEstados(Filtros.CatTipoEdoCita, modelo);

                var estadosCita = ListaConsultaEstados(null, Filtros.CatTipoEdoCitaId, modelo);

                DateTime? fechaInicioCita = null,
                    fechaFinCita = null,
                    fechaInicioRegistro = null,
                    fechaFinRegistro = null;
                FechaRangoDate(ref fechaInicioCita, ref fechaFinCita, Filtros.FechaInicioCita, Filtros.FechaFinCita);
                FechaRangoDate(ref fechaInicioRegistro, ref fechaFinRegistro, Filtros.FechaInicioRegistro,
                    Filtros.FechaFinRegistro);

                if (fechaFinCita != null)
                {
                    fechaFinCita = ((DateTime) fechaFinCita).AddHours(23);
                    fechaFinCita = ((DateTime)fechaFinCita).AddMinutes(59);
                }

                if (fechaFinRegistro != null)
                {
                    fechaFinRegistro = ((DateTime)fechaFinRegistro).AddHours(23);
                    fechaFinRegistro = ((DateTime)fechaFinRegistro).AddMinutes(59);
                }

                resultado = resultado.Select(F =>
                    {
                        var estados = new List<Tuple<string, int, string>>();
                        foreach (var edo in estadosCita)
                        {
                            var estado = edo.Id;
                            var valor = edo.Informacion;
                            var cantidad = modelo.Solicitud.Where(
                                    A => A.CatTipoEdoCitaId == estado &&                                                
                                         listaAuxiliar.Contains(A.TramiteUnidadAtencion.CatTipoTramiteId) &&
                                         A.TramiteUnidadAtencion.UnidadAtencionId == F.UnidadAtencionId &&
                                         
                                         (!fechaInicioRegistro.HasValue || !fechaFinRegistro.HasValue ||
                                          A.FechaRegistro > fechaInicioRegistro &&
                                          A.FechaRegistro < fechaFinRegistro) &&
                                         (!fechaInicioRegistro.HasValue || fechaFinRegistro.HasValue ||
                                          A.FechaRegistro > fechaInicioRegistro) &&
                                         
                                          (!fechaInicioCita.HasValue || !fechaFinCita.HasValue ||
                                          A.FechaCita > fechaInicioCita && A.FechaCita < fechaFinCita) &&
                                         (!fechaInicioCita.HasValue || fechaFinCita.HasValue ||
                                          A.FechaCita > fechaFinCita))
                                .Select(A => new EstadoCitaDTO
                                {
                                    FechaCita = A.FechaCita,
                                    FechaRegistro = A.FechaRegistro
                                })
                                .Distinct()
                                .ToList()
                                .Count;

                            estados.Add(Tuple.Create(valor, cantidad, edo.Valor));
                        }
                        F.Valores = estados;
                        return F;
                    })
                    .ToList();
                return resultado;
            }
        }

        /// <summary>
        /// Consulta del reporte por filtros
        /// </summary>
        /// <param name="Filtros"></param>
        /// <returns></returns>
        public List<GeneralCitaDTO> ListaReporte1(GeneralCitaDTO Filtros)
        {
            List<GeneralCitaDTO> resultado;

            var consulta = QueryStringReporte1(Filtros);

            using (var modelo = new ISSSTEEntities())
            {

                var resultadoQuery = (from c in modelo.Solicitud
                 join T in modelo.TramiteUnidadAtencion on c.TramiteUnidadAtencionId equals T
                     .TramiteUnidadAtencionId
                 join a in modelo.Involucrado on c.SolicitudId equals a.SolicitudId
                 join b in modelo.Involucrado on c.SolicitudId equals b.SolicitudId
                 where a.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente
                       && b.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Promovente
                 select new GeneralCitaDTO
                 {
                     FechaRegistro = c.FechaRegistro,

                     FechaCita = c.FechaCita,
                     HoraCita = c.Horario.HoraInicio,

                     EstadoCita = c.CatTipoEdoCita.Concepto,
                     EstadoTramite = T.CatTipoTramite.Concepto,
                     ColorCita = c.CatTipoEdoCita.Semaforo,
                     ColorTramite = T.CatTipoTramite.Semaforo,
                     ConceptoUnidad = T.UnidadAtencion.Descripcion,

                     CatTipoTramiteId = T.CatTipoTramiteId,
                     CatTipoEdoCitaId = c.CatTipoEdoCitaId,
                     UnidadAtencionId = T.UnidadAtencionId,

                     NombrePaciente = a.Persona.Nombre,
                     Apellido1Paciente = a.Persona.Paterno,
                     Apellido2Paciente = a.Persona.Materno,
                     CURPPaciente = a.Persona.CURP,
                     EsMasculinoPaciente = a.Persona.EsMasculino,
                     GeneroPaciente = a.Persona.EsMasculino ? "Masculino" : "Femenino",
                     EdadPaciente = a.Persona.Edad,
                     CorreoPaciente = a.Persona.Correo,
                     TelefonoPaciente = (!string.IsNullOrEmpty(a.Persona.Lada) ? a.Persona.Lada + "-" : "") +
                                        a.Persona.Telefono,
                     CelularPaciente = a.Persona.TelefonoMovil,
                     EsUsuarioBloqueadoPaciente = a.Persona.EsUsuarioBloqueado,

                     NombrePromovente = b.PersonaId == a.PersonaId ? "" : b.Persona.Nombre,
                     Apellido1Promovente = b.PersonaId == a.PersonaId ? "" : b.Persona.Paterno,
                     Apellido2Promovente = b.PersonaId == a.PersonaId ? "" : b.Persona.Materno,
                     CURPPromovente = b.PersonaId == a.PersonaId ? "" : b.Persona.CURP,
                     EsMasculinoPromovente = b.PersonaId == a.PersonaId ? (bool?)null : b.Persona.EsMasculino,
                     GeneroPromovente = b.PersonaId == a.PersonaId
                         ? ""
                         : (b.Persona.EsMasculino ? "Masculino" : "Femenino"),
                     EdadPromovente = b.PersonaId == a.PersonaId ? 0 : b.Persona.Edad,
                     CorreoPromovente = b.PersonaId == a.PersonaId ? "" : b.Persona.Correo,
                     TelefonoPromovente = b.PersonaId == a.PersonaId
                         ? ""
                         : (!string.IsNullOrEmpty(b.Persona.Lada) ? b.Persona.Lada + "-" : "")
                           + b.Persona.Telefono,
                     CelularPromovente = b.PersonaId == a.PersonaId ? "" : b.Persona.TelefonoMovil
                 }).Where(consulta).AsQueryable();

                resultado = (from c in modelo.Solicitud
                        join T in modelo.TramiteUnidadAtencion on c.TramiteUnidadAtencionId equals T
                            .TramiteUnidadAtencionId
                        join a in modelo.Involucrado on c.SolicitudId equals a.SolicitudId
                        join b in modelo.Involucrado on c.SolicitudId equals b.SolicitudId
                        where a.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Paciente
                              && b.CatTipoInvolucradoId == Enumeracion.EnumTipoInvolucrado.Promovente
                        select new GeneralCitaDTO
                        {
                            FechaRegistro = c.FechaRegistro,

                            FechaCita = c.FechaCita,
                            HoraCita = c.Horario.HoraInicio,
                            
                            EstadoCita = c.CatTipoEdoCita.Concepto,
                            EstadoTramite = T.CatTipoTramite.Concepto,
                            ColorCita = c.CatTipoEdoCita.Semaforo,
                            ColorTramite = T.CatTipoTramite.Semaforo,
                            ConceptoUnidad = T.UnidadAtencion.Descripcion,

                            CatTipoTramiteId = T.CatTipoTramiteId,
                            CatTipoEdoCitaId = c.CatTipoEdoCitaId,
                            UnidadAtencionId = T.UnidadAtencionId,

                            NombrePaciente = a.Persona.Nombre,
                            Apellido1Paciente = a.Persona.Paterno,
                            Apellido2Paciente = a.Persona.Materno,
                            CURPPaciente = a.Persona.CURP,
                            EsMasculinoPaciente = a.Persona.EsMasculino,
                            GeneroPaciente = a.Persona.EsMasculino ? "Masculino" : "Femenino",
                            EdadPaciente = a.Persona.Edad,
                            CorreoPaciente = a.Persona.Correo,
                            TelefonoPaciente = (!string.IsNullOrEmpty(a.Persona.Lada) ? a.Persona.Lada + "-" : "") +
                                               a.Persona.Telefono,
                            CelularPaciente = a.Persona.TelefonoMovil,
                            EsUsuarioBloqueadoPaciente = a.Persona.EsUsuarioBloqueado,

                            NombrePromovente = b.PersonaId == a.PersonaId ? "" : b.Persona.Nombre,
                            Apellido1Promovente = b.PersonaId == a.PersonaId ? "" : b.Persona.Paterno,
                            Apellido2Promovente = b.PersonaId == a.PersonaId ? "" : b.Persona.Materno,
                            CURPPromovente = b.PersonaId == a.PersonaId ? "" : b.Persona.CURP,
                            EsMasculinoPromovente = b.PersonaId == a.PersonaId ? (bool?) null : b.Persona.EsMasculino,
                            GeneroPromovente = b.PersonaId == a.PersonaId
                                ? ""
                                : (b.Persona.EsMasculino ? "Masculino" : "Femenino"),
                            EdadPromovente = b.PersonaId == a.PersonaId ? 0 : b.Persona.Edad,
                            CorreoPromovente = b.PersonaId == a.PersonaId ? "" : b.Persona.Correo,
                            TelefonoPromovente = b.PersonaId == a.PersonaId
                                ? ""
                                : (!string.IsNullOrEmpty(b.Persona.Lada) ? b.Persona.Lada + "-" : "")
                                  + b.Persona.Telefono,
                            CelularPromovente = b.PersonaId == a.PersonaId ? "" : b.Persona.TelefonoMovil
                        }).Where(consulta)
                    .Distinct()
                    .ToList();

                consulta = QueryStringReporte2(Filtros);

                resultado = resultado.Select(F =>
                    {
                        F.HoraSolicitud = TimeSpan.Parse("" + F.FechaRegistro.Hour + ":" + F.FechaRegistro.Minute);
                        F.HoraCitaTicks = F.HoraCita.Ticks;
                        F.HoraSolicitudTicks = F.HoraSolicitud.Ticks;
                        F.FechaCitaTicks = F.FechaCita.Ticks;
                        F.FechaRegistroTicks = F.FechaRegistro.Ticks;
                        F.FechaRegistroCad = ("" + F.FechaRegistro).Split(' ')[0];
                        F.FechaCitaCad = ("" + F.FechaCita).Split(' ')[0];
                        return F;
                    })
                    .AsQueryable()
                    .Where(consulta)
                    .Distinct()
                    .ToList();

                Filtros.TotalRegistros = resultado.Count;
                Enumeracion.TotalRegistros = Filtros.TotalRegistros;

                if (Filtros.ConPaginadorController)
                    resultado = resultado.Skip((Filtros.PaginaSolicitada - 1) * Filtros.RegistrosPorPagina)
                        .Take(Filtros.RegistrosPorPagina)
                        .Distinct()
                        .ToList();
            }

            return resultado;
        }
    }
}