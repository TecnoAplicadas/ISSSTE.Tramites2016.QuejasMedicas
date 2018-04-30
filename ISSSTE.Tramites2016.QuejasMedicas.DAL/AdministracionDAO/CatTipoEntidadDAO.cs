using System.Collections.Generic;
using System.Linq;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.AdministracionDAO
{
    public class CatTipoEntidadDAO : DoDisposeDTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EsActivo"></param>
        /// <returns></returns>
        public List<CatTipoEntidadDTO> ConsultaConceptosCatTipoEntidad(bool? EsActivo)
        {
            List<CatTipoEntidadDTO> listado;
            using (var modelo = new ISSSTEEntities())
            {
                listado = (from a in modelo.CatTipoEntidad
                    where !EsActivo.HasValue || a.EsActivo == EsActivo
                    select new CatTipoEntidadDTO
                    {
                        CatTipoEntidadId = a.CatTipoEntidadId,
                        Concepto = a.Concepto,
                        EsActivo = a.EsActivo,
                        Siglas = a.Siglas
                    }).ToList();
            }
            return listado;
        }
    }
}