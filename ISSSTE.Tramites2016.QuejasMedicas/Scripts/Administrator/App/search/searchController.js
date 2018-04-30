(function() {
    "use strict";

    var controllerId = "searchController";

    angular
        .module(appName)
        .controller(controllerId, ["$routeParams","common", "searchDataService", "webApiService", "localStorageService", searchController]);


    function searchController($routeParams,common, searchDataService, webApiService, localStorageService) {

        var vm = this;

        vm.delegationId = null;
        vm.RequestTypeId = null;
        vm.query = "";
        var d = new Date();
        vm.startDate = d.toLocaleDateString("en-GB");
        vm.endDate = d.toLocaleDateString("en-GB");
        vm.dates = vm.startDate + "-" + vm.endDate;
        vm.selectedStatus = null;

        vm.statusList = [];
        vm.requests = [];
        vm.requestTypes = [];
        vm.delegations = [];

        vm.init = init;
        vm.initSearch = initSearch;
        vm.searchRequests = searchRequests;
        vm.selectStatus = selectStatus;
        vm.clearAllFilters = clearAllFilters;

        //Paginacion
        vm.pagination = new paginationService();
        vm.pagination.callBackFunction = vm.searchRequests;
        vm.MuestraToolTip = MuestraToolTip;
        vm.OcultaToolTip = OcultaToolTip;


        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function MuestraToolTip(mensaje)
        {
            tooltip.show(mensaje);
        }

        function OcultaToolTip() {
            tooltip.hide();
        }

        function clearAllFilters()
        {
            if (vm.delegations.length >1) {
                vm.delegationId = null;
            }         
            vm.RequestTypeId = null;
            vm.startDate = null;
            vm.endDate = null;
            setStatusValues(0, "Todos los estatus");
            vm.selectedStatus = null;            
            vm.query = "";
        }

        function getRequestFilters() {

            vm.delegationId = localStorageService.get("delegationId");
            vm.RequestTypeId=localStorageService.get("RequestTypeId");
            vm.startDate = localStorageService.get("startDate");
            vm.endDate = localStorageService.get("endDate");
            vm.selectedStatus = localStorageService.get("selectedStatus");
            vm.query = localStorageService.get("querySearch");
        }

        function setRequestFilters() {

            localStorageService.set("delegationId", vm.delegationId);
            localStorageService.set("RequestTypeId", vm.RequestTypeId);
            localStorageService.set("startDate", vm.startDate);
            localStorageService.set("endDate", vm.endDate);
            localStorageService.set("selectedStatus", vm.selectedStatus);
            localStorageService.set("querySearch", vm.query);

            return localStorageService;
        }

        function initSearch() {
            common.displayLoadingScreen();

            var preserveFilters = localStorageService.get("preserveFilters");
            
            if (preserveFilters == true)
            {
                getRequestFilters();
                searchRequests();
                setStatusValues(vm.selectedStatus != null ? vm.selectedStatus.EstadoCitaId : 0, vm.selectedStatus != null ? vm.selectedStatus.Concepto : "");
                localStorageService.set("preserveFilters",false);
            }

            var initPromises = [];

            initPromises.push(getStatusList());
            initPromises.push(getRequests());
            initPromises.push(getRequestTypes());
            initPromises.push(getDelegations());

            common.$q.all(initPromises)
                .finally(function() {
                    init();
                    UI.initStatusDropdown();
                    common.hideLoadingScreen();
                });
        }

        function getDelegations() {
            return webApiService.makeRetryRequest(1,
                function() {
                    return searchDataService.getUserDelegations();
                }).then(function(data) {
                vm.delegations = data;
                if (vm.delegations.length == 1) {
                    vm.delegationId = vm.delegations[0].DelegacionId;
                }
            }).catch(function(reason) {
                common.showErrorMessage(reason, Messages.error.errorGetDelegations);
            });
        }

        function getRequestTypes() {

            return webApiService.makeRetryRequest(1,
                    function() {
                        return searchDataService.getRequestTypes();
                    })
                .then(function(data) {
                    vm.requestTypes = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetRequestTypes);
                });
        }

        function selectStatus(status) {
            vm.selectedStatus = status;
            //vm.searchRequests();
        }



        function isValidFilters()
        {

            if ((vm.delegationId == null || vm.delegations.length==1) &&
                vm.RequestTypeId == null &&
                vm.selectedStatus == null &&
                vm.query == "" &&
                (vm.startDate == null ||
                vm.endDate == null)) {
                common.showErrorMessage(Messages.validation.filtersRequiered);
                return false;
            }

            if (!(isValidDate(vm.startDate) && isValidDate(vm.endDate)) && !(vm.startDate==null || vm.endDate==null))
            {
                common.showErrorMessage(Messages.validation.invalidDate);
                return false;
            }

            return true;

        }

       

        function searchRequests() {

            if (!isValidFilters()) { return; }

            vm.dates = vm.startDate + "-" + vm.endDate;
            if (vm.dates == null || vm.dates == "") return;

            common.displayLoadingScreen();

            getRequests()
                .finally((function() {
                    common.hideLoadingScreen();
                }));
        }

        function getRequests() {

            return webApiService.makeRetryRequest(1,
                    function() {
                        return searchDataService.getRequests(vm.pagination.selectedPageSize,
                            vm.pagination.selectedPage,
                            vm.query,
                            vm.dates,
                            vm.selectedStatus != null ? vm.selectedStatus.EstadoCitaId : null,
                            vm.delegationId,
                            vm.RequestTypeId);

                    })
                .then(function(data) {
                    vm.pagination.selectedPage = data.CurrentPage;
                    vm.pagination.totalPages = data.TotalPages;
                    vm.pagination.resultCount = data.ResultCount;

                    vm.pagination.pages =
                        vm.pagination.paginationHelper.getPages(vm.pagination.selectedPage, vm.pagination.totalPages);

                    vm.requests = data.ResultList;

                    setRequestFilters();

                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.requests);
               });
        }

        function getStatusList() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return searchDataService.getStatusList();
                    })
                .then(function(data) {
                    vm.statusList = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.statusList);
                });
        }


    }
})();