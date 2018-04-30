using System.Collections.Generic;
using ISSSTE.Tramites2016.QuejasMedicas.DAL.CuentaDAO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.CuentaDTO;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Domain.CuentaService
{
    public class UsuarioService : DoDisposeDTO
    {
        public MenuDTO ConstruyeMenuUsuario(List<string> Roles)
        {
            return new MenuDAO().ConstruyeListaMenuLj(Roles);
        }
    }
}