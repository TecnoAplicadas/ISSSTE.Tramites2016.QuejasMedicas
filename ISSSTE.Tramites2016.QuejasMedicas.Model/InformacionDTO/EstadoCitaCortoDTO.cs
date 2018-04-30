using System;
using System.Collections.Generic;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class EstadoCitaCortoDTO
    {
        public List<Tuple<string, int, string>> Valores { get; set; }

        public int UnidadAtencionId { get; set; }
        public int CatTipoTramiteId { get; set; }
        public string Descripcion { get; set; }
    }
}