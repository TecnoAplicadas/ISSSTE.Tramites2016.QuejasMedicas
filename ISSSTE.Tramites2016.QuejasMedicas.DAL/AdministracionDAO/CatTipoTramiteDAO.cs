using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using static ISSSTE.Tramites2016.QuejasMedicas.Model.Enums.Enumeracion;
using ISSSTE.Tramites2016.QuejasMedicas.Model;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.AdministracionDAO
{
    public class CatTipoTramiteDAO : DoDisposeDTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DetalleTramite"></param>
        /// <returns></returns>
        /// <exception cref="QuejasMedicasException"></exception>
        public async Task<int> AgregarOActualizarCatTipoTramite(CatTipoTramiteDTO DetalleTramite)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var existeConcepto =
                    modelo.CatTipoTramite.FirstOrDefault(
                        S => S.Concepto.Equals(DetalleTramite.Concepto) &&
                             S.CatTipoTramiteId != DetalleTramite.CatTipoTramiteId);

                if (existeConcepto != null)
                    throw new QuejasMedicasException(
                        string.Format(EnumConstErrores.ErrorConceptoYaExiste, "Nombre del trámite"));

                var resultado = new CatTipoTramite
                {
                    CatTipoTramiteId = DetalleTramite.CatTipoTramiteId,
                    Concepto = DetalleTramite.Concepto,
                    Semaforo = DetalleTramite.Semaforo,
                    Homoclave = DetalleTramite.HomoClave,
                    EsActivo = DetalleTramite.EsActivo
                };

                modelo.CatTipoTramite.AddOrUpdate(resultado);

                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CatalogoReques"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public async Task<PagedInformationDTO<CatTipoTramiteDTO>> ConsultaConceptos(CatalogoRequestDTO CatalogoReques,
            bool? EsActivo)
        {
            var pageInfo = new PagedInformationDTO<CatTipoTramiteDTO>();

            var paginado = CatalogoReques.Paginado;
            pageInfo.SetElementosPorPagina = paginado.PageSize;
            pageInfo.CurrentPage = paginado.CurrentPage;
            pageInfo.QueryString = paginado.QueryString;

            using (var modelo = new ISSSTEEntities())
            {
                var requestsQuery = (from a in modelo.CatTipoTramite
                    where !EsActivo.HasValue || a.EsActivo == EsActivo
                    select new CatTipoTramiteDTO
                    {
                        CatTipoTramiteId = a.CatTipoTramiteId,
                        Concepto = a.Concepto,
                        EsActivo = a.EsActivo,
                        Semaforo = a.Semaforo,
                        HomoClave = a.Homoclave
                    }).AsQueryable();

                if (!string.IsNullOrEmpty(pageInfo.GetFiltroBusqueda))
                    requestsQuery = requestsQuery
                        .Where(R => R.Concepto.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower()));

                var requestCount = requestsQuery;
                pageInfo.ResultCount = await requestCount.CountAsync();

                var requests = await requestsQuery
                    .OrderByDescending(R => R.CatTipoTramiteId)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.GetCurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();

                pageInfo.ResultList = requests;

                return pageInfo;
            }
        }
    }
}