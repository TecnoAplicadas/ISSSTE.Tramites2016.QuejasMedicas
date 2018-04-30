using System.Web.Mvc;

namespace ISSSTE.Tramites2016.QuejasMedicas
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection Filters)
        {
            Filters.Add(new HandleErrorAttribute());
        }
    }
}