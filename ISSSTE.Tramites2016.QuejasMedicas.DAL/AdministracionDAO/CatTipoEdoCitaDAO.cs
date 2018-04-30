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
    public class CatTipoEdoCitaDAO : DoDisposeDTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DetalleEstadoCita"></param>
        /// <returns></returns>
        /// <exception cref="QuejasMedicasException"></exception>
        public async Task<int> AgregarOActualizarCatTipoEdoCita(CatTipoEdoCitaDTO DetalleEstadoCita)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var existeConcepto =
                    modelo.CatTipoEdoCita.FirstOrDefault(
                        S => S.Concepto.Equals(DetalleEstadoCita.Concepto) &&
                             S.CatTipoEdoCitaId != DetalleEstadoCita.CatTipoEdoCitaId);

                if (existeConcepto != null)
                    throw new QuejasMedicasException(string.Format(EnumConstErrores.ErrorConceptoYaExiste,
                        "Nombre del Estado de la cita"));

                var resultado = new CatTipoEdoCita
                {
                    CatTipoEdoCitaId = DetalleEstadoCita.CatTipoEdoCitaId,
                    Concepto = DetalleEstadoCita.Concepto,
                    Semaforo = DetalleEstadoCita.Semaforo,
                    EsActivo = DetalleEstadoCita.EsActivo
                };

                modelo.CatTipoEdoCita.AddOrUpdate(resultado);

                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CatalogoReques"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public async Task<PagedInformationDTO<CatTipoEdoCitaDTO>> ConsultaConceptos(CatalogoRequestDTO CatalogoReques,
            bool? EsActivo)
        {
            var pageInfo = new PagedInformationDTO<CatTipoEdoCitaDTO>();

            var paginado = CatalogoReques.Paginado;
            pageInfo.SetElementosPorPagina = paginado.PageSize;
            pageInfo.CurrentPage = paginado.CurrentPage;
            pageInfo.QueryString = paginado.QueryString;

            using (var modelo = new ISSSTEEntities())
            {
                var requestsQuery = (from a in modelo.CatTipoEdoCita
                    where !EsActivo.HasValue || a.EsActivo == EsActivo
                    select new CatTipoEdoCitaDTO
                    {
                        CatTipoEdoCitaId = a.CatTipoEdoCitaId,
                        Concepto = a.Concepto,
                        EsActivo = a.EsActivo,
                        Semaforo = a.Semaforo
                    }).AsQueryable();

                if (!string.IsNullOrEmpty(pageInfo.GetFiltroBusqueda))
                    requestsQuery = requestsQuery
                        .Where(R => R.Concepto.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower()));

                var requestCount = requestsQuery;
                pageInfo.ResultCount = await requestCount.CountAsync();

                var requests = await requestsQuery
                    .OrderByDescending(R => R.CatTipoEdoCitaId)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.GetCurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .OrderBy(S => S.CatTipoEdoCitaId)
                    .ToListAsync();

                pageInfo.ResultList = requests;
            }

            return pageInfo;
        }
    }
}