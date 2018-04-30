(function() {
    "use strict";

    angular.module("common").factory("logger",
        ["$log", "appConfig", logger]);

    function logger($log, appConfig) {
        var service = {
            log: log,
            logError: logError,
            logSuccess: logSuccess,
            logWarning: logWarning
        };

        return service;

        //#region Methods

        function log(message, data, source) {
            writeLog(message, data, source, "info");
        }

        function logError(message, data, source) {
            writeLog(message, data, source, "error");
        }

        function logSuccess(message, data, source) {
            writeLog(message, data, source, "success");
        }

        function logWarning(message, data, source) {
            writeLog(message, data, source, "warning");
        }

        //#endregion

        //#region Helpers

        function writeLog(message, data, source, notificationType) {
            var write = (notificationType === "error") ? $log.error : $log.log;
            source = source ? "[" + source + "] " : "";
            write(source, message, data);

            if (appConfig.showDebugNotiSetting) {
                if (notificationType === "info") {
                    UI.createInfoMessage(message);
                } else if (notificationType === "error") {
                    UI.createErrorMessage(message);
                } else if (notificationType === "warning") {
                    UI.createWarningMessage(message);
                } else if (notificationType === "success") {
                    UI.createSuccessMessage(message);
                }
            }
        }

        //#endregion
    }
})();