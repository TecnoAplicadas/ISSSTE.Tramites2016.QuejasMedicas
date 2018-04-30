using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.AdministracionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.AdministracionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.AdministracionService
{
    public class CatTipoEntidadService
    {
        public List<CatTipoEntidadDTO> ConsultaConceptos(bool? EsActivo)
        {
            var catTipoEntidadDao = new CatTipoEntidadDAO();
            return catTipoEntidadDao.ConsultaConceptosCatTipoEntidad(EsActivo);
        }
    }
}