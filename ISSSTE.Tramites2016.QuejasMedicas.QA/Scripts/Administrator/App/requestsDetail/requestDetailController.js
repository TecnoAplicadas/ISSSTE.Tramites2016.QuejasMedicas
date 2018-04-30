(function () {
    "use strict";

    var controllerId = "requestDetailController";
    angular
        .module(appName)
        .controller(controllerId,
            [
                "$routeParams", "$location", "common", "requestsDataService", "webApiService",
                requestDetailController
            ]);


    function requestDetailController($routeParams, $location, common, requestsDataService, webApiService) {

        var vm = this;

        //vm.requestInfo = null;
        vm.nextStatusId = null;
        vm.init = init;
        vm.information = {};
        vm.statusList = {};

        vm.initRequest = initRequest;
        vm.getRequestEntitleInformation = getRequestEntitleInformation;
        vm.unlockUserByRequestId = unlockUserByRequestId;
        vm.saveStatusAppointment = saveStatusAppointment;
        vm.ImprimirCitaPDF = ImprimirCitaPDF;


        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }


        function ImprimirCitaPDF(NumeroFolioParam) {

            ImprimirCitaGeneral(NumeroFolioParam);
        }

        function initRequest() {

            vm.information = {};
            var initPromises = [];

            var requestId = $routeParams[REQUEST_ID_PARAM];

            if (requestId) {
                initPromises.push(getRequestEntitleInformation(requestId));
            }

            common.$q.all(initPromises)
                .finally(function () {
                    init();
                });
        }


        function getRequestEntitleInformation(requestId) {
            return webApiService.makeRetryRequest(1,
                    function () {
                        return requestsDataService.getRequestEntitleInformation(requestId);
                    })
                .then(function (data) {
                    vm.information = data;
                    getNextStatusList();

                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.requestDetail);
                });
        }


        function getNextStatusList() {
            return webApiService.makeRetryRequest(1,
                    function () {
                        return requestsDataService.getNextStatusList();
                    })
                .then(function (data) {
                    vm.statusList = data;

                    if (vm.statusList.length > 0) {

                        vm.statusList.inArray = inArrayFindBy;
                        var result = vm.statusList.inArray(vm.information.Cita.CatTipoEdoCitaId, "EstadoCitaId");

                        if (result != -1)
                            vm.nextStatusId = vm.information.Cita.CatTipoEdoCitaId;

                    }

                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                });
        }

        function saveStatusAppointment(requestId) {
            return webApiService.makeRetryRequest(1,
                    function () {
                        return requestsDataService.saveStatusAppointment(requestId, vm.nextStatusId);
                    })
                .then(function (data) {
                    common.showSuccessMessage(Messages.success.itemUpdated);
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.addOrUpdateItem);
                });
        }

        function unlockUserByRequestId(requestId) {
            return webApiService.makeRetryRequest(1,
                    function () {
                        return requestsDataService.unlockUserByRequestId(requestId);
                    })
                .then(function (data) {
                    vm.information.Paciente.EsUsuarioBloqueado = false;
                    common.showSuccessMessage(Messages.success.unlockUser);
                })
                .catch(function (reason) {
                    common.showErrorMessage(reason, Messages.error.addOrUpdateItem);
                });
        }

    }
})
    ();