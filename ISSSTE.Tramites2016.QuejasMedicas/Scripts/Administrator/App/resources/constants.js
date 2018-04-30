var REQUEST_ID_PARAM = "solicitudId";
var REQUEST_FILTERS_PARAM = "filters";

var MESSAGE_ID_PARAM = "messageId";
var JSON_CONTENT_TYPE = "application/json";
var FORM_CONTENT_TYPE = "application/x-www-form-urlencoded";
var BEARER_TOKEN_AUTHENTICATION_TEMPLATE = "Bearer {0}";

var Constants =
{
    accountRoutes: {
        logout: "account/logout?returnUrl={0}",
        softLogout: "account/logout?returnUrl={0}&soft=true",
        login: "account/login"
    },
    roles: {
        administrator: {
            names: "Administrador UADyCS",
            Permissions: { "CanRead": true, "CanCreate": false, "CanEdit": false, "CanCancel": true }
        },
        operator: {
            names: "Operador UADyCS",
            Permissions: { "CanRead": true, "CanCreate": true, "CanEdit": true, "CanCancel": true }
        },
    }
};