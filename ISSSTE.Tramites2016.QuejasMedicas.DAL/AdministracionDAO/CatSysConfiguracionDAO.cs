using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.AdministracionDAO
{
    public class CatSysConfiguracionDAO : DoDisposeDTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SettingDetail"></param>
        /// <returns></returns>
        public async Task<int> SaveCatSysConfiguracion(CatalogoResultDTO SettingDetail)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var resultado = await modelo.CatSysConfiguracion
                    .Where(S => S.CatSysConfiguracionId == SettingDetail.CatalogoId)
                    .FirstOrDefaultAsync();

                resultado.Valor = SettingDetail.Valor;
                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Paginado"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public async Task<PagedInformationDTO<CatalogoResultDTO>> ConsultaConceptos(BasePageInformationDTO Paginado,
            bool? EsActivo)
        {
            var pageInfo = new PagedInformationDTO<CatalogoResultDTO>
            {
                SetElementosPorPagina = Paginado.PageSize,
                CurrentPage = Paginado.CurrentPage,
                QueryString = Paginado.QueryString
            };


            using (var modelo = new ISSSTEEntities())
            {
                var requestsQuery = (from a in modelo.CatSysConfiguracion
                    where (!EsActivo.HasValue || a.EsActivo == EsActivo) && a.EsAdministrable
                    select new CatalogoResultDTO
                    {
                        CatalogoId = a.CatSysConfiguracionId,
                        Concepto = a.Concepto,
                        Descripcion = a.Descripcion,
                        Valor = a.Valor,
                        EsAdministrable = a.EsAdministrable,
                        TipoDato = a.TipoDato,
                        EsActivo = a.EsActivo
                    }).AsQueryable();

                if (!string.IsNullOrEmpty(pageInfo.GetFiltroBusqueda))
                    requestsQuery = requestsQuery
                        .Where(R => R.Concepto.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower())
                                    || R.Valor.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower()));

                var requestCount = requestsQuery;
                pageInfo.ResultCount = await requestCount.CountAsync();

                var requests = await requestsQuery
                    .OrderByDescending(R => R.CatalogoId)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.GetCurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();

                pageInfo.ResultList = requests;
            }

            return pageInfo;
        }
    }
}