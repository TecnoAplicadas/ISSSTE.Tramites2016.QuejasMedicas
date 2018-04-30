(function() {
    "use strict";

    var controllerId = "reportsController";

    angular
        .module(appName)
        .controller(controllerId, ["common", "reportsDataService", "webApiService", reportsController]);


    function reportsController(common, reportsDataService, webApiService) {

        var vm = this;

        vm.initSearch = initSearch;
        //Paginacion
        vm.pagination = new paginationService();
        vm.pagination.callBackFunction = vm.searchRequests;


        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initSearch() {

            common.displayLoadingScreen();

            var initPromises = [];
            //initPromises.push(getStatusList());
            //initPromises.push(getRequests());
            //initPromises.push(getRequestTypes());
            //initPromises.push(getDelegations());

            common.$q.all(initPromises)
                .finally(function() {
                    init();
                    common.hideLoadingScreen();
                });

        }


    }
})();