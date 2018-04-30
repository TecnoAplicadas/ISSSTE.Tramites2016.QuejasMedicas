using ISSSTE.Tramites2016.QuejasMedicas.DAL.InformacionDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.InformacionService
{
    public class GenericoService
    {
        public string DeleteReactive(DuplaValoresDTO DuplaValores)
        {
            var genericoDao = new GenericoDAO();
            return genericoDao.DeleteReactive(DuplaValores);
        }

        public string PalabraCifrada(string Palabra)
        {
            var genericoDao = new GenericoDAO();
            return genericoDao.PalabraCifrada(Palabra);
        }
    }
}