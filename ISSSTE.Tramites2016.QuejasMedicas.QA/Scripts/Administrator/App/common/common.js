(function() {
    "use strict";

    var commonModule = angular.module("common", []);

    commonModule.provider("commonConfig",
        function() {
            this.config = {
                userRoles: [],
                userMenu: []
            };

            this.$get = function() {
                return {
                    config: this.config
                };
            };
        });

    commonModule.factory("common", ["$q", "$rootScope", "$timeout", "commonConfig", "appConfig", "logger", common]);

    function common($q, $rootScope, $timeout, commonConfig, appConfig, logger) {
        var service = {
            //Dependencias de Angular
            $broadcast: $broadcast,
            $q: $q,
            $timeout: $timeout,
            //Servicios del proyecto
            logger: logger,
            activateController: activateController,
            config: commonConfig.config,

            //Funciones para validación del usuario
            doesUserHasNecessaryRoles: doesUserHasNecessaryRoles,

            //Funciones para mostrar pantalla de carga
            displayLoadingScreen: displayLoadingScreen,
            hideLoadingScreen: hideLoadingScreen,

            //Funciones para la navegación
            changeNavigationRequestId: changeNavigationRequestId,
            overrideNavigationMenu: overrideNavigationMenu,

            //Funciones para mostrar notificaciones
            showInfoMessage: showInfoMessage,
            showErrorMessage: showErrorMessage,
            showWarningMessage: showWarningMessage,
            showSuccessMessage: showSuccessMessage,

            //Funciones de configuración
            getBaseUrl: getBaseUrl
        };

        return service;

        //Se llama al servicio $broadcats de angular
        function $broadcast() {
            return $rootScope.$broadcast.apply($rootScope, arguments);
        }

        //Función utilizada cuando un controller termino su carga
        function activateController(promises, controllerId) {
            return $q.all(promises).then(function(eventArgs) {
                var data = { controllerId: controllerId };
                $broadcast(appConfig.events.controllerActivateSuccess, data);
                // hide the workingOnIt animation
                $broadcast(appConfig.events.workingOnItToggle, { show: false });
            });
        }

        function doesUserHasNecessaryRoles(necessaryRoles) {
            var result = false;

            if (commonConfig.config.userRoles) {
                var commonPermissionRoles = R.intersection(commonConfig.config.userRoles, necessaryRoles);

                result = commonPermissionRoles.length > 0;
            }

            return result;
        }

        function displayLoadingScreen() {
            $broadcast(appConfig.events.workingOnItToggle, { show: true });
        }

        function hideLoadingScreen() {
            $broadcast(appConfig.events.workingOnItToggle, { show: false });
        }

        function changeNavigationRequestId(requestId) {
            $broadcast(appConfig.events.changeRequestId, { requestId: requestId });
        }

        function overrideNavigationMenu(override) {
            $broadcast(appConfig.events.navigationMenuOverrided, { override: override });
        }

        function showInfoMessage(message, title) {
            UI.createInfoMessage(message, title);
        }

        function showErrorMessage(message, title) {
            UI.createErrorMessage(message, title);
        }

        function showWarningMessage(message, title) {
            UI.createWarningMessage(message, title);
        }

        function showSuccessMessage(message, title) {
            UI.createSuccessMessage(message, title);
        }

        function getBaseUrl() {
            return UI.getBaseUrl();
        }


    }
})();