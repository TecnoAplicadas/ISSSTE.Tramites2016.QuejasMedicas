//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISSSTE.Tramites2016.QuejasMedicas.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class SysAcceso
    {
        public int SysAccesoId { get; set; }
        public Nullable<int> CatSysRolId { get; set; }
        public int CatSysModuloId { get; set; }
        public bool EsActivo { get; set; }
    
        public virtual CatSysModulo CatSysModulo { get; set; }
        public virtual CatSysRol CatSysRol { get; set; }
    }
}