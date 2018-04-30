(function() {
    "use strict";

    angular
        .module(appName)
        .factory("appointmentsDataService",
            ["$http", "common", "appConfig", "authenticationService", appointmentsDataService]);

    function appointmentsDataService($http, common, appConfig, authenticationService) {

        var factory = {
            getStatusList: getStatusList,
            getAppointments: getAppointments,
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


        function getAppointments(dates, query, statusId, delegationId, RequestTypeId) {
            var url = common.getBaseUrl() +
                "api/Calendar/Appointments?dates={0}&query={1}&statusId={2}&delegationId={3}&RequestTypeId={4}".format(
                    dates,
                    query,
                    statusId,
                    delegationId,
                    RequestTypeId);
            var accessToken = authenticationService.getAccessToken();
            
            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getPagedAppointments(pageSize, page, query, statusId) {
            var url = common.getBaseUrl() +
                "api/Administrator/Appointments?pageSize={0}&page={1}&query={2}&statusId={3}".format(pageSize,
                    page,
                    query,
                    statusId);
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