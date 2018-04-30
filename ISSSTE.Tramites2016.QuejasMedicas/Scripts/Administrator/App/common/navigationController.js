(function() {
    "use strict";

    var controllerId = "navigationController";

    angular
        .module(appName)
        .controller(controllerId,
            [
                "$rootScope", "$routeParams", "$location", "common", "authenticationService", "webApiService",
                "appConfig","localStorageService", navigationController
            ]);

    function navigationController($rootScope,
        $routeParams,
        $location,
        common,
        authenticationService,
        webApiService,
        appConfig,
        localStorageService
        ) {

        var REQUEST_ID_TEMPLATE_PARAM = ":" + REQUEST_ID_PARAM;
        var REQUEST_FILTERS_TEMPLATE_PARAM = ":" + REQUEST_FILTERS_PARAM;
        var MESSAGE_ID_TEMPLATE_PARAM = ":" + MESSAGE_ID_PARAM;

        var vm = this;

        //GLOBAL 

        vm.delegationId = null;
        vm.RequestTypeId = null;
        vm.startDate = null;
        vm.endDate =  null;
        vm.selectedStatus = null;

        //GLOBAL 

        vm.searchUrl = Routes.search.url;
        vm.requestDetailUrl = Routes.requestDetail.url;
        vm.calendarUrl = Routes.calendar.url;
        vm.getRequestDetailUrl = getRequestDetailUrl;
        vm.getSearchUrl = getSearchUrl;
        vm.navigateToRequestUrl = navigateToRequestUrl;
        vm.showNotificationsModal = showNotificationsModal;
        vm.hideNotificationsModal = hideNotificationsModal;
        vm.createUserMenu = createUserMenu;
        vm.initSearch = initSearch;
        vm.searchNotifications = searchNotifications;
        vm.delegations = [];
        vm.Notifications = [];

        $rootScope.$on("$routeChangeSuccess",
            function(event, current, previous) {
                var currentRequestUrl = current.originalPath;
            });

        function navigateToRequestUrl() {
            $location.path(Routes.search.url);
        }

        function getRequestDetailUrl(requestId) {
            return vm.requestDetailUrl.replace(REQUEST_ID_TEMPLATE_PARAM, requestId);
        }

        function getSearchUrl(preserveFilters) {
            localStorageService.set("preserveFilters", preserveFilters);
            return vm.searchUrl;
        }


        function hideNotificationsModal() {
            $("#notificaciones-modal").modal("hide");
        }

        function showNotificationsModal() {
            $("#notificaciones-modal").modal("show");
            searchNotifications();
        }

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }


        function searchNotifications() {
            common.displayLoadingScreen();

            getNotifications()
                .finally((function() {
                    common.hideLoadingScreen();
                }));
        }

        function initSearch() {

            common.displayLoadingScreen();

            var promises = [];
            promises.push(createUserMenu());
            promises.push(getNotifications());

            common.$q.all(promises)
                .finally(function() {
                    init();
                    common.hideLoadingScreen();
                });
        }

        function createUserMenu() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return authenticationService.getMenu();
                    })
                .then(function(Data) {

                    common.config.userMenu.push(Data);

                    var menu_nuevo = $("#demo_menu");

                    menu_nuevo.easytree({
                        addable: false,
                        editable: false,
                        deletable: false,
                        data: Data.menu,
                        clickNode: MenuHide,
                        baseUrl: common.getBaseUrl(),
                        allowActivate:true
                    });

                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, "Error al obtener el menu");
                });

        }

        function getNotifications() {
            return webApiService.makeRetryRequest(1,
                function() {
                    return authenticationService.getNotifications();
                }).then(function(data) {
                vm.Notifications = data;

            }).catch(function(reason) {
                common.showErrorMessage(reason, Messages.error.errorGetNotifications);
            });
        }

        function getDelegations() {
            return webApiService.makeRetryRequest(1,
                function() {
                    common.displayLoadingScreen();
                    return authenticationService.getUserDelegations();
                }).then(function(data) {
                vm.delegations = data;
                common.hideLoadingScreen();
            }).catch(function(reason) {
                common.showErrorMessage(reason, Messages.error.errorGetDelegations);
                common.hideLoadingScreen();
            });
        }


    }
})();