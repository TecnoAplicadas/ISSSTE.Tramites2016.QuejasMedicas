using System.Collections.Generic;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO
{
    public class InformacionAtributos
    {
        public InformacionAtributos()
        {
            Propiedades = new CatalogoAtributoInfo();
        }

        public string NombrePropiedad { get; set; }
        public CatalogoAtributoInfo Propiedades { get; set; }
    }


    public class InformacionCatalogo
    {
        public List<InformacionAtributos> Atributos { get; set; }

        public dynamic Resultado { get; set; }
    }
}