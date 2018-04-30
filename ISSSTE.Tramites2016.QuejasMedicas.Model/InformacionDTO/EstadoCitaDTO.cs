using System;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class EstadoCitaDTO
    {
        public Tuple<int, int, string> Valores { get; set; }
        public string Descripcion { get; set; }

        public string Concepto { get; set; }
        public int EstadoCitaId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaCita { get; set; }
    }
}