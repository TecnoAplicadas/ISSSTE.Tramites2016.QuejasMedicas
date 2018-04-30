using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService
{
    public interface IConfiguracionesService
    {
        Task<PagedInformationDTO<CatalogoResultDTO>> ConsultaCatSysConfiguracion(BasePageInformationDTO Paginado,
            bool? EsActivo);

        Task<PagedInformationDTO<CatalogoResultDTO>> ConsultaSysLenguajeCiudadano(BasePageInformationDTO Paginado,
            bool? EsActivo);

        Task<int> SaveCatSysConfiguracion(CatalogoResultDTO SettingDetail);

        Task<int> SaveCatSysLenguajeCiudadano(CatalogoResultDTO SettingDetail);

        Task<int> SaveRequirementDetail(RequisitoDTO RequirementDetail);

        Task<PagedInformationDTO<RequisitoDTO>> ConsultaRequisitos(BasePageInformationDTO Paginado, bool? EsActivo,
            int? CatTipoTramiteId);
    }
}