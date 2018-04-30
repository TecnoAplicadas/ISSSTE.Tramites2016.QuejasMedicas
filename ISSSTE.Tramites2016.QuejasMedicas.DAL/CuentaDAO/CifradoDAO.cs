using System;
using System.Security.Cryptography;
using System.Text;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Enums;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO
{
    public class CifradoDAO : DoDisposeDTO
    {
        /// <summary>
        ///     Devuelve el valor asociado a una cadena cifrada
        /// </summary>
        /// <param name="Texto"></param>
        /// <param name="Llave"></param>
        /// <returns></returns>
        public string Cifrar(string Texto, string Llave = "")
        {
            if (string.IsNullOrEmpty(Llave))
                Llave =
                    new ConfiguracionDAO().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.LlaveCifradoUsuario);

            byte[] llaveArreglo, textoArreglo, textoCifrado;

            using (var objHashMd5 = new MD5CryptoServiceProvider())
            {
                textoArreglo = Encoding.UTF8.GetBytes(Texto);
                llaveArreglo = objHashMd5.ComputeHash(Encoding.UTF8.GetBytes(Llave));
            }

            using (var objTdes = new TripleDESCryptoServiceProvider
            {
                Key = llaveArreglo,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {
                var objCTransform = objTdes.CreateEncryptor();
                textoCifrado = objCTransform.TransformFinalBlock(textoArreglo, 0, textoArreglo.Length);
            }
            //se regresa el resultado en forma de una cadena
            return Convert.ToBase64String(textoCifrado, 0, textoCifrado.Length);
        }

        /// <summary>
        ///     Devuelve el valor original de una cadena cifrada
        /// </summary>
        /// <param name="TextoEncriptado"></param>
        /// <param name="Llave"></param>
        /// <returns></returns>
        public string Descifrar(string TextoEncriptado, string Llave = "")
        {
            if (string.IsNullOrEmpty(Llave))
                Llave =
                    new ConfiguracionDAO().GetValorConfiguracion(Enumeracion.EnumSysConfiguracion.LlaveCifradoUsuario);

            byte[] llaveArreglo, resultadoArreglo;
            var arregloDescifrado = Convert.FromBase64String(TextoEncriptado);

            using (var objHashMd5 = new MD5CryptoServiceProvider())
            {
                llaveArreglo = objHashMd5.ComputeHash(Encoding.UTF8.GetBytes(Llave));
            }

            using (var objTdes = new TripleDESCryptoServiceProvider
            {
                Key = llaveArreglo,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {
                var objCTransform = objTdes.CreateDecryptor();
                resultadoArreglo = objCTransform.TransformFinalBlock(arregloDescifrado, 0, arregloDescifrado.Length);
            }
            return Encoding.UTF8.GetString(resultadoArreglo);
        }
    }
}