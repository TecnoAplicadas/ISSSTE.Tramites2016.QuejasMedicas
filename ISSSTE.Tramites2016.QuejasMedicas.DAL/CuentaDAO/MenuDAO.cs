using System.Collections.Generic;
using System.Linq;
using ISSSTE.Tramites2016.QuejasMedicas.Model.CuentaDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO
{
    public class MenuDAO : DoDisposeDTO
    {
        /// <summary>
        ///     Construcción del menú principal
        /// </summary>
        /// <param name="Roles"></param>
        /// <returns></returns>
        public MenuDTO ConstruyeListaMenuLj(List<string> Roles)
        {
            var query = new List<ModuloDTO>();
            var busqueda = ConstruyeListaMenu(Roles);

            if (busqueda != null && busqueda.Count > 0)
                query = (from item in busqueda
                    where item.idPadre == null
                    select new ModuloDTO
                    {
                        href = item.href,
                        iconUrl = item.iconUrl,
                        id = item.id,
                        idPadre = item.idPadre,
                        text = item.text,
                        DescripcionModulo = item.DescripcionModulo,
                        children = ObtenerHijos(item.id, busqueda)
                    }).ToList();

            var menu = new MenuDTO {menu = query};

            return menu;
        }

        /// <summary>
        ///     Obtención de hijos
        /// </summary>
        /// <param name="IdElemento"></param>
        /// <param name="ListaBusqueda"></param>
        /// <returns></returns>
        private List<ModuloDTO> ObtenerHijos(int IdElemento, List<ModuloDTO> ListaBusqueda)
        {
            var query = (from item in ListaBusqueda
                let tieneHijos = ListaBusqueda.Any(O => O.idPadre == item.id)
                where item.idPadre == IdElemento
                select new ModuloDTO
                {
                    href = item.href,
                    iconUrl = item.iconUrl,
                    id = item.id,
                    idPadre = item.idPadre,
                    text = item.text,
                    DescripcionModulo = item.DescripcionModulo,
                    children = tieneHijos ? ObtenerHijos(item.id, ListaBusqueda) : null
                }).ToList();

            return query;
        }

        /// <summary>
        ///     Recursividad en el menú
        /// </summary>
        /// <param name="Roles"></param>
        /// <returns></returns>
        public List<ModuloDTO> ConstruyeListaMenu(List<string> Roles)
        {
            List<ModuloDTO> result;
            using (var modelo = new ISSSTEEntities())
            {
                result = (from cm in modelo.CatSysModulo
                    join pm in modelo.SysAcceso on cm.CatSysModuloId equals pm.CatSysModuloId
                    join cp in modelo.CatSysRol on pm.CatSysRolId equals cp.CatSysRolId
                    where Roles.Contains(cp.Concepto) && pm.EsActivo
                    select new ModuloDTO
                    {
                        href = cm.ActionName,
                        DescripcionModulo = cm.Concepto,
                        EsActivo = cm.EsActivo,
                        iconUrl = cm.Icono,
                        id = cm.CatSysModuloId,
                        idPadre = cm.CatSysModuloPadreId,
                        text = cm.LinkText
                    }).ToList();
            }
            return result;
        }
    }
}