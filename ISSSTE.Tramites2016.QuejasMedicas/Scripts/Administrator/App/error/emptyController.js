(function() {
    "use strict";

    var controllerId = "emptyController";
    angular
        .module(appName)
        .controller(controllerId, ["common", emptyController]);


    function emptyController(common) {
        //#region Controller Members

        var vm = this;

        vm.init = init;

        //#endregion

        //#region Functions

        function init(overrideMenu) {
            if (overrideMenu)
                common.overrideNavigationMenu(true);

            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        //#endregion
    }
})();