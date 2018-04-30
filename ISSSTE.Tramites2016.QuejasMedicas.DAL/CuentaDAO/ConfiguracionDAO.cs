using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO
{
    public class ConfiguracionDAO : DoDisposeDTO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdCatTipoTramite"></param>
        /// <returns></returns>
        public string GetFormatoTramite(int IdCatTipoTramite)
        {
            var rutaLog = GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.RutaArchivos);
            string formaPdf;
            using (var modelo = new ISSSTEEntities())
            {
                formaPdf = (from a in modelo.CatTipoTramite
                    where a.CatTipoTramiteId == IdCatTipoTramite
                    select a.FormatoUniversal).FirstOrDefault();
            }
            return string.IsNullOrEmpty(formaPdf) ? "" : Path.Combine(rutaLog, formaPdf);
        }

        /// <summary>
        ///     Valor de un parámetro de configuración desde BD
        /// </summary>
        /// <param name="Variable"></param>
        /// <returns></returns>
        public string GetValorConfiguracion(string Variable)
        {
            var valor = string.Empty;
            Variable = Variable.ToUpper().Trim();
            using (var modelo = new ISSSTEEntities())
            {
                var catSysConfiguracion =
                    modelo.CatSysConfiguracion.FirstOrDefault(T => T.Concepto.ToUpper().Contains(Variable));
                if (catSysConfiguracion != null)
                    valor = catSysConfiguracion.Valor;
            }
            return valor;
        }


        /// <summary>
        ///     Valor de un parámetro de configuración desde BD
        ///     PR => Devolver una excepcion si el parametro no se econtro.
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetValorConfiguracionAsync(List<string> Llaves)
        {
            Dictionary<string, string> valor;
            Llaves.ForEach(S => { S.Trim().ToUpper(); });

            using (var modelo = new ISSSTEEntities())
            {
                valor = await modelo.CatSysConfiguracion
                    .Where(T => Llaves.Contains(T.Concepto.ToUpper()))
                    .ToDictionaryAsync(S => S.Concepto, S => S.Valor);
            }

            if (valor.Count == 0)
            {
                var llaves = string.Join(",", Llaves.ToArray());
                throw new QuejasMedicasException("No existen las configuraciones especificadas:" + llaves + ".");
            }

            return valor;
        }
    }
}