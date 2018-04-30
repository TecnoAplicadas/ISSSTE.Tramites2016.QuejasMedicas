using System;
using System.Collections.Generic;
using System.Linq;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO
{
    public class PromoventeDAO : DoDisposeDTO
    {
        /// <summary>
        ///     Secciones de consulta - Información general
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <param name="EsPrincipal"></param>
        /// <returns></returns>
        public PromoventeDTO ConsultaListado(int CatTipoTramiteId, bool EsPrincipal)
        {
            var promoventeDto = new PromoventeDTO();
            using (var modelo = new ISSSTEEntities())
            {
                if (EsPrincipal)
                {
                    promoventeDto.SeccionPrincipal = SeccionPrincipal(CatTipoTramiteId, modelo, true);
                    if (promoventeDto.SeccionPrincipal != null)
                    {
                        foreach (var t in promoventeDto.SeccionPrincipal)
                        {
                            t.Detalle = DetalleSeccion(modelo, t.SeccionId);
                        }
                    }
                }
                else
                {
                    promoventeDto.SeccionSecundaria = SeccionPrincipal(CatTipoTramiteId, modelo, false);
                    if (promoventeDto.SeccionSecundaria != null)
                    {
                        foreach (var t in promoventeDto.SeccionSecundaria)
                        {
                            t.Detalle = DetalleSeccion(modelo, t.SeccionId);
                        }
                    }
                }

                promoventeDto.Requisitos = RequisitosPorTipoTramiteId(modelo, CatTipoTramiteId);
            }
            return promoventeDto;
        }

        /// <summary>
        ///     Descripción asociado a un tipo de trámite
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        public string ConceptoTipoTramite(int CatTipoTramiteId)
        {
            using (var modelo = new ISSSTEEntities())
            {
                return (from a in modelo.CatTipoTramite
                    where a.EsActivo && a.CatTipoTramiteId == CatTipoTramiteId
                    select a.Concepto).FirstOrDefault();
            }
        }

        /// <summary>
        ///     Requisitos de un tipo de tramite
        /// </summary>
        /// <param name="Modelo"></param>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        private List<RequisitoDTO> RequisitosPorTipoTramiteId(ISSSTEEntities Modelo, int CatTipoTramiteId)
        {
            return (from a in Modelo.Requisito
                where a.EsActivo && a.CatTipoTramiteId == CatTipoTramiteId
                select new RequisitoDTO
                {
                    NombreDocumento = a.NombreDocumento,
                    Descripcion = a.Descripcion
                }).ToList();
        }


        /// <summary>
        ///     DetalleSeccion de una sección de información
        /// </summary>
        /// <param name="Modelo"></param>
        /// <param name="SeccionId"></param>
        /// <returns></returns>
        private string DetalleSeccion(ISSSTEEntities Modelo, int SeccionId)
        {
            return (from a in Modelo.SeccionDetalle
                where a.EsActivo && a.SeccionId == SeccionId
                select a.Contenido).FirstOrDefault();
        }

        /// <summary>
        ///     Listado de secciones de información
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <param name="Modelo"></param>
        /// <param name="EsPrimerNivel"></param>
        /// <returns></returns>
        private List<SeccionDTO> SeccionPrincipal(int CatTipoTramiteId, ISSSTEEntities Modelo, bool EsPrimerNivel)
        {
            return (from b in Modelo.Seccion
                join c in Modelo.CatTipoSeccion on b.CatTipoSeccionId equals c.CatTipoSeccionId
                where b.CatTipoTramiteId == CatTipoTramiteId
                      && b.EsActivo && c.EsActivo && b.EsPrimerNivel == EsPrimerNivel
                orderby b.Orden
                select new SeccionDTO
                {
                    Orden = b.Orden,
                    Titulo = c.Concepto,
                    Imagen = c.Imagen,
                    SeccionId = b.SeccionId
                }).ToList();
        }

        /// <summary>
        /// Validación de existencia de un trámite
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        public bool SeEncuentraTramite(int CatTipoTramiteId)
        {
	        try
	        {
		        using (var modelo = new ISSSTEEntities())
		        {
			        var existe = (from b in modelo.CatTipoTramite
				        where b.CatTipoTramiteId == CatTipoTramiteId
				        select b).Count();

			        //return (Existe > 0) ? true : false;
			        return existe > 0;
		        }
			}
	        catch (Exception e)
	        {
		        Console.WriteLine(e);
		        throw;
	        }
            
        }
    }
}