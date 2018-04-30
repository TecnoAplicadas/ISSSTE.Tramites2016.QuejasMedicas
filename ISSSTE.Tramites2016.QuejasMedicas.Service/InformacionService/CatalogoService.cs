using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.InformacionService
{
    public class CatalogoService
    {
        public List<DuplaValoresDTO> GetUadyCs(int IdSesionTipoTramite)
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetUadyCs(IdSesionTipoTramite);
        }

        public List<DuplaValoresDTO> GetUserUADyCS(int[] UADyCSIds)
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetUserUADyCS(UADyCSIds);
        }
        
        public List<DuplaValoresDTO> GetTramiteUnidadMedica(CalendarioDTO CalendarioDto)
        {
            var catalogoDao = new CatalogoDAO();            
            return catalogoDao.GetTramiteUnidadMedica(CalendarioDto);
        }

        public List<DuplaValoresDTO> GetTramiteUnidadMedicaEspecial(CalendarioDTO CalendarioDto)
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetTramiteUnidadMedicaEspecial(CalendarioDto);
        }

        public CalendarioDTO GetBusqueda(string Concepto, int IdCatTipoTramite)
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetBusqueda(Concepto, IdCatTipoTramite);
        }

        public List<HorarioDTO> GetBusquedaDia(string Concepto, int TramiteUnidadAtencionId)
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetBusquedaDia(Concepto, TramiteUnidadAtencionId);
        }

        public List<HorarioDTO> GetBusquedaHorario(CalendarioDTO CalendarioDto)
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetBusquedaHorario(CalendarioDto);
        }

        public List<DuplaValoresDTO> GetBusquedaTipoTramite()
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetBusquedaTipoTramite();
        }

        public string GetBusquedaHorarioUadyCs(int TramiteUnidadAtencionId)
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetBusquedaHorarioUadyCs(TramiteUnidadAtencionId);
        }

        public string GetHomClave(int CatTipoTramiteId)
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetHomoClave(CatTipoTramiteId);
        }

        public List<DuplaValoresDTO> GetBusquedaEstadoCita()
        {
            var catalogoDao = new CatalogoDAO();
            return catalogoDao.GetBusquedaEstadoCita();
        }
    }
}