(function() {
    "use strict";

    var controllerId = "workingonit";
    angular.module(appName)
        .controller(controllerId, ["$rootScope", "common", "appConfig", workingonit]);

    function workingonit($rootScope, common, appConfig) {
        //#region Members

        var vm = this;

        vm.isWorking = true;

        //#endregion

        //#Initialization

        //Se registra listener para quitar pantalla de "Trabajando en ello"
        //    se llama:
        //      common.$broadcast(commonConfig.config.workingOnItToggleEvent, {show: false});
        $rootScope.$on(appConfig.events.workingOnItToggle,
            function(event, data) {
                common.logger.log(Messages.info.workingOnItToggle, data, controllerId);
                vm.isWorking = data.show;
            });

        //Se registra a evento de cambio de ruta, para mostrar la pantalla de "Trabajando en ello"
        $rootScope.$on("$routeChangeStart",
            function(event, next, current) {
                common.logger.log("$routeChangeStart", event, controllerId);
                vm.isWorking = true;
            });

        //#endregion
    }
})();