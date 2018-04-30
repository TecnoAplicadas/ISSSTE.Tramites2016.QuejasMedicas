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
    public class UnidadMedicaDAO : DoDisposeDTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnidadMedicaDto"></param>
        /// <returns></returns>
        /// <exception cref="QuejasMedicasException"></exception>
        public async Task<int> AgregarOActualizarUnidadMedica(UnidadMedicaDTO UnidadMedicaDto)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var existeConcepto =
                    modelo.UnidadMedica.FirstOrDefault(S => S.Descripcion.Equals(UnidadMedicaDto.Descripcion) &&
                                                            S.UnidadMedicaId != UnidadMedicaDto.UnidadMedicaId);

                if (existeConcepto != null)
                    throw new QuejasMedicasException(string.Format(EnumConstErrores.ErrorConceptoYaExiste,
                        "nombre de la unidad médica"));

                var resultado = new UnidadMedica
                {
                    UnidadMedicaId = UnidadMedicaDto.UnidadMedicaId,
                    Descripcion = UnidadMedicaDto.Descripcion,
                    UnidadAtencionId = UnidadMedicaDto.UnidadAtencionId,
                    EsActivo = UnidadMedicaDto.EsActivo
                };

                modelo.UnidadMedica.AddOrUpdate(resultado);

                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CatalogoReques"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public async Task<PagedInformationDTO<UnidadMedicaDTO>> ConsultaConceptos(CatalogoRequestDTO CatalogoReques,
            bool? EsActivo)
        {
            var pageInfo = new PagedInformationDTO<UnidadMedicaDTO>();

            var paginado = CatalogoReques.Paginado;
            pageInfo.SetElementosPorPagina = paginado.PageSize;
            pageInfo.CurrentPage = paginado.CurrentPage;
            pageInfo.QueryString = paginado.QueryString;

            using (var modelo = new ISSSTEEntities())
            {
                var requestsQuery = (from a in modelo.UnidadMedica
                    where !EsActivo.HasValue || a.EsActivo == EsActivo
                    select new UnidadMedicaDTO
                    {
                        UnidadMedicaId = a.UnidadMedicaId,
                        UnidadAtencionConcepto = a.UnidadAtencion.Descripcion,
                        UnidadAtencionId = a.UnidadAtencionId,
                        Descripcion = a.Descripcion,
                        EsActivo = a.EsActivo
                    }).AsQueryable();

                if (!string.IsNullOrEmpty(pageInfo.GetFiltroBusqueda))
                    requestsQuery = requestsQuery
                        .Where(R => R.Descripcion.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower())
                                    || R.UnidadAtencionConcepto.ToLower()
                                        .Contains(pageInfo.GetFiltroBusqueda.ToLower()));

                var requestCount = requestsQuery;
                pageInfo.ResultCount = await requestCount.CountAsync();

                var requests = await requestsQuery
                    .OrderByDescending(R => R.UnidadMedicaId)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.GetCurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();

                pageInfo.ResultList = requests;

                return pageInfo;
            }
        }
    }
}