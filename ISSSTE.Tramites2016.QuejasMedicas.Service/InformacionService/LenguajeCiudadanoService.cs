using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.InformacionService
{
    public class LenguajeCiudadanoService
    {
        public Dictionary<string, string> ConsultaLenguajeCiudadanoServer()
        {
            var lenguajeCiudadanoDao = new LenguajeCiudadanoDAO();
            return lenguajeCiudadanoDao.ConsultaLenguajeCiudadanoServer();
        }

        public List<DuplaValoresDTO> ConsultaLenguajeCiudadano()
        {
            var lenguajeCiudadanoDao = new LenguajeCiudadanoDAO();
            return lenguajeCiudadanoDao.ConsultaLenguajeCiudadano();
        }

     
    }
}