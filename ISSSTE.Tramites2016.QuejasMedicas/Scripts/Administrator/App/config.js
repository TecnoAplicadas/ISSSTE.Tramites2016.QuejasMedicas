(function() {
    "use strict";
    var app = angular.module(appName);

    //eventos que serán monitoreados en la aplicación
    var events = {
        controllerActivateSuccess: "controller.activateSuccess",
        workingOnItToggle: "workingonit.toggle",
        overrideMenu: "navigationService.overrideMenu",
        changeRequestId: "navigationService.changeRequestid"
    };

    //Objeto de configuraciones de la aplicación
    var config = {
        //Configuracion del decorador de excepción
        appErrorPrefix: "[ERR] ",
        //Eventos de la aplicación
        events: events,
        //Versión
        version: "1.0.0.0",
        //Configuración de notificación de depuración
        showDebugNotiSetting: false,
        //URL base del API REST
        //webApiBaseUrl: 'http://localhost:7461',
        //Llave para almacenar el token de autenticación
        authorizationDataLocalStorageKey: "ISSSTE.Tramites2016.QmRdRI.Administrator.App.AuthorizationData",
    };

    //crea una variable global en la aplicación llamada 'config'
    app.value("appConfig", config);

    //configura el servicio de logueo de angular antes del arranque
    app.config([
        "$logProvider", function($logProvider) {
            //activa/desactiva la depuración (no info/advertencias)
            if ($logProvider.debugEnabled) {
                $logProvider.debugEnabled(true);
            }
        }
    ]);

})();