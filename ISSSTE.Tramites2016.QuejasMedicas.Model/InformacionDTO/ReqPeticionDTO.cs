using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO
{
    public class ReqPeticionDTO : DoDisposeDTO
    {
        public BasePageInformationDTO Paginado { get; set; }
        public int? RequestTypeId { get; set; }
    }
}