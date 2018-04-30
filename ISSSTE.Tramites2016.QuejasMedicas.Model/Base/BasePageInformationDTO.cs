namespace ISSSTE.Tramites2016.QuejasMedicas.Model.Base
{
    public class BasePageInformationDTO
    {
        public string QueryString { get; set; }

        public int
            ResultCount { get; set; } // <Total de elementos> (sin considerar los filtros de pagnacion Skip y Take.

        public int
            PageSize
        {
            get;
            set;
        } // <Tamaño de la pagina o elementos por pagina - PageSize > Necesario del lado del cliente para la paginacion

        public int CurrentPage { get; set; } // <CurrentPage >  Necesario del lado del cliente para la paginacion
    }
}