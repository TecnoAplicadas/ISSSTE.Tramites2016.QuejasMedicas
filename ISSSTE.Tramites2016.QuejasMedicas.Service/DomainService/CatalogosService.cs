using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.AdministracionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;
using Newtonsoft.Json;
using static ISSSTE.Tramites2016.QuejasMedicas.Model.Enums.Enumeracion;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.DomainService
{
    public class CatalogosService
    {
        public async Task<List<CatalogoDTO>> ObtenerCatalogos()
        {
            return await Task.Run(() => new List<CatalogoDTO>
            {
                new CatalogoDTO {CatalogoId = (int) CatalogoEnum.TipoTramite, Descripcion = "Tipos de trámite"},
                new CatalogoDTO {CatalogoId = (int) CatalogoEnum.EstadoCita, Descripcion = "Estados de la cita"},
                new CatalogoDTO {CatalogoId = (int) CatalogoEnum.UnidadMedica, Descripcion = "Unidades médicas"},
                new CatalogoDTO {CatalogoId = (int) CatalogoEnum.UnidadAtencion, Descripcion = "UADyCS"},
                new CatalogoDTO
                {
                    CatalogoId = (int) CatalogoEnum.TramiteUnidadAtencion,
                    Descripcion = "Asignar UADyCS a trámite"
                }
            });
        }


        public async Task<AsignacionesTramiteUnidadDTO> ConsultaAsignacionesTramiteUnidad(int CatTipoTramiteId,
            bool? EsActivo)
        {
            return await new TramiteUnidadAtencionDAO().ConsultaAsignacionesTramiteUnidad(CatTipoTramiteId, EsActivo);
        }

        public async Task<int> AsignacionesTramiteUnidad(AsignacionesTramiteUnidadDTO AsignacionesTramiteUnidad)
        {
            return await new TramiteUnidadAtencionDAO().AsignacionesTramiteUnidad(AsignacionesTramiteUnidad);
        }

        public async Task<InformacionCatalogo> ConsultaCatalogoPorIdCatalogo(CatalogoRequestDTO CatalogoReques)
        {
            var informacionCatalogo = new InformacionCatalogo();


            switch (CatalogoReques.CatalogoId)
            {
                case (int) CatalogoEnum.TipoTramite:
                    informacionCatalogo.Atributos =
                        ReflexionHelper.ObtenerElementosLinq(new CatTipoTramiteDTO(), MemberTypes.Property);
                    informacionCatalogo.Resultado =
                        await new CatTipoTramiteDAO().ConsultaConceptos(CatalogoReques, null);
                    break;
                case (int) CatalogoEnum.EstadoCita:

                    informacionCatalogo.Atributos =
                        ReflexionHelper.ObtenerElementosLinq(new CatTipoEdoCitaDTO(), MemberTypes.Property);
                    informacionCatalogo.Resultado =
                        await new CatTipoEdoCitaDAO().ConsultaConceptos(CatalogoReques, null);
                    break;
                case (int) CatalogoEnum.UnidadAtencion:
                    informacionCatalogo.Atributos =
                        ReflexionHelper.ObtenerElementosLinq(new UnidadAtencionDTO(), MemberTypes.Property);
                    informacionCatalogo.Resultado =
                        await new UnidadAtencionDAO().ConsultaConceptos(CatalogoReques, null);
                    break;
                case (int) CatalogoEnum.TramiteUnidadAtencion:
                    informacionCatalogo.Atributos =
                        ReflexionHelper.ObtenerElementosLinq(new TramiteUnidadAtencionDTO(), MemberTypes.Property);
                    informacionCatalogo.Resultado =
                        await new TramiteUnidadAtencionDAO().ConsultaConceptos(CatalogoReques, null);
                    break;
                case (int) CatalogoEnum.UnidadMedica:
                    informacionCatalogo.Atributos =
                        ReflexionHelper.ObtenerElementosLinq(new UnidadMedicaDTO(), MemberTypes.Property);
                    informacionCatalogo.Resultado = await new UnidadMedicaDAO().ConsultaConceptos(CatalogoReques, null);
                    break;
            }

            return informacionCatalogo;
        }


        public async Task<int> ActualizarCatalogo(CatalogoDTO CatalogoRequest)
        {
            var resultado = 0;
            switch (CatalogoRequest.CatalogoId)
            {
                case (int) CatalogoEnum.TipoTramite:
                    var tramite = JsonConvert.DeserializeObject<CatTipoTramiteDTO>(CatalogoRequest.JsonObject);
                    resultado = await new CatTipoTramiteDAO().AgregarOActualizarCatTipoTramite(tramite);
                    break;
                case (int) CatalogoEnum.EstadoCita:
                    var estadoCita = JsonConvert.DeserializeObject<CatTipoEdoCitaDTO>(CatalogoRequest.JsonObject);
                    resultado = await new CatTipoEdoCitaDAO().AgregarOActualizarCatTipoEdoCita(estadoCita);
                    break;
                case (int) CatalogoEnum.UnidadAtencion:
                    var unidad = JsonConvert.DeserializeObject<UnidadAtencionDTO>(CatalogoRequest.JsonObject);
                    resultado = await new UnidadAtencionDAO().AgregarOActualizar(unidad);
                    break;
                case (int) CatalogoEnum.TramiteUnidadAtencion:
                    var tramiteUnidad =
                        JsonConvert.DeserializeObject<TramiteUnidadAtencionDTO>(CatalogoRequest.JsonObject);
                    resultado =
                        await new TramiteUnidadAtencionDAO().AgregarOActualizarTramiteUnidadAtencion(tramiteUnidad);
                    break;
                case (int) CatalogoEnum.UnidadMedica:
                    var unidadMedica = JsonConvert.DeserializeObject<UnidadMedicaDTO>(CatalogoRequest.JsonObject);
                    resultado = await new UnidadMedicaDAO().AgregarOActualizarUnidadMedica(unidadMedica);
                    break;
            }

            return resultado;
        }
    }
}