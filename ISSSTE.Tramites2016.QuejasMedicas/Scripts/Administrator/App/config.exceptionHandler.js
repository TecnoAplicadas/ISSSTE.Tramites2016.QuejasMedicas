(function() {
    "use strict";

    var logSource = "exceptionHandler";

    var app = angular.module(appName);

    //crea un manejador de excepciones para todos los errores que angular emite para ser controlados por la aplicación
    app.config([
        "$provide", function($provide) {
            $provide.decorator("$exceptionHandler", ["$delegate", "appConfig", "logger", extendExceptionHandler]);
        }
    ]);

    //extiende el manejador de excepciones de angular
    function extendExceptionHandler($delegate, appConfig, logger) {
        var appErrorPrefix = appConfig.appErrorPrefix;

        return function(exception, cause) {
            $delegate(exception, cause);
            if (appErrorPrefix && exception.message.indexOf(appErrorPrefix) === 0) {
                return;
            }

            var errorData = { exception: exception, cause: cause };
            var message = appErrorPrefix + exception.message;
            //logueo del error usando custom logger
            logger.logError(message, errorData, logSource);
        };
    }

})();