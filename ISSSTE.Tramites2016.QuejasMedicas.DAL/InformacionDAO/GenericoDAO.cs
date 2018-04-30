using System;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO
{
    public class GenericoDAO : DoDisposeDTO
    {
        private const string Contexto = Enumeracion.EnumVarios.Contexto;
        private const string LLaveFija = "ISSSTE";

        /// <summary>
        ///     Palabra a cifrar
        /// </summary>
        /// <param name="Palabra"></param>
        /// <returns></returns>
        public string PalabraCifrada(string Palabra)
        {
            var cifradoDao = new CifradoDAO();
            return cifradoDao.Cifrar(Palabra, LLaveFija);
        }

        /// <summary>
        ///     Eliminación / Reactivación genérica
        /// </summary>
        /// <param name="DuplaValores"></param>
        /// <returns></returns>
        public string DeleteReactive(DuplaValoresDTO DuplaValores)
        {
            var cifradoDao = new CifradoDAO();
            DuplaValores.Valor = cifradoDao.Descifrar(DuplaValores.Valor, LLaveFija);

            var resultado = Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.RegistroEliminado];
            var esActivo = false;
            ConsultaEstadoRegistro(DuplaValores, ref resultado, ref esActivo);
            if (esActivo && !DuplaValores.Reactivar
                || esActivo == false && DuplaValores.Reactivar)
            {
                ActualizacionEstadoRegistro(DuplaValores, esActivo);
            }
            else
            {
                if (esActivo == false && !DuplaValores.Reactivar)
                    throw new Exception(Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.RegistroYaEliminado]);
                if (esActivo && DuplaValores.Reactivar)
                    throw new Exception(Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.RegistroYaActivo]);
            }
            return resultado;
        }

        /// <summary>
        ///     Cadena de conexión a la BD
        /// </summary>
        /// <returns></returns>
        private dynamic CadenaConexion()
        {
            dynamic result = ConfigurationManager.ConnectionStrings[Contexto].ToString();
            result = Regex.Split(result, ";MultipleActiveResultSets")[0];
            result = Regex.Split(result, "data source")[1];
            result = result.Replace("\"", "");
            return "data source" + result;
        }

        /// <summary>
        ///     Actualización de estados
        /// </summary>
        /// <param name="Modelo"></param>
        /// <param name="EsActivo"></param>
        private void ActualizacionEstadoRegistro(DuplaValoresDTO Modelo, bool EsActivo)
        {
            var result = CadenaConexion();


            //// Modelo.Database.ExecuteSqlCommand("DELETE FROM Usuario WHERE id IN (@p0, @p1)", 1, 4);
            using (var con = new SqlConnection(result))
            {
                try
                {
                    con.Open();

                    var valor = EsActivo ? 0 : 1;
                    var sql = @"UPDATE " + Modelo.Valor + " SET EsActivo=" + valor + " WHERE " + Modelo.Valor + "Id =" +
                              Modelo.Id;

                    var command = new SqlCommand(sql, con) {CommandType = CommandType.Text};
                    command.ExecuteNonQuery();
                }
                catch
                {
                    // ignored
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        /// <summary>
        ///     Consulta de estados
        /// </summary>
        /// <param name="Modelo"></param>
        /// <param name="Resultado"></param>
        /// <param name="EsActivo"></param>
        private void ConsultaEstadoRegistro(DuplaValoresDTO Modelo, ref string Resultado, ref bool EsActivo)
        {
            using (var con = new EntityConnection("name=" + Contexto))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "SELECT st.EsActivo FROM " + Contexto + "." + Modelo.Valor;
                cmd.CommandText += " AS st WHERE st." + Modelo.Valor + "Id =" + Modelo.Id;
                using (var rdr = cmd.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection))
                {
                    while (rdr.Read())
                    {
                        EsActivo = rdr.GetBoolean(0);
                        if (EsActivo == false)
                            Resultado = Enumeracion.MensajesServidor[Enumeracion.EnumConstErrores.RegistroActivado];
                    }
                }
            }
        }

        /// <summary>
        /// Validaciones del Entity
        /// </summary>
        /// <param name="Ex"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string DB_Validacion<T>(T Ex)
        {
            var entityValidationErrors = new StringBuilder();
            try
            {
                var dbEntityValidationException = Ex as DbEntityValidationException;
                var validationErrors =
                    dbEntityValidationException?.EntityValidationErrors.FirstOrDefault();

                if (validationErrors != null)
                    foreach (var errores in validationErrors.ValidationErrors)
                    {
                        entityValidationErrors.Append("Propiedad: " + errores.PropertyName + ", ");
                        entityValidationErrors.Append("Error: " + errores.ErrorMessage);
                        entityValidationErrors.AppendLine();
                    }
            }
            catch
            {
                // ignored
            }
            return entityValidationErrors.ToString();
        }
    }
}