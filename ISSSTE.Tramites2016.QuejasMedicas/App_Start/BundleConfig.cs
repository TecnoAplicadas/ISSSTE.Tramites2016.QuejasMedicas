using ISSSTE.Tramites2016.Common.Web;
using System.Web.Optimization;

namespace ISSSTE.Tramites2016.QuejasMedicas
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection Bundles)
        {
            #region Librerias

            //->jquery
            Bundles.Add(new ScriptBundle("~/bundles/JQuery")
                .IncludeDirectory("~/Scripts/Libraries/JQuery", "*.js", false)
            );

            //->ramda
            Bundles.Add(new ScriptBundle("~/bundles/ramda")
                .Include(
                    "~/Scripts/Libraries/ramda/ramda.js"
                )
            );

            //->angular
            Bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/Libraries/Angular/angular.js",
                "~/Scripts/Libraries/Angular/angular-resource.js",
                "~/Scripts/Libraries/Angular/angular-route.js",
                "~/Scripts/Libraries/Angular/angular-sanitize.js",
                "~/Scripts/Libraries/Angular/angular-local-storage.js",
                "~/Scripts/Libraries/Angular/angular-local-storage.js"
            ));

            //->angular-auto-validate
            Bundles.Add(new ScriptBundle("~/bundles/angular-auto-validate").Include(
                "~/Scripts/Libraries/AngularAutoValidate/jcs-auto-validate.js"
            ));

            //->bootstrap-calendar
            Bundles.Add(new ScriptBundle("~/bundles/bootstrap-calendar").Include(
                "~/Scripts/Libraries/Underscore/underscore-min.js",
                "~/Scripts/Libraries/BootstrapCalendar/js/language/es-MX.js",
                "~/Scripts/Libraries/BootstrapCalendar/js/calendar.min.js"
            ));

            //->angular-upload
            Bundles.Add(new ScriptBundle("~/bundles/angular-upload").Include(
                "~/Scripts/Libraries/AngularUpload/startup.js",
                "~/Scripts/Libraries/AngularUpload/ng-file-upload-all.js"
            ));

            //->bootstrap-datetimepicker
            Bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker")
                .Include(
                    "~/Scripts/Libraries/BootstrapDatetimePicker/moment-with-locales.js",
                    "~/Scripts/Libraries/BootstrapDatetimePicker/bootstrap-datetimepicker.js",
                    "~/Scripts/Libraries/BootstrapDatetimePicker/angular-bootstrap-datetimepicker-directive.js",
                    "~/Scripts/Libraries/BootstrapDatetimePicker/daterangepicker.js"
                ));


            //->modernizr
            Bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .IncludeDirectory("~/Scripts/Libraries/Modernizr", "*.js", false)
            );

            //->respond
            Bundles.Add(new ScriptBundle("~/bundles/Respond")
                .IncludeDirectory("~/Scripts/Libraries/Respond", "*.js", false)
            );

            //->utils
            Bundles.Add(new ScriptBundle("~/bundles/PaginacionAngular")
                .IncludeDirectory("~/Scripts/Libraries/PaginacionAngular", "*.js", false)
            );


            //->easyTree
            Bundles.Add(new ScriptBundle("~/bundles/easy-tree").Include(
                "~/Scripts/Libraries/EasyTree/jsEasyTree.js"
            ));

            //->   bootbox
            Bundles.Add(new ScriptBundle("~/bundles/bootbox").Include(
                "~/Scripts/Libraries/Bootboxjs/bootbox.min.js"
            ));

            #endregion Librerias

            #region EstilosAdministrador

            Bundles.Add(new StyleBundle("~/bundles/administrator/css")
                .IncludeWithCssRewriteTransform(
                    "~/Scripts/Libraries/Bootstrap/css/bootstrap.css",
                    "~/Content/Administrator/general.css",
                    "~/Content/Entitle/bootstrap-datetimepicker.css",
                    "~/Content/Administrator/menu.css",
                    "~/Content/Administrator/menu-lateral.css",
                    "~/Content/Administrator/buscador.css",
                    "~/Content/Administrator/site.css",
                    "~/Content/Administrator/daterangepicker.css",
                    "~/Scripts/Libraries/BootstrapCalendar/css/calendar.min.css",
                    "~/Content/Administrator/ngduallist.css"
                )
            );

            #endregion EstilosAdministrador

            #region Scripts

            Bundles.Add(new ScriptBundle("~/bundles/administrator/scripts-utils")
                .Include(
                    "~/Scripts/Administrator/UI/scripts.js"
                    //"~/Scripts/Administrator/UI/agregar-tabla.js"
                )
            );


            Bundles.Add(new ScriptBundle("~/bundles/administrator/app")
                .IncludeDirectory("~/Scripts/Administrator/App/resources", "*.js", true)
                .Include(
                    "~/Scripts/Administrator/App/app.js",
                    "~/Scripts/Administrator/App/config.js",
                    "~/Scripts/Administrator/App/config.exceptionHandler.js",
                    "~/Scripts/Administrator/App/config.routes.js"
                )
                .IncludeDirectory("~/Scripts/Administrator/App/dualList", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/common", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/error", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/search", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/requestsDetail", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/appointments", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/calendar", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/catalogs", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/requirements", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/settings", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/reports", "*.js", true)
            );

            Bundles.Add(new ScriptBundle("~/bundles/PaginacionAngular")
                .IncludeDirectory("~/Scripts/Libraries/PaginacionAngular", "*.js", false)
            );

            Bundles.Add(new ScriptBundle("~/bundles/administrator/app/login")
                .IncludeDirectory("~/Scripts/Administrator/App/resources", "*.js", true)
                .Include(
                    "~/Scripts/Administrator/App/app.js",
                    "~/Scripts/Administrator/App/config.js",
                    "~/Scripts/Administrator/App/config.exceptionHandler.js",
                    "~/Scripts/Administrator/App/config.routes.js"
                )
                .IncludeDirectory("~/Scripts/Administrator/App/common", "*.js", true)
                .IncludeDirectory("~/Scripts/Administrator/App/login", "*.js", true)
            );

            Bundles.Add(new ScriptBundle("~/bundles/baseJS").Include(
                "~/Scripts/Libraries/JQuery/jquery-2.1.4.js",
                "~/Scripts/Libraries/JQuery/jquery-ui.js",
                "~/Scripts/Libraries/Bootstrap/js/bootstrap.min.js"
            ));

            Bundles.Add(new ScriptBundle("~/bundles/baseJSAll")
                .IncludeDirectory("~/Scripts/BaseJS", "*.js", true)
            );

            Bundles.Add(new ScriptBundle("~/bundles/cross-list")
                .Include(
                    "~/Scripts/Libraries/CrossList/jsCrossList.js")
            );

            Bundles.Add(new ScriptBundle("~/bundles/dual-list-angular")
                .Include(
                    "~/Scripts/Libraries/DualListAngular/ngduallist.js")
            );

            Bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include(
                    "~/Scripts/Libraries/Bootstrap/js/bootstrap.js")
            );

            #endregion Scripts


            //Se deshabilita el bundle, pues cuando se minifica se tiene un problema con las rutas de las fuentes de bootstrap
            BundleTable.EnableOptimizations = false;
        }
    }
}