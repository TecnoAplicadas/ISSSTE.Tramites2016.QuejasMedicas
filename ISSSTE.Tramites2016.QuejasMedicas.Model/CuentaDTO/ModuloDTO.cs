using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.CuentaDTO
{
    public class ModuloDTO : DoDisposeDTO
    {
        public ModuloDTO()
        {
            children = new List<ModuloDTO>();
            text = string.Empty;
            iconUrl = string.Empty;
            href = string.Empty;
            DescripcionModulo = string.Empty;
        }

        public int id { get; set; }
        public int? idPadre { get; set; }

        public List<ModuloDTO> children { get; set; }

        public string href { get; set; }
        public string iconUrl { get; set; }
        public string text { get; set; }
        public string DescripcionModulo { get; set; }

        public bool EsActivo { get; set; }
    }
}