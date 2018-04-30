(function() {
    "use strict";

    var controllerId = "loginController";
    angular
        .module(appName)
        .controller(controllerId, ["$window", "common", "authenticationService", loginController]);


    function loginController($window, common, authenticationService) {

        //console.log(new Error().stack);
        //#region Controller Members

        var vm = this;

        vm.continueToLogin = continueToLogin;
        vm.login = login;
        vm.logout = logout;
        //vm.clearToken = clearToken;
        //#endregion

        //#region Functions
        //function clearToken() {
        //    //"cokie borrada."
        //    authenticationService.clearToken();
        //}

        function SringisEmpty(str) {
            str = str.trim();
            return (!str || 0 === str.length);
        }

        function continueToLogin(returnUrl) {
            authenticationService.clearToken();
            // MFP Login -->Redirige al administrador, cuando se inicia desde la ventana de continuar (Account/login)
            //if (SringisEmpty(returnUrl))
            //    returnUrl = Constants.accountRoutes.login + "?returnUrl=" + common.getBaseUrl() + "Administrador/";

            //if (SringisEmpty(returnUrl))
            //    returnUrl = common.getBaseUrl() + "Administrador/";

            $window.location.replace("externallogin?returnUrl=" + encodeURIComponent(returnUrl));
        }

        function login(userName, returnUrl, clientId) {
            authenticationService.login(clientId, userName)
                .then(function (response) {
                    $window.location.replace(returnUrl);
                }).catch(function (reason) {
                    $window.location.replace("loginerror");
                });
        }

        function logout(logoutUrl, returnUrl, soft) {
            authenticationService.clearToken();
            //"logout =>logoutUrl: " + logoutUrl);
            //"logout =>returnUrl: " + returnUrl);
            if (soft) {
                $window.setTimeout(function() {
                        $window.location.replace(returnUrl);
                    },
                    2000);
            } else {
                $window.setTimeout(function () {
                        $window.location.replace(logoutUrl + "?returnUrl=" + encodeURIComponent(returnUrl));
                    },
                    2000);
            }
        }

        //#endregion
    };
})();