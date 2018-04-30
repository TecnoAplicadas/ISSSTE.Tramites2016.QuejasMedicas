using System.Collections.Generic;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService
{
    public class ConfiguracionService : DoDisposeDTO
    {
        public string GetValorConfiguracion(string Llave)
        {
            var configuracionDao = new ConfiguracionDAO();
            return configuracionDao.GetValorConfiguracion(Llave);
        }

        public async Task<Dictionary<string, string>> GetValorConfiguracionAsync(List<string> Llaves)
        {
            var configuracionDao = new ConfiguracionDAO();
            return await configuracionDao.GetValorConfiguracionAsync(Llaves);
        }


        public string GetFormatoTramite(int IdCatTipoTramite)
        {
            var configuracionDao = new ConfiguracionDAO();
            return configuracionDao.GetFormatoTramite(IdCatTipoTramite);
        }
    }
}