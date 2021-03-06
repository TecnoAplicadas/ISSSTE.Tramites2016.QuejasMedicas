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
    
    public partial class Solicitud
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Solicitud()
        {
            this.Involucrado = new HashSet<Involucrado>();
        }
    
        public int SolicitudId { get; set; }
        public bool EsActivo { get; set; }
        public int TramiteUnidadAtencionId { get; set; }
        public string NumeroFolio { get; set; }
        public int CatTipoEdoCitaId { get; set; }
        public int HorarioId { get; set; }
        public System.DateTime FechaCita { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int UnidadMedicaId { get; set; }
    
        public virtual CatTipoEdoCita CatTipoEdoCita { get; set; }
        public virtual Horario Horario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Involucrado> Involucrado { get; set; }
        public virtual TramiteUnidadAtencion TramiteUnidadAtencion { get; set; }
        public virtual UnidadMedica UnidadMedica { get; set; }
    }
}
