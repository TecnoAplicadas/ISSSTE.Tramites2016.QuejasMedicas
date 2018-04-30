(function() {
    "use strict";

    angular
        .module(appName)
        .factory("reportsDataService", ["$http", "common", "appConfig", "authenticationService", reportsDataService]);

    function reportsDataService($http, common, appConfig, authenticationService) {


        var factory = {
        };

        return factory;

    }

})();