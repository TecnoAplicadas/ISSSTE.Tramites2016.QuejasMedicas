(function() {
    "use strict";

    angular
        .module(appName)
        .factory("requirementsDataService",
            ["$http", "common", "appConfig", "authenticationService", requirementsDataService]);

    function requirementsDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getRequestTypes: getRequestTypes,
            getRequirements: getRequirements,
            saveRequirementDetail: saveRequirementDetail
        };

        return factory;

        function getRequirements(pageSize, page, query, RequestTypeIdArg) {

            var url = common.getBaseUrl() + "api/Administrator/requirements";
            var accessToken = authenticationService.getAccessToken();

            var paginadoArg = {
                PageSize: pageSize,
                CurrentPage: page,
                QueryString: query,
                ResultCount: 0
            };


            var parametros = { paginado: paginadoArg, RequestTypeId: RequestTypeIdArg };


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


        function saveRequirementDetail(settingDetail) {

            var url = common.getBaseUrl() + "api/Administrator/SaveRequirementDetail";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, settingDetail);

            return $http.post(url,
                            settingDetail,
                            {
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


    }
})();