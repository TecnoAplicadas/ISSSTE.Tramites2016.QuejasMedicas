(function() {
    "use strict";

    angular
        .module(appName)
        .service("navigationService", ["$http", "common", "appConfig", "authenticationService"], navigationService);

    function navigationService($http, common, appConfig, authenticationService) {

        var factory = {
            getMenu: getMenu,
        };

        return factory;

        function getMenu() {
            var url = common.getBaseUrl() + "api/Common/User/Menu";
            var accessToken = authenticationService.getAccessToken();
            jsValidaAngular(url, null);

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