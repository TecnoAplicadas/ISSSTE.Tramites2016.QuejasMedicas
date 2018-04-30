using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO
{
    public class ReflexionHelper
    {
        public static List<InformacionAtributos> ObtenerElementosLinq(object Objeto, MemberTypes TipoElemento)
        {
            var atributos = (from i in Objeto.GetType().GetMembers()
                where i.MemberType == TipoElemento
                select new InformacionAtributos
                {
                    NombrePropiedad = ((PropertyInfo) i).Name,
                    Propiedades = ((PropertyInfo) i).GetCustomAttributes(typeof(CatalogoAtributoInfo), false)
                        .Select(S => new CatalogoAtributoInfo
                        {
                            EsEditable = ((CatalogoAtributoInfo) S).EsEditable,
                            EsVisible = ((CatalogoAtributoInfo) S).EsVisible,
                            NombreVista = ((CatalogoAtributoInfo) S).NombreVista,
                            Orden = ((CatalogoAtributoInfo) S).Orden,
                            EsRequerido = ((CatalogoAtributoInfo) S).EsRequerido,
                            Tipo = ((CatalogoAtributoInfo) S).Tipo,
                            TamanioMaximo = ((CatalogoAtributoInfo)S).TamanioMaximo,
                            TipoDato = ((CatalogoAtributoInfo)S).TipoDato
                        })
                        .FirstOrDefault()
                }).ToList();

            return atributos;
        }
    }
}