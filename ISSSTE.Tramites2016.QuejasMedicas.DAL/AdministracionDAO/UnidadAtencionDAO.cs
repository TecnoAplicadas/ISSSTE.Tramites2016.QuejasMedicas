using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using static ISSSTE.Tramites2016.QuejasMedicas.Model.Enums.Enumeracion;
using ISSSTE.Tramites2016.QuejasMedicas.Model;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.AdministracionDAO
{
    public class UnidadAtencionDAO : DoDisposeDTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DetalleUnidadAtencion"></param>
        /// <returns></returns>
        /// <exception cref="QuejasMedicasException"></exception>
        public async Task<int> AgregarOActualizar(UnidadAtencionDTO DetalleUnidadAtencion)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var existeConcepto =
                    modelo.UnidadAtencion.FirstOrDefault(
                        S => S.Descripcion.Equals(DetalleUnidadAtencion.Descripcion) &&
                             S.UnidadAtencionId != DetalleUnidadAtencion.UnidadAtencionId);

                if (existeConcepto != null)
                    throw new QuejasMedicasException(string.Format(EnumConstErrores.ErrorConceptoYaExiste,
                        "Nombre de la Unidad de Atencón"));

                var resultado = new UnidadAtencion
                {
                    UnidadAtencionId = DetalleUnidadAtencion.UnidadAtencionId,
                    Descripcion = DetalleUnidadAtencion.Descripcion,
                    Prefijo = DetalleUnidadAtencion.Prefijo,
                    Mascara = DetalleUnidadAtencion.Mascara,
                    EsActivo = DetalleUnidadAtencion.EsActivo,
                    CatTipoEntidadId = DetalleUnidadAtencion.CatTipoEntidadId
                };

                if (DetalleUnidadAtencion.DomicilioId != null && DetalleUnidadAtencion.DomicilioId != 0)
                {
                    var domicilio = await modelo.Domicilio
                        .Where(S => S.DomicilioId == DetalleUnidadAtencion.DomicilioId)
                        .FirstOrDefaultAsync();

                    if (domicilio != null)
                    {
                        resultado.DomicilioId = DetalleUnidadAtencion.DomicilioId;
                        domicilio.Calle = DetalleUnidadAtencion.Calle;
                        domicilio.NumeroExterior = DetalleUnidadAtencion.NumeroExterior;
                        domicilio.NumeroInterior = DetalleUnidadAtencion.NumeroInterior;
                        domicilio.CodigoPostal = DetalleUnidadAtencion.CodigoPostal;
                        domicilio.Colonia = DetalleUnidadAtencion.Colonia;
                        domicilio.Municipio = DetalleUnidadAtencion.Municipio;
                    }
                }
                else
                {
                    var domicilio = new Domicilio
                    {
                        Calle = DetalleUnidadAtencion.Calle,
                        NumeroExterior = DetalleUnidadAtencion.NumeroExterior,
                        NumeroInterior = DetalleUnidadAtencion.NumeroInterior,
                        CodigoPostal = DetalleUnidadAtencion.CodigoPostal,
                        Colonia = DetalleUnidadAtencion.Colonia,
                        Municipio = DetalleUnidadAtencion.Municipio,
                        EsActivo = true
                    };

                    modelo.Domicilio.Add(domicilio);

                    await modelo.SaveChangesAsync();

                    resultado.DomicilioId = domicilio.DomicilioId;
                }

                modelo.UnidadAtencion.AddOrUpdate(resultado);

                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CatalogoReques"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public async Task<PagedInformationDTO<UnidadAtencionDTO>> ConsultaConceptos(CatalogoRequestDTO CatalogoReques,
            bool? EsActivo)
        {
            var paginado = CatalogoReques.Paginado;

            var pageInfo = new PagedInformationDTO<UnidadAtencionDTO>
            {
                SetElementosPorPagina = paginado.PageSize,
                CurrentPage = paginado.CurrentPage,
                QueryString = paginado.QueryString
            };

            using (var modelo = new ISSSTEEntities())
            {
                var requestsQuery = (from a in modelo.UnidadAtencion
                    join b in modelo.Domicilio on a.DomicilioId equals b.DomicilioId into ab
                    from res in ab.DefaultIfEmpty()
                    select new UnidadAtencionDTO
                    {
                        UnidadAtencionId = a.UnidadAtencionId,
                        Descripcion = a.Descripcion,
                        EsActivo = a.EsActivo,
                        CatTipoEntidadId = a.CatTipoEntidadId,
                        Contador = a.Contador,
                        Prefijo = a.Prefijo,
                        Mascara = a.Mascara,
                        ReiniciarContador = a.ReiniciarContador,
                        DomicilioId = res == null ? default(int) : res.DomicilioId,
                        Calle = res == null ? string.Empty : res.Calle,
                        NumeroExterior = res == null ? string.Empty : res.NumeroExterior,
                        NumeroInterior = res == null ? string.Empty : res.NumeroInterior,
                        CodigoPostal = res == null ? default(int) : res.CodigoPostal,
                        Colonia = res == null ? string.Empty : res.Colonia,
                        Municipio = res == null ? string.Empty : res.Municipio
                    }).AsQueryable();

                if (EsActivo.HasValue)
                    requestsQuery = requestsQuery.Where(R => R.EsActivo == EsActivo);

                if (!string.IsNullOrEmpty(pageInfo.GetFiltroBusqueda))
                    requestsQuery = requestsQuery
                        .Where(R => R.Descripcion.ToLower().Contains(pageInfo.GetFiltroBusqueda.ToLower()));

                var requestCount = requestsQuery;

                pageInfo.ResultCount = await requestCount.CountAsync();

                pageInfo.ResultList = await requestsQuery
                    .OrderByDescending(R => R.UnidadAtencionId)
                    .Skip(pageInfo.GetElementosPorPagina * (pageInfo.GetCurrentPage - 1))
                    .Take(pageInfo.GetElementosPorPagina)
                    .ToListAsync();
            }

            return pageInfo;
        }
    }
}