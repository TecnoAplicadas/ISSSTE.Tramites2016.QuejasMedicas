using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class PromoventeDTO : CitaDTO
    {
        public List<SeccionDTO> SeccionPrincipal { get; set; }
        public List<SeccionDTO> SeccionSecundaria { get; set; }
        public List<DuplaValoresDTO> MensajesVista { get; set; }
        public DuplaValoresDTO DuplaValores { get; set; }
    }

    public class SeccionDTO : DoDisposeDTO
    {
        public int? Orden { get; set; }
        public int SeccionId { get; set; }
        public string Header1 { get; set; }
        public string Header2 { get; set; }
        public string Tag { get; set; }
        public string Titulo { get; set; }
        public string Imagen { get; set; }
        public string Detalle { get; set; }
    }
}