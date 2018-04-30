using System.Globalization;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class DomicilioDTO : DoDisposeDTO
    {
        public int EntidadId { get; set; }

        public int MunicipioId { get; set; }

      
        public int? CodigoPostal { get; set; }

        public string CodPostal { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Código postal", EsEditable = true, EsRequerido = false, TipoDato = TipoDeDatoEnum.numero,TamanioMaximo =6)]
        public string CodigoPostalVista {
            get { return (CodigoPostal == null ? string.Empty : CodigoPostal.ToString().PadLeft(5, '0')); }
            set {

                int TempcodigoPostal = 0;
                bool esNumero =int.TryParse(value, out TempcodigoPostal);
                CodigoPostal = esNumero ? TempcodigoPostal : default(int);
            }
        }

        public int? DomicilioId { get; set; }

        public string Entidad { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Municipio", EsEditable = true, EsRequerido = false, TamanioMaximo = 50)]
        public string Municipio { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Colonia", EsEditable = true, EsRequerido = false, TamanioMaximo = 50)]
        public string Colonia { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Calle", EsEditable = true, EsRequerido = false, TamanioMaximo = 100)]
        public string Calle { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Número interior", EsEditable = true, EsRequerido = false, TamanioMaximo = 50)]
        public string NumeroInterior { get; set; }

        [CatalogoAtributoInfo(NombreVista = "Número exterior", EsEditable = true, EsRequerido = false, TamanioMaximo = 30)]
        public string NumeroExterior { get; set; }

        public int TramiteUnidadAtencionId { get; set; }
    }
}