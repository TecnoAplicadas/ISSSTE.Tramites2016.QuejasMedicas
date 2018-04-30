using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.AdministracionDAO
{
    public class TramiteUnidadAtencionDAO : DoDisposeDTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="AsignacionesTramiteUnidad"></param>
        /// <returns></returns>
        public async Task<int> AsignacionesTramiteUnidad(AsignacionesTramiteUnidadDTO AsignacionesTramiteUnidad)
        {
            using (var modelo = new ISSSTEEntities())
            {
                AsignacionesTramiteUnidad.UnidadesAsignadas.ForEach(S =>
                {
                    var asignacion = modelo.TramiteUnidadAtencion.FirstOrDefault(
                        Tu => Tu.CatTipoTramiteId == AsignacionesTramiteUnidad.CatTipoTramiteId
                              && Tu.UnidadAtencionId == S.UnidadAtencionId);

                    if (asignacion != null)
                    {
                        asignacion.EsActivo = true;
                    }
                    else
                    {
                        var resultado = new TramiteUnidadAtencion
                        {
                            CatTipoTramiteId = AsignacionesTramiteUnidad.CatTipoTramiteId,
                            UnidadAtencionId = S.UnidadAtencionId,
                            EsActivo = true
                        };

                        modelo.TramiteUnidadAtencion.Add(resultado);
                    }
                });

                AsignacionesTramiteUnidad.UnidadesNoAsignadas.ForEach(S =>
                {
                    var desasignar = modelo.TramiteUnidadAtencion
                        .Where(Tu => Tu.CatTipoTramiteId == AsignacionesTramiteUnidad.CatTipoTramiteId
                                     && Tu.UnidadAtencionId == S.UnidadAtencionId)
                        .ToList();

                    desasignar.ForEach(D => { D.EsActivo = false; });
                });
                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DetalleTramiteUnidad"></param>
        /// <returns></returns>
        public async Task<int> AgregarOActualizarTramiteUnidadAtencion(TramiteUnidadAtencionDTO DetalleTramiteUnidad)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var resultado = new TramiteUnidadAtencion
                {
                    TramiteUnidadAtencionId = DetalleTramiteUnidad.TramiteUnidadAtencionId,
                    CatTipoTramiteId = DetalleTramiteUnidad.CatTipoTramiteId,
                    UnidadAtencionId = DetalleTramiteUnidad.UnidadAtencionId,
                    EsActivo = DetalleTramiteUnidad.EsActivo
                };

                modelo.TramiteUnidadAtencion.AddOrUpdate(resultado);

                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public async Task<AsignacionesTramiteUnidadDTO> ConsultaAsignacionesTramiteUnidad(int CatTipoTramiteId,
            bool? EsActivo)
        {
            var asignaciones = new AsignacionesTramiteUnidadDTO();

            using (var modelo = new ISSSTEEntities())
            {
                var unidadesTramite = await modelo.TramiteUnidadAtencion
                    .Where(S => S.EsActivo && S.CatTipoTramiteId == CatTipoTramiteId)
                    .Select(S => new TramiteUnidadAtencionDTO
                    {
                        UnidadAtencionId = S.UnidadAtencionId,
                        UnidadAtencionConcepto = S.UnidadAtencion.Descripcion
                    })
                    .ToListAsync();

                var unidades =
                    await modelo.UnidadAtencion
                        .Where(S => S.EsActivo)
                        .Select(S => new TramiteUnidadAtencionDTO
                        {
                            UnidadAtencionId = S.UnidadAtencionId,
                            UnidadAtencionConcepto = S.Descripcion
                        })
                        .ToListAsync();

                var noasignadas = unidades.Select(S => S.UnidadAtencionId)
                    .Except(unidadesTramite.Select(S => S.UnidadAtencionId))
                    .ToList();

                asignaciones.UnidadesAsignadas = unidadesTramite;
                asignaciones.UnidadesNoAsignadas = unidades.Where(S => noasignadas.Contains(S.UnidadAtencionId))
                    .ToList();
            }

            return asignaciones;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CatalogoReques"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public async Task<PagedInformationDTO<TramiteUnidadAtencionDTO>> ConsultaConceptos(
            CatalogoRequestDTO CatalogoReques, bool? EsActivo)
        {
            var pageInfo = new PagedInformationDTO<TramiteUnidadAtencionDTO>();

            var paginado = CatalogoReques.Paginado;
            pageInfo.SetElementosPorPagina = paginado.PageSize;
            pageInfo.CurrentPage = paginado.CurrentPage;
            pageInfo.QueryString = paginado.QueryString;

            using (var modelo = new ISSSTEEntities())
            {
                var requestsQuery = (from a in modelo.TramiteUnidadAtencion
                    where !EsActivo.HasValue || a.EsActivo == EsActivo
                    select new TramiteUnidadAtencionDTO
                    {
                        TramiteUnidadAtencionId = a.TramiteUnidadAtencionId,
                        UnidadAtencionConcepto = a.UnidadAtencion.Descripcion,
                        TramiteUnidadAtencionConcepto = a.CatTipoTramite.Concepto,
                        EsActivo = a.EsActivo,
                        UnidadAtencionId = a.UnidadAtencionId,
                        CatTipoTramiteId = a.CatTipoTramiteId
                    }).AsQueryable();

                if (!string.IsNullOrEmpty(pageInfo.GetFiltroBusqueda))
                    requestsQuery = requestsQuery
                        .Where(R => R.UnidadAtencionConcepto.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower())
                                    || R.TramiteUnidadAtencionConcepto.ToLower()
                                        .Contains(pageInfo.GetFiltroBusqueda.ToLower()));

                var requestCount = requestsQuery;
                pageInfo.ResultCount = await requestCount.CountAsync();

                var requests = await requestsQuery
                    .OrderByDescending(R => R.UnidadAtencionId)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.GetCurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();

                pageInfo.ResultList = requests;
            }

            return pageInfo;
        }
    }
}