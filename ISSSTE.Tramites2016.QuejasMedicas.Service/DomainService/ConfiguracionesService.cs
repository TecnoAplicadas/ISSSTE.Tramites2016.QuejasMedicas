using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.AdministracionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Domain.IDomainService;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.Base;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.DomainService
{
    public class ConfiguracionesService : IConfiguracionesService
    {
        public async Task<PagedInformationDTO<CatalogoResultDTO>> ConsultaCatSysConfiguracion(
            BasePageInformationDTO Paginado, bool? EsActivo)
        {
            var catSysConfiguracionDao = new CatSysConfiguracionDAO();
            return await catSysConfiguracionDao.ConsultaConceptos(Paginado, EsActivo);
        }


        public async Task<int> SaveCatSysConfiguracion(CatalogoResultDTO SettingDetail)
        {
            var catSysConfiguracionDao = new CatSysConfiguracionDAO();
            return await catSysConfiguracionDao.SaveCatSysConfiguracion(SettingDetail);
        }

        public async Task<PagedInformationDTO<CatalogoResultDTO>> ConsultaSysLenguajeCiudadano(
            BasePageInformationDTO Paginado, bool? EsActivo)
        {
            var lenguajeCiudadanoDao = new LenguajeCiudadanoDAO();
            return await lenguajeCiudadanoDao.ConsultaLenguajeCiudadano(Paginado, EsActivo);
        }

        public async Task<int> SaveCatSysLenguajeCiudadano(CatalogoResultDTO SettingDetail)
        {
            var lenguajeCiudadanoDao = new LenguajeCiudadanoDAO();
            return await lenguajeCiudadanoDao.ActualizacionLenguajeCiudadano(SettingDetail);
        }

        public async Task<PagedInformationDTO<RequisitoDTO>> ConsultaRequisitos(BasePageInformationDTO Paginado,
            bool? EsActivo, int? CatTipoTramiteId)
        {
            var requisitoDao = new RequisitoDAO();
            return await requisitoDao.ConsultaRequisitosPaginado(Paginado, EsActivo, CatTipoTramiteId);
        }

        public async Task<int> SaveRequirementDetail(RequisitoDTO RequirementDetail)
        {
            var requisitoDao = new RequisitoDAO();
            return await requisitoDao.SalvarActualizarRequisito(RequirementDetail);
        }
    }
}