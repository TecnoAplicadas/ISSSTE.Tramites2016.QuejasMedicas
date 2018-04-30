(function() {
    "use strict";

    var controllerId = "catalogsController";
    angular
        .module(appName)
        .controller(controllerId, ["$filter", "common", "catalogsDataService", "webApiService", catalogsController]);

    function catalogsController($filter, common, catalogsDataService, webApiService) {
        var vm = this;

        vm.catalogoDescription = null;
        vm.catalogoId = 1;
        vm.RequestTypeId = null; // Id de tramite
        vm.DelegacionId = null; // Id de delegacion
        vm.query = "";

        vm.requestTypes = [];
        vm.catalogElements = [];// Elementos del catalogo
        vm.headerData = []; //Atributos de un campo de catalogo
        vm.catalogList = []; // Lista de los catalogos que el usuario puede visualizar
        vm.entityType = [];
        vm.editableFields = []; // Contiene los campos habilitados para su edicion
        vm.assignamentsTUs = [];
        vm.selectListItems = [];
        vm.delegations = [];


        vm.pagination = new paginationService();

        vm.catalogDetail = {};
        vm.initCatalogs = initCatalogs;
        vm.searchCatalog = searchCatalog;
        vm.selectedCatalog = selectedCatalog;
        vm.newCatalogDetailClick = newCatalogDetailClick;
        vm.getCatalogDetailClick = getCatalogDetailClick;
        vm.getEditableFields = getEditableFields;
        vm.initFormValidation = initFormValidation;
        vm.pagination.callBackFunction = vm.searchCatalog;
        vm.saveCatalogDetailClick = saveCatalogDetailClick;
        vm.editManyCatalogDetailClick = editManyCatalogDetailClick;
        vm.searchAllocationsTUs = searchAllocationsTUs;
        vm.saveAllocationsTUClick = saveAllocationsTUClick;
        vm.deleteDetailClick = deleteDetailClick;
        vm.activateDetailClick = activateDetailClick;

        vm.leftValue = [];
        vm.rightValue = [];
        vm.addValue = [];
        vm.removeValue = [];
        vm.leftValueCopy = [];
        vm.rightValueCopy = [];

        vm.getRegex = getRegex;// Dependencia con scripts.js

        function loadMoreLeft() {
            vm.leftValue = [];
            angular.forEach(vm.assignamentsTUs.UnidadesNoAsignadas,
                function(item) {
                    vm.leftValue.push({
                        'name': item.UnidadAtencionConcepto,
                        'UnidadAtencionId': item.UnidadAtencionId
                    });
                });

            vm.leftValueCopy = angular.copy(vm.leftValue);
        }

        function loadMoreRight() {
            vm.rightValue = [];
            angular.forEach(vm.assignamentsTUs.UnidadesAsignadas,
                function(item) {
                    vm.rightValue.push({
                        'name': item.UnidadAtencionConcepto,
                        'UnidadAtencionId': item.UnidadAtencionId
                    });
                });

            vm.rightValueCopy = angular.copy(vm.rightValue);
        }

        vm.options = {
            leftContainerScrollEnd: function() {

            },
            rightContainerScrollEnd: function() {

            },
            leftContainerSearch: function(text) {
                console.log(text);
                vm.leftValue = $filter("filter")(vm.leftValueCopy,
                    {
                        'name': text
                    });
            },
            rightContainerSearch: function(text) {

                vm.rightValue = $filter("filter")(vm.rightValueCopy,
                    {
                        'name': text
                    });
            },
            leftContainerLabel: "UADyCS No Asignadas",
            rightContainerLabel: "UADyCS Asignadas",
            onMoveRight: function() {
                console.log("right");
            },
            onMoveLeft: function() {
                console.log("left");
            }

        };

        function init() {
            common.logger.log(Messages.info.controllerLoaded, null, controllerId);
            common.activateController([], controllerId);
        }


        function initCatalogs() {

            common.displayLoadingScreen();

            var promises = [];

            promises.push(getCatalogoLista());
            promises.push(getCatalog());
            promises.push(getEntityTypes());
            promises.push(getDelegations());

            common.$q.all(promises)
                .finally(function() {
                    init();
                    common.hideLoadingScreen();
                });
        }


        function initFormValidation(form) {

            if (form == null || form == undefined) return;
            try {
                form.$setPristine();
                form.$setUntouched();
            } catch (ex) {
            }
        }

        function getElementsToSave() {

            angular.forEach(vm.editableFields,
                function(item) {

                    vm.catalogDetail[item.NombrePropiedad] = item[item.NombrePropiedad];
                });

        }


        function getEditableFieldsInfo() {
            var data = [];

            angular.forEach(vm.headerData,
                function(item) {
                    if (item.Propiedades != null && item.Propiedades != undefined)
                        if (item.Propiedades.EsEditable == true) {

                            if (vm.catalogDetail.hasOwnProperty(item.NombrePropiedad)) {

                                var propertyValues = {};
                                propertyValues[item.NombrePropiedad] = vm.catalogDetail[item.NombrePropiedad];
                                propertyValues["NombrePropiedad"] = item.NombrePropiedad;
                                propertyValues["NombreVista"] = item.Propiedades.NombreVista;
                                propertyValues["Tipo"] = item.Propiedades.TipoElementoString;
                                propertyValues["EsRequerido"] = item.Propiedades.EsRequerido;
                                propertyValues["TamanioMaximo"] = item.Propiedades.TamanioMaximo;
                                propertyValues["TipoDato"] = item.Propiedades.TipoDatoString;
                                

                                data.push(propertyValues);
                            }
                        }
                });

            return data;
        }

        function selectedCatalog(catalogoId, catalogoDescription) {
            vm.catalogoId = catalogoId;
            vm.catalogoDescription = catalogoDescription;
            $(".BotonLink").css("color", "blue");
            $("#btn_" + catalogoId).css("color", "maroon");
            searchCatalog();
        }

        function getEditableFields() {
            var data = [];

            angular.forEach(vm.headerData,
                function(item) {
                    if (item.Propiedades != null && item.Propiedades != undefined)
                        if (item.Propiedades.EsEditable == true) {

                            //var propertyValues = { [item.NombrePropiedad]: vm.catalogDetail[item.NombrePropiedad] };
                            var propertyValues = {};
                            propertyValues[item.NombrePropiedad] = item.Propiedades.TipoElementoString == "checkbox" ? true : "";
                            propertyValues["NombrePropiedad"] = item.NombrePropiedad;
                            propertyValues["NombreVista"] = item.Propiedades.NombreVista;
                            propertyValues["Tipo"] = item.Propiedades.TipoElementoString;
                            propertyValues["EsRequerido"] = item.Propiedades.EsRequerido;
                            propertyValues["TamanioMaximo"] = item.Propiedades.TamanioMaximo;
                            propertyValues["TipoDato"] = item.Propiedades.TipoDatoString;

                            data.push(propertyValues);
                        }
                });

            return data;
        }



        //Informacion o detalle del elemento de catalogo seleccionado
        function getCatalogDetailClick(catalogtDetail, form) {

            initFormValidation(form);

            vm.catalogDetail = catalogtDetail;

            //Obtencion de la informacion de las propiedades editables
            vm.editableFields = getEditableFieldsInfo();

            if (vm.catalogDetail != null || vm.catalogDetail != undefined)
                $("#catalogDetail-modal").modal("show");
        }

        function searchCatalog() {
            common.displayLoadingScreen();

            getCatalog()
                .finally((function() {
                    common.hideLoadingScreen();
                }));
        }

        function editManyCatalogDetailClick(form) {

            vm.leftValue = [];
            vm.rightValue = [];
            vm.RequestTypeId = null;

            initFormValidation(form);
            getRequestTypes();

            $("#seleccionTU-modal").modal("show");
        }

        function newCatalogDetailClick(form) {
            initFormValidation(form);

            //Obtencion de las propiedades habilitadas para su edicion
            vm.editableFields = getEditableFields();
            vm.catalogDetail = {};
            vm.catalogDetail.EsActivo = true;
            $("#catalogDetail-modal").modal("show");
        }


        function deleteDetailClick(catalogtDetail) {

            getElementsToSave();

            vm.catalogDetail = catalogtDetail;

            vm.catalogDetail.EsActivo = false;

            webApiService.makeRetryRequest(1,
                    function() {

                        return catalogsDataService.saveRequirementDetail(vm.catalogoId, vm.catalogDetail);
                    })
                .then(function(data) {

                    searchCatalog();
                    common.showSuccessMessage(Messages.success.itemUpdated);
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.addOrUpdateItem);
                });
        }


        function activateDetailClick(catalogtDetail) {

            getElementsToSave();

            vm.catalogDetail = catalogtDetail;

            vm.catalogDetail.EsActivo = true;

            webApiService.makeRetryRequest(1,
                    function() {

                        return catalogsDataService.saveRequirementDetail(vm.catalogoId, vm.catalogDetail);
                    })
                .then(function(data) {

                    searchCatalog();
                    common.showSuccessMessage(Messages.success.itemUpdated);
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.addOrUpdateItem);
                });
        }

        function saveCatalogDetailClick() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        //Obtencion de las propiedades habilitadas para su edicion
                        getElementsToSave();
                        return catalogsDataService.saveRequirementDetail(vm.catalogoId, vm.catalogDetail);
                    })
                .then(function(data) {
                    $("#catalogDetail-modal").modal("hide");
                    searchCatalog();
                    common.showSuccessMessage(Messages.success.itemUpdated);
                })
                .catch(function(reason) {

                    if (reason.QuejasMedicasMessage != undefined && reason.QuejasMedicasMessage != null)
                        common.showErrorMessage("", reason.QuejasMedicasMessage);
                    else
                        common.showErrorMessage("", Messages.error.addOrUpdateItem);
                });
        }


        function saveAllocationsTUClick() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return catalogsDataService.saveAllocationsTUClick(vm.leftValue,
                            vm.rightValue,
                            vm.RequestTypeId);
                    })
                .then(function(data) {
                    $("#seleccionTU-modal").modal("hide");
                    searchCatalog();
                    common.showSuccessMessage(Messages.success.itemUpdated);
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.addOrUpdateItem);
                });
        }

        function getCatalogoLista() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return catalogsDataService.getCatalogoLista();
                    })
                .then(function(data) {
                    vm.catalogList = data;

                    if (vm.catalogList.length > 0) {
                        vm.catalogoDescription = vm.catalogList[0].Descripcion;
                    }
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetCatalogs);
                });
        }


        function searchAllocationsTUs() {
            vm.leftValue = [];
            vm.rightValue = [];

            if (vm.RequestTypeId == null) return;
            common.displayLoadingScreen();

            getAllocationsTUs()
                .finally((function() {
                    common.hideLoadingScreen();
                }));
        }

        function getAllocationsTUs() {

            return webApiService.makeRetryRequest(1,
                    function() {

                        return catalogsDataService.getAllocationsTUs(vm.RequestTypeId);
                    })
                .then(function(data) {
                    vm.assignamentsTUs = data;
                    loadMoreLeft();
                    loadMoreRight();

                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetUADyCSAllocations);
                });
        }

        function getEntityTypes() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return catalogsDataService.getEntityTypes();
                    })
                .then(function(data) {
                    vm.entityTypes = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetEntityTypes);
                });
        }

        function getDelegations() {
            return webApiService.makeRetryRequest(1,
                function() {
                    return catalogsDataService.getDelegations();
                }).then(function(data) {
                vm.delegations = data;
            }).catch(function(reason) {
                common.showErrorMessage(reason, Messages.error.errorGetDelegations);
            });
        }

        function getRequestTypes() {

            return webApiService.makeRetryRequest(1,
                    function() {
                        return catalogsDataService.getRequestTypes();
                    })
                .then(function(data) {
                    vm.requestTypes = data;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.errorGetRequestTypes);
                });
        }

        function getCatalog() {
            return webApiService.makeRetryRequest(1,
                    function() {
                        return catalogsDataService.getCatalogo(vm.pagination.selectedPageSize,
                            vm.pagination.selectedPage,
                            vm.query,
                            vm.catalogoId);
                    })
                .then(function(data) {

                    vm.headerData = data.Atributos;

                    vm.pagination.selectedPage = data.Resultado.CurrentPage;
                    vm.pagination.totalPages = data.Resultado.TotalPages;
                    vm.pagination.resultCount = data.Resultado.ResultCount;

                    vm.pagination.pages =
                        vm.pagination.paginationHelper.getPages(vm.pagination.selectedPage, vm.pagination.totalPages);

                    vm.catalogElements = data.Resultado.ResultList;
                })
                .catch(function(reason) {
                    common.showErrorMessage(reason, Messages.error.catalogList);
                });
        }
    }
})();