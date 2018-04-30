(function() {
    "use strict";

    angular
        .module(appName)
        .factory("calendarDataService", ["$http", "common", "appConfig", "authenticationService", calendarDataService]);

    function calendarDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getExistingData: getExistingData,
            getUserDelegationsByConfig: getUserDelegationsByConfig,
            getUserDelegations: getUserDelegations,
            saveSchedules: saveSchedules,
            deleteSchedule: deleteSchedule,
            saveDiaEspecialDTOs: saveDiaEspecialDTOs,
            deleteDiaEspecialDTO: deleteDiaEspecialDTO,
            saveSpecialScheduleDays: saveSpecialScheduleDays,
            deleteSpecialScheduleDays: deleteSpecialScheduleDays,
            getDeleteHorarioDiaEspecialDTOs: getDeleteHorarioDiaEspecialDTOs,
            getRequestTypes: getRequestTypes,
            getWeekdays: getWeekdays

        };

        return factory;

        function getWeekdays() {
            
            var url = common.getBaseUrl() + "api/Common/Weekdays";
            var accessToken = authenticationService.getAccessToken();
            jsValidaAngular(url, null);

           return $http.get(url, {
               headers: {
                   'Content-Type': JSON_CONTENT_TYPE,
                   'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
               }
           });
        }

        function getExistingData(RequestTypeId, delegationId) {
            var url = common.getBaseUrl() +
                "api/Administrator/AdmninistratorSchedule?RequestTypeId={0}&delegationId={1}".format(RequestTypeId,
                    delegationId);

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

        function getUserDelegationsByConfig(RequestTypeId) {
            var url = common.getBaseUrl() + "api/Common/User/DelegationsByConfig/{0}".format(RequestTypeId);
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

        function saveSchedules(schedules) {
            var url = common.getBaseUrl() + "/api/Administrator/SaveSchedules";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, schedules);

            return $http.post(url,
                            schedules,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });
        }

        function deleteSchedule(SolicitudId, UnidadAtencionId, HorarioInicio, CatTipoDiaSemanaId) {
            var url = common.getBaseUrl() +
                "api/Administrator/DeleteSchedule?SolicitudId={0}&UnidadAtencionId={1}&HorarioInicio={2}&CatTipoDiaSemanaId={3}"
                .format(SolicitudId, UnidadAtencionId, HorarioInicio, CatTipoDiaSemanaId);
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        //Elimia el horario de un dia especial
        function getDeleteHorarioDiaEspecialDTOs(SolicitudId, UnidadAtencionId, HorarioInicio) {
            var url = common.getBaseUrl() +
                "api/Administrator/DeleteSpecialScheduleDays?SolicitudId={0}&UnidadAtencionId={1}&HorarioInicio={2}"
                .format(SolicitudId, UnidadAtencionId, HorarioInicio);
           
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, null);

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function saveDiaEspecialDTOs(nonWorkingDays) {
            var url = common.getBaseUrl() + "/api/Administrator/SaveNonLaborableDays";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, nonWorkingDays);

            return $http.post(url,
                            nonWorkingDays,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });
        }

        function deleteDiaEspecialDTO(info) {
            var url = common.getBaseUrl() + "api/Administrator/DeleteNonLaboraleDays";
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, info);

            return $http.post(url,
                            info,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });
        }

        function saveSpecialScheduleDays(specialScheduleDays) {
            var url = common.getBaseUrl() + "/api/Administrator/SaveSpecialScheduleDays";           
            var accessToken = authenticationService.getAccessToken();

            jsValidaAngular(url, specialScheduleDays);

            return $http.post(url,
                            specialScheduleDays,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });
        }

        function deleteSpecialScheduleDays(specialScheduleDays) {
            var url = common.getBaseUrl() +
                "api/Administrator/DeleteSpecialScheduleDays/{0}".format(specialScheduleDays);
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