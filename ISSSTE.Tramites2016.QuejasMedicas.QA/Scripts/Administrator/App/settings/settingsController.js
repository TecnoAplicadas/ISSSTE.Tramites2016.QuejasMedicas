(function() {
    "use strict";

    var controllerId = "settingsController";
    angular
        .module(appName)
        .controller(controllerId, ["common", "settingsDataService", "webApiService", settingsController]);

    function settingsController(common, settingsDataService, webApiService) {
        var vm = this;

        vm.categorySelectedId = 1;
        vm.query = "";


        vm.categorys = [
            {
                id: 1,
                name: "Configuraciones"

            },
            {
                id: 2,
                name: "Lenguaje ciudadano"
            }
        ];


        vm.requestTypes = [];
        vm.settings = [];
        vm.init = init;
        vm.initSettings = initSettings;
        vm.searchSettings = searchSettings;
        vm.getSettingsDetailClick = getSettingsDetailClick;
        vm.saveSettingDetailClick = saveSettingDetailClick;
        vm.changeCategory = changeCategory;
        vm.changeSearchCategory = changeSearchCategory;
        vm.initFormValidation = initFormValidation;

        vm.pagination = new paginationService();
        vm.pagination.callBackFunction = vm.changeSearchCategory;
        vm.settingDetail = {};

        vm.getRegex = getRegex //;Dependencia con scripts.js


        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }

        function initSettings() {
            common.displayLoadingScreen();

            var promises = [];
            promises.push(getSettings());

            common.$q.all(promises)
                .finally(function() {
                    init();
                    common.hideLoadingScreen();
                });
        }

        function changeCategory() {
            vm.pagination.selectedPage = 1;
            changeSearchCategory();
        }

        function changeSearchCategory() {

            if (vm.categorySelectedId == 1)
                searchSettings();
            else
                searchMessagesApp();
        }

        function initFormValidation(form) {

            if (form == null || form == undefined) return;
            try {
                form.$setPristine();
                form.$setUntouched();
            } catch (ex) {
            }
        }

        function getSettingsDetailClick(settingDetailArg) {

            vm.settingDetail.CatalogoId = settingDetailArg.CatalogoId;
            vm.settingDetail.Concepto = settingDetailArg.Concepto;
            vm.settingDetail.Descripcion = settingDetailArg.Descripcion;
            vm.settingDetail.Valor = settingDetailArg.Valor;
            vm.settingDetail.TipoDato = settingDetailArg.TipoDato;

            if (vm.settingDetail != null || vm.settingDetail != undefined)
                $("#settingDetail-modal").modal("show");
        }

        function searchSettings() {
            common.displayLoadingScreen();

            getSettings()
                .finally((function() {
                    common.hideLoadingScreen();
                }));
        }

        function searchMessagesApp() {
            common.displayLoadingScreen();

            getMessagessApp()
                .finally((function() {
                    common.hideLoadingScreen();
                }));
        }

        function getMessagessApp() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return settingsDataService.getMessagessApp(vm.pagination.selectedPageSize,
                            vm.pagination.selectedPage,
                            vm.query);
                    })
                .then(function(data) {
                    vm.pagination.selectedPage = data.CurrentPage;
                    vm.pagination.totalPages = data.TotalPages;
                    vm.pagination.resultCount = data.ResultCount;


                    vm.pagination.pages =
                        vm.pagination.paginationHelper.getPages(vm.pagination.selectedPage, vm.pagination.totalPages);

                    vm.settings = data.ResultList;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetSettings);
                });
        }

        function getSettings() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return settingsDataService.getSettings(vm.pagination.selectedPageSize,
                            vm.pagination.selectedPage,
                            vm.query);
                    })
                .then(function(data) {
                    vm.pagination.selectedPage = data.CurrentPage;
                    vm.pagination.totalPages = data.TotalPages;
                    vm.pagination.resultCount = data.ResultCount;

                    vm.pagination.pages =
                        vm.pagination.paginationHelper.getPages(vm.pagination.selectedPage, vm.pagination.totalPages);

                    vm.settings = data.ResultList;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetSettings);
                });
        }


        function saveSettingDetailClick(settingDetail) {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return settingsDataService.saveSettingDetail(settingDetail, vm.categorySelectedId);
                    })
                .then(function(data) {
                    $("#settingDetail-modal").modal("hide");
                    changeSearchCategory(); //MFP 25-04-2017
                    common.showSuccessMessage(Messages.success.itemUpdated);
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.addOrUpdateItem);
                });
        }




    }
})();