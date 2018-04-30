using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CuentaDTO
{
    public class MenuDTO : DoDisposeDTO
    {
        public MenuDTO()
        {
            menu = new List<ModuloDTO>();
        }

        public List<ModuloDTO> menu { get; set; }
    }
}