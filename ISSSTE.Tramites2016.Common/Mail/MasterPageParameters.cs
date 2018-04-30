#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#endregion

namespace ISSSTE.Tramites2016.Common.Mail
{
    public class MasterPageParameters
    {
        /// <summary>
        ///     Carga la master page por default con los parámetros por default.
        /// </summary>
        /// <param name="masterpageText">
        ///     Es el texto en html que contendra los tags: [HTMLBODY] y [HTMLFOOTER] para ser
        ///     substitu[idos por los textos de la aplicaci[on
        /// </param>
        /// <param name="pathMainLogo">ruta desde la cual se va a cargar el logo por default, que se incluye como referencia "logo"</param>
        public MasterPageParameters(string masterpageText, string pathMainLogo)
        {
            Masterpage = masterpageText;
            Add("logo", pathMainLogo);
        }

        public MasterPageParameters()
        {
        }

        public string Masterpage { get; set; }

        /// <summary>
        ///     Recursos a insertarse en el correo.
        ///     Key: identifiador
        ///     Value: Ruta desntro de la estructura de disco en donde se encuentra el archivo o contenido a insertar.
        /// </summary>
        public Dictionary<string, string> Resources { get; set; }

        /// <summary>
        ///     Carga el archivo template desde una ruta
        /// </summary>
        /// <param name="filePathMaster">Ruta del archivo: debe tener  [HTMLBODY] y [HTMLFOOTER] y usar "logo"</param>
        /// <param name="filePathLogo">Archivo del logo</param>
        public static MasterPageParameters LoadFromFile(string filePathMaster, string filePathLogo)
        {
            var result = new MasterPageParameters(File.ReadAllText(filePathMaster, Encoding.Default), filePathLogo);
            result.GeneerateAutoamticKeys();
            return result;
        }

        public void Add(string key, string value)
        {
            if (Resources == null)
                Resources = new Dictionary<string, string>();

            if (Resources.ContainsKey(key))
                Resources[key] = value;
            else
                Resources.Add(key, value);
        }

        public void GeneerateAutoamticKeys()
        {
            if (Resources?.Count > 0)
            {
                var first = Resources.FirstOrDefault();
                var path = Path.GetDirectoryName(first.Value);
                //Add("fb2png", Path.Combine(path, "fb2.png"));
                //Add("tw2png", Path.Combine(path, "tw2.png"));
                if (path != null)
                {
                    Add("gobmxlogosvg", Path.Combine(path, "gobmxlogo.png"));
                    Add("logo_mexicosvg", Path.Combine(path, "logo_mexico.png"));
                }
            }
        }
    }
}