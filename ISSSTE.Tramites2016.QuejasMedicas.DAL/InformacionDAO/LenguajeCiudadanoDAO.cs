using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO
{
    public class LenguajeCiudadanoDAO : DoDisposeDTO
    {
        /// <summary>
        ///     Consulta del lenguaje ciudadano
        /// </summary>
        /// <param name="Paginado"></param>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public async Task<PagedInformationDTO<CatalogoResultDTO>> ConsultaLenguajeCiudadano(
            BasePageInformationDTO Paginado, bool? EsActivo)
        {
            var pageInfo = new PagedInformationDTO<CatalogoResultDTO>
            {
                SetElementosPorPagina = Paginado.PageSize,
                CurrentPage = Paginado.CurrentPage,
                QueryString = Paginado.QueryString
            };

            using (var modelo = new ISSSTEEntities())
            {
                var requestsQuery = (from a in modelo.LenguajeCiudadano
                    where !EsActivo.HasValue || a.EsActivo == EsActivo
                    select new CatalogoResultDTO
                    {
                        CatalogoId = a.LenguajeCiudadanoId,
                        Concepto = a.Region,
                        Valor = a.Concepto,
                        EsActivo = a.EsActivo,
                        Descripcion = a.Descripcion,
                        TipoDato = "LetraNumero"
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

                return pageInfo;
            }
        }

        /// <summary>
        ///     Actualización del lenguaje ciudadano
        /// </summary>
        /// <param name="SettingDetail"></param>
        /// <returns></returns>
        public async Task<int> ActualizacionLenguajeCiudadano(CatalogoResultDTO SettingDetail)
        {
            using (var modelo = new ISSSTEEntities())
            {
                var resultado =
                    await modelo.LenguajeCiudadano
                        .Where(S => S.LenguajeCiudadanoId == SettingDetail.CatalogoId)
                        .FirstOrDefaultAsync();

                resultado.Region = SettingDetail.Concepto;
                resultado.Concepto = SettingDetail.Valor;

                return await modelo.SaveChangesAsync();
            }
        }

        /// <summary>
        ///     Mensajes del lado del Servidor
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> ConsultaLenguajeCiudadanoServer()
        {
            var mensajeServidor = new Dictionary<string, string>();
            using (var modelo = new ISSSTEEntities())
            {
                var duplaValoresDto = (from a in modelo.LenguajeCiudadano
                        where a.EsActivo && !a.EsDeVista
                        select new DuplaValoresDTO
                        {
                            Informacion = a.IdTecnico,
                            Valor = a.Concepto
                        }).OrderBy(O => O.Valor)
                    .ToList();

                foreach (var item in duplaValoresDto)
                    mensajeServidor.Add(item.Informacion, item.Valor);
            }
            return mensajeServidor;
        }


        public async Task<Dictionary<string, string>> LenguajeCiudadanoServerAsync(List<string> Llaves)
        {
            Dictionary<string, string> valor = new Dictionary<string, string>();

            Llaves.ForEach(S => { S.Trim().ToUpper(); });

            using (var modelo = new ISSSTEEntities())
            {
                valor = await modelo.LenguajeCiudadano
                    .Where(T => T.EsActivo && Llaves.Contains(T.IdTecnico.ToUpper()))
                    .Distinct()
                    .ToDictionaryAsync(S => S.IdTecnico, S => S.Concepto);                    ;
            }

            if (valor.Count == 0)
            {
                var llaves = string.Join(",", Llaves.ToArray());
                throw new QuejasMedicasException("No existen las configuraciones especificadas:" + llaves + ".");
            }

            return valor;
        }
        /// <summary>
        ///     Mensajes del lado del cliente
        /// </summary>
        /// <returns></returns>
        public List<DuplaValoresDTO> ConsultaLenguajeCiudadano()
        {
            List<DuplaValoresDTO> duplaValoresDto;
            using (var modelo = new ISSSTEEntities())
            {
                duplaValoresDto = (from a in modelo.LenguajeCiudadano
                        where a.EsActivo && a.EsDeVista
                        select new DuplaValoresDTO
                        {
                            Informacion = a.IdTecnico,
                            Valor = a.Concepto
                        }).OrderBy(O => O.Valor)
                    .ToList();
            }
            return duplaValoresDto;
        }
    }
}