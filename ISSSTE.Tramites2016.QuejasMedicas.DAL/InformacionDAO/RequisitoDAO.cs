using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO
{
    public class RequisitoDAO : DoDisposeDTO
    {
        /// <summary>
        ///     Consulta de requisitos paginando el resultado
        /// </summary>
        /// <param name="Paginado"></param>
        /// <param name="EsActivo"></param>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        public async Task<PagedInformationDTO<RequisitoDTO>> ConsultaRequisitosPaginado(BasePageInformationDTO Paginado,
            bool? EsActivo, int? CatTipoTramiteId)
        {
            var pageInfo = new PagedInformationDTO<RequisitoDTO>
            {
                SetElementosPorPagina = Paginado.PageSize,
                CurrentPage = Paginado.CurrentPage,
                QueryString = Paginado.QueryString
            };


            using (var modelo = new ISSSTEEntities())
            {
                var requestsQuery = (from a in modelo.Requisito
                    where !EsActivo.HasValue || a.EsActivo == EsActivo
                    select new RequisitoDTO
                    {
                        RequisitoId = a.RequisitoId,
                        CatTipoTramiteId = a.CatTipoTramiteId,
                        NombreDocumento = a.NombreDocumento,
                        EsObligatorio = a.EsObligatorio,
                        EsActivo = a.EsActivo,
                        Descripcion = a.Descripcion
                    }).AsQueryable();

                if (!string.IsNullOrEmpty(pageInfo.GetFiltroBusqueda))
                    requestsQuery = requestsQuery
                        .Where(R => R.Descripcion.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower())
                                    || R.NombreDocumento.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower()));

                if (CatTipoTramiteId.HasValue)
                    requestsQuery = requestsQuery.Where(R => R.CatTipoTramiteId == CatTipoTramiteId);

                var requestCount = requestsQuery;
                pageInfo.ResultCount = await requestCount.CountAsync();

                var requests = await requestsQuery
                    .OrderByDescending(R => R.RequisitoId)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.GetCurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();

                pageInfo.ResultList = requests;
            }
            return pageInfo;
        }

        /// <summary>
        ///     Actualización / Inserción de un requisito
        /// </summary>
        /// <param name="DetalleRequisito"></param>
        /// <returns></returns>
        public async Task<int> SalvarActualizarRequisito(RequisitoDTO DetalleRequisito)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var resultado = new Requisito
                {
                    RequisitoId = DetalleRequisito.RequisitoId,
                    CatTipoTramiteId = DetalleRequisito.CatTipoTramiteId,
                    Descripcion = DetalleRequisito.Descripcion,
                    NombreDocumento = DetalleRequisito.NombreDocumento,
                    EsActivo = DetalleRequisito.EsActivo,
                    EsObligatorio = DetalleRequisito.EsObligatorio
                };

                modelo.Requisito.AddOrUpdate(resultado);
                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        ///     Lista de consulta requisitos
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        public List<RequisitoDTO> ConsultaRequisitos(int CatTipoTramiteId)
        {
            List<RequisitoDTO> requisitos;
            using (var modelo = new ISSSTEEntities())
            {
                requisitos = (from a in modelo.Requisito
                    where a.EsActivo && a.CatTipoTramiteId == CatTipoTramiteId
                    select new RequisitoDTO
                    {
                        RequisitoId = a.RequisitoId,
                        CatTipoTramiteId = a.CatTipoTramiteId,
                        NombreDocumento = a.NombreDocumento,
                        Descripcion = a.Descripcion,
                        EsObligatorio = a.EsObligatorio,
                        EsActivo = a.EsActivo
                    }).ToList();
            }
            return requisitos;
        }
    }
}