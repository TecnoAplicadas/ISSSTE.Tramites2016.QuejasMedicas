using System;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.InformacionService
{
    public class CitaService
    {
        public CitaDTO GetCita(CitaDTO CitaDto,bool CualquierEstado=false)
        {
            var citaDao = new CitaDAO();
            return citaDao.GetCita(CitaDto, CualquierEstado);
        }

        public async Task CancelacionAutomaticaDelDia()
        {
            var citaDao = new CitaDAO();
            await citaDao.CancelacionAutomaticaDelDia();
        }

        public async Task ReinicioContador()
        {
            var citaDao = new CitaDAO();
            await citaDao.ReinicioContador();
        }

        public void GetDomicilio(int? DomicilioId, CitaDTO CitaDto)
        {
            var citaDao = new CitaDAO();
            citaDao.GetDomicilio(DomicilioId, CitaDto);
        }

        public string AgendarCita(GeneralCitaDTO GeneralCitaDto)
        {
            var citaDao = new CitaDAO();
            return citaDao.AgendarCita(GeneralCitaDto);
        }

        public string CancelarCita(GeneralCitaDTO GeneralCitaDto)
        {
            var citaDao = new CitaDAO();
            return citaDao.CancelarCita(GeneralCitaDto);
        }

        public bool BloqueoPersona(PersonaDTO PersonaDto)
        {
            var citaDao = new CitaDAO();
            return citaDao.BloqueoPersona(PersonaDto);
        }

        public int CitasVigentesPorTramite(PersonaDTO PersonaDto)
        {
            var citaDao = new CitaDAO();
            return citaDao.CitasVigentesPorTramite(PersonaDto);
        }
    }
}