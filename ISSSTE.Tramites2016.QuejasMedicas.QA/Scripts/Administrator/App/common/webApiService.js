(function() {
    "use strict";

    angular
        .module(appName)
        .service("webApiService", ["$window", "$http", "common", "authenticationService", webApiService]);

    function webApiService($window, $http, common, authenticationService) {

        //#region Members

        var vm = this;

        vm.makeRetryRequest = makeRetryRequest;
        vm.makeRetryRequestCount = makeRetryRequestCount;

        //#endregion 

        //#region Fields

        var authorizationData = {};

        //#nedregion

        //#region Methods

        function makeRetryRequest(retryLimit, bodyToRetry) {
            var defer = common.$q.defer();

            bodyToRetry()
                .success(function (result) {
                    defer.resolve(result);
                })
                .error(function (error, status) {
                    if (status == 401) {
                        var authData = authenticationService.getAuthorizationData();

                        if (authData != undefined) {
                            authenticationService.login(authData.client_id, authData.user_name)
                                .then(function () {
                                    vm.makeRetryRequestCount(bodyToRetry, retryLimit, 1)
                                        .then(function (data) {
                                            defer.resolve(data);
                                        })
                                        .catch(function (reason) {
                                            defer.reject(reason);
                                        });
                                }).catch(function (reason) {
                                    defer.reject(reason);

                                    $window.location.href = common.getBaseUrl() + Constants.accountRoutes.softLogout.format(encodeURIComponent($window.location.href));
                                });
                        } else {
                            $window.location.href = common.getBaseUrl() + Constants.accountRoutes.logout.format(encodeURIComponent($window.location.href));
                        }
                    } else {
                        defer.reject(error);
                    }
                });

            return defer.promise;
        }


        function makeRetryRequestCount(bodyToRetry, retryLimit, retryCount) {
            var defer = common.$q.defer();

            if (retryCount <= retryLimit) {
                bodyToRetry().success(function (result) {
                    defer.resolve(result);
                }).error(function (err, status) {
                    vm.makeRetryRequestCount(bodyToRetry, retryLimit, retryCount + 1);

                    if (retryCount == retryLimit - 1)
                        defer.reject({ err: err, status: status });
                });
            }

            return defer.promise;
        }

        //#endregion
    }
})();