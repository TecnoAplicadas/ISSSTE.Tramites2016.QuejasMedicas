using System;
using System.Web.Optimization;

namespace ISSSTE.Tramites2016.Common.Web
{
    public static class Extensions
    {
        /// <summary>
        ///     Includes the specified <paramref name="VirtualPaths" /> within the bundle and attached the
        ///     <see cref="System.Web.Optimization.CssRewriteUrlTransform" /> item transformer to each item
        ///     automatically.
        /// </summary>
        /// <param name="Bundle">The bundle.</param>
        /// <param name="VirtualPaths">The virtual paths.</param>
        /// <returns>Bundle.</returns>
        /// <exception cref="System.ArgumentException">Only available to StyleBundle;bundle</exception>
        /// <exception cref="System.ArgumentNullException">virtualPaths;Cannot be null or empty</exception>
        public static Bundle IncludeWithCssRewriteTransform(this Bundle Bundle, params string[] VirtualPaths)
        {
            if (!(Bundle is StyleBundle))
                throw new ArgumentException("Only available to StyleBundle", "Bundle");
            if (VirtualPaths == null || VirtualPaths.Length == 0)
                throw new ArgumentNullException("VirtualPaths", "Cannot be null or empty");
            IItemTransform itemTransform = new CssRewriteUrlTransform();
            foreach (var virtualPath in VirtualPaths)
                if (!string.IsNullOrWhiteSpace(virtualPath))
                    Bundle.Include(virtualPath, itemTransform);
            return Bundle;
        }
    }
}