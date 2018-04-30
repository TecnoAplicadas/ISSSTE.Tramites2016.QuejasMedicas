(function() {
    //'use strict';

    var controllerId = "requirementsController";
    angular
        .module(appName)
        .controller(controllerId, ["common", "requirementsDataService", "webApiService", requirementsController]);

    function requirementsController(common, requirementsDataService, webApiService) {
        var vm = this;

        vm.query = "";
        vm.RequestTypeId = null; // Id de tramite
        vm.requestTypes = [];
        vm.requirements = [];
        vm.requirementDetail = {};

        vm.init = init;
        vm.initRequirements = initRequirements;
        vm.searchRequirements = searchRequirements;
        vm.getRequirementsDetailClick = getRequirementsDetailClick;
        vm.saveRequirementsDetailClick = saveRequirementsDetailClick;
        vm.newRequirementsDetailClick = newRequirementsDetailClick;
        vm.deleteRequirementsDetailClick = deleteRequirementsDetailClick;
        vm.activateRequirementsDetailClick = activateRequirementsDetailClick;

        vm.pagination = new paginationService();
        vm.pagination.callBackFunction = vm.searchRequirements;


        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initRequirements() {

            common.displayLoadingScreen();

            var promises = [];
            promises.push(getRequirements());
            promises.push(getRequestTypes());

            common.$q.all(promises)
                .finally(function() {
                    init();
                    common.hideLoadingScreen();
                });
        }

        function initFormValidation(form) {

            if (form == null || form == undefined) return;

            form.$setPristine();
            form.$setUntouched();
        }

        function searchRequirements() {
            common.displayLoadingScreen();

            getRequirements()
                .finally((function() {
                    common.hideLoadingScreen();
                }));
        }


        function getRequirementsDetailClick(requirementDetail, form) {

            initFormValidation(form);

            vm.requirementDetail.RequisitoId = requirementDetail.RequisitoId;
            vm.requirementDetail.CatTipoTramiteId = requirementDetail.CatTipoTramiteId;
            vm.requirementDetail.NombreDocumento = requirementDetail.NombreDocumento;
            vm.requirementDetail.Descripcion = requirementDetail.Descripcion;
            vm.requirementDetail.EsObligatorio = requirementDetail.EsObligatorio;
            vm.requirementDetail.EsActivo = requirementDetail.EsActivo;

            if (vm.requirementDetail != null || vm.requirementDetail != undefined)
                $("#requirementsDetail-modal").modal("show");
        }

        function deleteRequirementsDetailClick(requirementDetail) {
            vm.requirementDetail = requirementDetail;
            vm.requirementDetail.EsActivo = false;

            saveRequirementsDetailClick(vm.requirementDetail);
        }

        function activateRequirementsDetailClick(requirementDetail) {
            vm.requirementDetail = requirementDetail;
            vm.requirementDetail.EsActivo = true;

            saveRequirementsDetailClick(vm.requirementDetail);
        }

        function newRequirementsDetailClick(form) {

            initFormValidation(form);

            vm.requirementDetail = {};
            vm.requirementDetail.EsActivo = true;
            $("#requirementsDetail-modal").modal("show");
        }


        function getRequirements() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return requirementsDataService.getRequirements(vm.pagination.selectedPageSize,
                            vm.pagination.selectedPage,
                            vm.query,
                            vm.RequestTypeId);
                    })
                .then(function(data) {
                    vm.pagination.selectedPage = data.CurrentPage;
                    vm.pagination.totalPages = data.TotalPages;
                    vm.pagination.resultCount = data.ResultCount;

                    vm.pagination.pages =
                        vm.pagination.paginationHelper.getPages(vm.pagination.selectedPage, vm.pagination.totalPages);

                    vm.requirements = data.ResultList;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetRequirements);
                });
        }


        function saveRequirementsDetailClick(settingDetail) {

            common.displayLoadingScreen();

            return webApiService.makeRetryRequest(1,
                    function() {
                        return requirementsDataService.saveRequirementDetail(settingDetail);
                    })
                .then(function(data) {
                    $("#requirementsDetail-modal").modal("hide");
                    searchRequirements();
                    common.showSuccessMessage(Messages.success.itemUpdated);
                    common.hideLoadingScreen();
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.addOrUpdateItem);
                    common.hideLoadingScreen();
                });
        }


        function getRequestTypes() {

            return webApiService.makeRetryRequest(1,
                    function() {
                        return requirementsDataService.getRequestTypes();
                    })
                .then(function(data) {
                    vm.requestTypes = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetRequestTypes);
                });
        }


    }
})();