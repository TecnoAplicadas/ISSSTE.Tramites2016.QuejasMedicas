using System;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO
{
    public class CatalogoAtributoInfo : Attribute
    {
        public CatalogoAtributoInfo()
        {
            EsEditable = false;
            EsVisible = true;
            Orden = 0;
            TamanioMaximo = 0;
            EsRequerido = true;
            NombreVista = string.Empty;
            Tipo = TipoElementoEnum.text;
            TipoDato = TipoDeDatoEnum.ninguno;
        }

        public bool EsRequerido { get; set; }
        public string NombreVista { get; set; }
        public bool EsEditable { get; set; }
        public bool EsVisible { get; set; }
        public int Orden { get; set; }
        public int TamanioMaximo { get; set; }
        public TipoElementoEnum Tipo { get; set; }
        public string TipoElementoString { get { return Tipo.ToString(); } }
        public TipoDeDatoEnum TipoDato { get; set; }
        public string TipoDatoString { get { return TipoDato.ToString(); } }
    }


    public enum TipoElementoEnum
    {
        text = 1,
        color = 2,
        checkbox = 3,
        select = 4
    }

    public enum TipoDeDatoEnum
    {
        ninguno=1,
        numero = 2,
        texto = 3

    }
}