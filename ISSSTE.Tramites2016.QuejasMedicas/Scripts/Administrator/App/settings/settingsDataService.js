(function() {
    "use strict";

    angular
        .module(appName)
        .factory("settingsDataService", ["$http", "common", "appConfig", "authenticationService", settingsDataService]);

    function settingsDataService($http, common, appConfig, authenticationService) {

        //#region Members

        var factory = {
            getRequestTypes: getRequestTypes,
            getSettings: getSettings,
            saveSettingDetail: saveSettingDetail,
            getMessagessApp: getMessagessApp
        };

        return factory;


        function getSettings(pageSize, page, query) {

            var url = common.getBaseUrl() + "api/Administrator/Settings";
            var accessToken = authenticationService.getAccessToken();

            var params = {
                PageSize: pageSize,
                CurrentPage: page,
                QueryString: query,
                ResultCount: 0
            };


           jsValidaAngular(url, params);

            return $http.post(url,
                            params,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });

        }

        function getMessagessApp(pageSize, page, query) {

            var url = common.getBaseUrl() + "api/Administrator/Messages";
            var accessToken = authenticationService.getAccessToken();

            var params = {
                PageSize: pageSize,
                CurrentPage: page,
                QueryString: query,
                ResultCount: 0
            };


            jsValidaAngular(url, params);

            return $http.post(url,
                            params,
                            {
                                headers: {
                                    'Content-Type': JSON_CONTENT_TYPE,
                                    'Authorization': BEARER_TOKEN_AUTHENTICATION_TEMPLATE.format(accessToken)
                                }
                            });
        }


        function saveSettingDetail(settingDetail, categorySelectedId) {

            var url = "";

            if (categorySelectedId == 1) url = common.getBaseUrl() + "api/Administrator/SaveSettingDetail";
            else url = common.getBaseUrl() + "api/Administrator/SaveMessageDetail";

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