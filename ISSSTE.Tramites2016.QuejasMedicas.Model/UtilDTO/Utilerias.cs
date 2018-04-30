using System;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO
{
    public class Utilerias : DoDisposeDTO
    {
        /// <summary>
        ///     cálculo de la edad
        /// </summary>
        /// <param name="Curp"></param>
        /// <returns></returns>
        public static int GetEdad(string Curp)
        {
            var edad = 0;
            try
            {
                var fechaNacimiento = GetFechaNacimiento(Curp);
                edad = DateTime.Today.AddTicks(-fechaNacimiento.Ticks).Year - 1;
            }
            catch (Exception)
            {
                // ignored
            }
            return edad;
        }

        /// <summary>
        ///     Cálculo del género
        /// </summary>
        /// <param name="Curp"></param>
        /// <returns></returns>
        public static bool GetSexo(string Curp)
        {
            var valor = false;
            try
            {
                valor = Curp[10] == 'H';
            }
            catch (Exception)
            {
                // ignored
            }
            return valor;
        }

        /// <summary>
        ///     Cálculo de la fecha de nacimiento
        /// </summary>
        /// <param name="Curp"></param>
        /// <returns></returns>
        public static DateTime GetFechaNacimiento(string Curp)
        {
            var fechaNacimiento = new DateTime();
            try
            {
                var anio = Curp.Substring(4, 2);
                var mes = int.Parse(Curp.Substring(6, 2));
                var dia = int.Parse(Curp.Substring(8, 2));
                var aNo = Curp.Substring(16, 1);
                aNo = char.IsNumber(aNo[0]) ? "19" + anio : "20" + anio;
                fechaNacimiento = new DateTime(int.Parse(aNo), mes, dia);
            }
            catch (Exception)
            {
                // ignored
            }
            return fechaNacimiento;
        }
    }
}