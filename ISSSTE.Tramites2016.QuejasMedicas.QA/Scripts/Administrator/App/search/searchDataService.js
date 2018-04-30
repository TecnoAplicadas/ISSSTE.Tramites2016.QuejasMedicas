(function() {
    "use strict";

    angular
        .module(appName)
        .factory("searchDataService", ["$http", "common", "appConfig", "authenticationService", searchDataService]);

    function searchDataService($http, common, appConfig, authenticationService) {


        var factory = {
            getStatusList: getStatusList,
            getRequests: getRequests,
            getRequestTypes: getRequestTypes,
            getUserDelegations: getUserDelegations
        };

        return factory;


        function getStatusList() {
            var url = common.getBaseUrl() + "api/Administrator/Status";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


        function getRequests(pageSize, page, query, dates, statusId, delegationId, RequestTypeId) {
            var url = common.getBaseUrl() +
                "api/Administrator/Requests?pageSize={0}&page={1}&dates={2}&query={3}&statusId={4}&delegationId={5}&RequestTypeId={6}"
                .format(pageSize, page, dates, query, statusId, delegationId, RequestTypeId);
           
            var accessToken = authenticationService.getAccessToken();
            
            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getRequestTypes(dates, query, statusId, delegationId, RequestTypeId) {
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

        function getUserDelegations() {
            var url = common.getBaseUrl() + "api/Common/User/Delegations";
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