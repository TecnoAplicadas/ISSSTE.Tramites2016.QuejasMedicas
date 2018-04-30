(function() {
    "use strict";

    angular
        .module(appName)
        .service("authenticationService",
            ["$http", "common", "appConfig", "localStorageService", authenticationService]);

    function authenticationService($http, common, appConfig, localStorageService) {

        //#region Members

        var vm = this;

        vm.getAuthorizationData = getAuthorizationData;
        vm.getAccessToken = getAccessToken;

        vm.login = login;
        vm.clearToken = clearToken;
        vm.validateToken = validateToken;
        vm.getUserRoles = getUserRoles;
        vm.getMenu = getMenu;
        vm.getUserDelegations = getUserDelegations;
        vm.getNotifications = getNotifications;
        //#endregion 

        //#region Constants

        var BEARER_TOKEN_DATA_TEMPLATE = "grant_type=password&username={0}&password=&client_id={1}";

        var AUTHENTICATION_TIMEOUT = 150000;

        //#endregion

        //#region Properties

        function getAuthorizationData() {
            return localStorageService.get(appConfig.authorizationDataLocalStorageKey);
        }

        function getAccessToken() {
            return getAuthorizationData() == null ? null : getAuthorizationData().access_token;
        }

        //#nedregion

        //#region Methods

        function login(clientId, userName) {

            var deferred = common.$q.defer();

            var data = BEARER_TOKEN_DATA_TEMPLATE.format(userName, clientId);

            $http.post(common.getBaseUrl() + "token",
                    data,
                    {
                        headers: {
                            "Content-Type": FORM_CONTENT_TYPE
                        },
                        timeout: AUTHENTICATION_TIMEOUT
                    })
                .success(function(data, status, headers, config) {
                    //"login correcto."
                    localStorageService.set(appConfig.authorizationDataLocalStorageKey, data);

                    deferred.resolve(data);
                })
                .error(function(data, status, headers, config) {
                    //"login incorrecto."
                    vm.clearToken();

                    //TODO: Validar procesamiento de errores
                    if (data.error_description != undefined)
                        deferred.reject(data.error_description);
                    else if (data.Message != undefined)
                        deferred.reject(data.Message);
                    else
                        deferred.reject("Ocurrió un error");
                });

            return deferred.promise;
        }

        function clearToken() {
            localStorageService.remove(appConfig.authorizationDataLocalStorageKey);
        }

        function validateToken() {
            var url = common.getBaseUrl() + "api/Administrator/Token/Validate";
            var accessToken = vm.getAccessToken();


            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getUserRoles() {
            var url = common.getBaseUrl() + "api/Administrator/User/Roles";
            var accessToken = vm.getAccessToken();


            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getMenu() {
            var url = common.getBaseUrl() + "api/Common/User/Menu";
            var accessToken = vm.getAccessToken();


            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getNotifications() {
            var url = common.getBaseUrl() + "api/Common/Notifications";
            var accessToken = vm.getAccessToken();


            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }

        function getUserDelegations() {
            var url = common.getBaseUrl() + "api/Common/User/Delegations";
            var accessToken = vm.getAccessToken();
		 

            return $http.get(url, {
                headers: {
                    'Content-Type': JSON_CONTENT_TYPE,
                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                }
            });
        }


        //#endregion
    }
})();