using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO
{
    public class CatalogoRequestDTO
    {
        public BasePageInformationDTO Paginado { get; set; }
        public int CatalogoId { get; set; }
    }
}