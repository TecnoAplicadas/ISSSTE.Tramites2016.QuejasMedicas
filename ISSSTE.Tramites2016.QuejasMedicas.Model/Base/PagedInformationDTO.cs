using System;
using System.Collections.Generic;

namespace ISSSTE.Tramites2016.QuejasMedicas.Model.Base
{
    /// <summary>
    ///     MFP 04/01/2017 Clase Generica para manejar la paginacion.
    /// </summary>
    public class PagedInformationDTO<T> : BasePageInformationDTO where T : class
    {
        public List<T> ResultList { get; set; }

        public int TotalPages // <TotalPages> Necesario del lado del cliente para la paginacion
            => PageSize > 0 ? (int) Math.Ceiling((decimal) ResultCount / PageSize) : 1;

        public int SetElementosPorPagina
        {
            set { PageSize = value; }
        }

        public int GetElementosPorPagina => PageSize;

        public string GetFiltroBusqueda => string.IsNullOrEmpty(QueryString) || string.IsNullOrWhiteSpace(QueryString)
            ? null
            : QueryString;

        public int GetCurrentPage => CurrentPage < TotalPages ? CurrentPage : TotalPages > 0 ? TotalPages : 1;
    }
}