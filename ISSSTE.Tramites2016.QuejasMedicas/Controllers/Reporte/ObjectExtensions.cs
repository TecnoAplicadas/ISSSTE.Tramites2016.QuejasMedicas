using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ISSSTE.Tramites2016.QuejasMedicas.Controllers.Reporte
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, object> AsDictionary(this object Source,
            BindingFlags BindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance |
                                       BindingFlags.ExactBinding)
        {
            return Source.GetType()
                .GetProperties(BindingAttr)
                .ToDictionary
                (
                    PropInfo => PropInfo.Name,
                    PropInfo => PropInfo.GetValue(Source, null)
                );
        }
    }
}