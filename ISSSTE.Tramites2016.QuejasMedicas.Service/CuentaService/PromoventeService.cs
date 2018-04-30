using ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService
{
    public class PromoventeService : DoDisposeDTO
    {
        /// <summary>
        /// Listado de trámites
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <param name="EsPrincipal"></param>
        /// <returns></returns>
        public PromoventeDTO ConsultaListado(int CatTipoTramiteId, bool EsPrincipal)
        {
            var promoventeDao = new PromoventeDAO();
            return promoventeDao.ConsultaListado(CatTipoTramiteId,EsPrincipal);
        }

        /// <summary>
        /// Consulta de un tramite
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        public string ConceptoTipoTramite(int CatTipoTramiteId)
        {
            var promoventeDao = new PromoventeDAO();
            return promoventeDao.ConceptoTipoTramite(CatTipoTramiteId);
        }

        /// <summary>
        /// Existencia de un trámite
        /// </summary>
        /// <param name="CatTipoTramiteId"></param>
        /// <returns></returns>
        public bool SeEncuentraTramite(int CatTipoTramiteId)
        {
            var promoventeDao = new PromoventeDAO();
            return promoventeDao.SeEncuentraTramite(CatTipoTramiteId);
        }
    }
}