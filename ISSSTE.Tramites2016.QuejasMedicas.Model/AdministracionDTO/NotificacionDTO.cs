using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class NotificacionDTO
    {
        public NotificacionDTO()
        {
            ListaCitasPendientes = new List<CitaDTO>();
            NumeroCitasPendientes = default(int);
            NumeroCitasPorVencer = default(int);
        }

        public int Total => NumeroCitasPendientes + NumeroCitasPorVencer;
        public List<CitaDTO> ListaCitasPendientes { get; set; }
        public int NumeroCitasPendientes { get; set; }
        public int NumeroCitasPorVencer { get; set; }
    }
}