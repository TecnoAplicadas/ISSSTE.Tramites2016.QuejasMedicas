using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.ReportesDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.ReporteService
{
    public class ReporteService
    {
        public List<GeneralCitaDTO> ListaReporte1(GeneralCitaDTO Filtros)
        {
            var reporteDao = new ColumnasDAO();
            return reporteDao.ListaReporte1(Filtros);
        }

        public List<EstadoCitaCortoDTO> ListaReporte2(GeneralCitaDTO Filtros)
        {
            var reporteDao = new ColumnasDAO();
            return reporteDao.ListaReporte2(Filtros);
        }
    }
}