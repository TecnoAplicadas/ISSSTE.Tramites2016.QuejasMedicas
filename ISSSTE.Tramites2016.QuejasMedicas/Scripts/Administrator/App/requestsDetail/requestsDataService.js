(function() {
    "use strict";

    angular
        .module(appName)
        .factory("requestsDataService", ["$http", "common", "appConfig", "authenticationService", requestsDataService]);

    function requestsDataService($http, common, appConfig, authenticationService) {


        var factory = {
            getRequestEntitleInformation: getRequestEntitleInformation,
            getNextStatusList: getNextStatusList,
            unlockUserByRequestId: unlockUserByRequestId,
            saveStatusAppointment: saveStatusAppointment
        };

        return factory;


        function getRequestEntitleInformation(requestId) {
            var url = common.getBaseUrl() + "api/Administrator/AllInformation/" + requestId;
            var accessToken = authenticationService.getAccessToken();


            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getNextStatusList() {
            var url = common.getBaseUrl() + "api/Common/NextStatus";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function saveStatusAppointment(requestId, statusId) {

            var url = common.getBaseUrl() +
                "api/Administrator/SaveStatus?requestId={0}&statusId={1}".format(requestId, statusId);
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function unlockUserByRequestId(requestId) {
            var url = common.getBaseUrl() + "api/Administrator/unlockUser/" + requestId;
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