(function() {
    "use strict";

    angular
        .module(appName)
        .factory("catalogsDataService", ["$http", "common", "appConfig", "authenticationService", catalogsDataService]);

    function catalogsDataService($http, common, appConfig, authenticationService) {


        var factory = {
            getCatalogoLista: getCatalogoLista,
            getCatalogo: getCatalogo,
            saveRequirementDetail: saveRequirementDetail,
            getEntityTypes: getEntityTypes,
            getRequestTypes: getRequestTypes,
            getAllocationsTUs: getAllocationsTUs,
            saveAllocationsTUClick: saveAllocationsTUClick,
            getDelegations: getDelegations
        };

        return factory;


        function saveAllocationsTUClick(leftValue, rightValue, RequestTypeId) {
            var url = common.getBaseUrl() + "api/Administrator/SetAsignacionesTramiteUnidad";
            var asignaciones = {
                CatTipoTramiteId: RequestTypeId,
                UnidadesNoAsignadas: leftValue,
                UnidadesAsignadas: rightValue
            };
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, asignaciones);

            return $http.post(url,
                            asignaciones,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });

        }

        function getDelegations() {
            var url = common.getBaseUrl() + "api/Common/Delegations";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getAllocationsTUs(RequestTypeId) {
            var url = common.getBaseUrl() + "api/Administrator/AsignacionesTramiteUnidad/{0}".format(RequestTypeId);
            var accessToken = authenticationService.getAccessToken();

           jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getRequestTypes() {
            var url = common.getBaseUrl() + "api/Common/RequestTypes";
            var accessToken = authenticationService.getAccessToken();

           jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getCatalogo(pageSize, page, query, catalogoId) {

            var url = common.getBaseUrl() + "api/Administrator/Catalogo";

            var params = {
                PageSize: pageSize,
                CurrentPage: page,
                QueryString: query,
                ResultCount: 0
            };

            var parametros = { Paginado: params, CatalogoId: catalogoId };

            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, parametros);

            return $http.post(url,
                            parametros,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });
        }


        function getCatalogoLista() {
            var url = common.getBaseUrl() + "api/Administrator/Catalogos";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getEntityTypes() {
            var url = common.getBaseUrl() + "api/Common/Entities";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


        function saveRequirementDetail(catalogoId, data) {
            var json = JSON.stringify(data);
            var parametros = { CatalogoId: catalogoId, JsonObject: json };

            var url = common.getBaseUrl() + "api/Administrator/ActulizaCatalogo";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, parametros);

            return $http.post(url,
                            parametros,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });

        }

    }
})();