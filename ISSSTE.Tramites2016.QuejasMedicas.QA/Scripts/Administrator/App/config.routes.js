var Routes = {
    search: {
        url: "/search",
        templateUrl: "Scripts/Administrator/App/search/search.html",
        roles: [Constants.roles.administrator.names, Constants.roles.operator.names]
    },
    requestDetail: {
        url: "/requestsDetail/:solicitudId",
        templateUrl: "Scripts/Administrator/App/requestsDetail/requestDetail.html",
        roles: [Constants.roles.administrator.names, Constants.roles.operator.names]
    },
    appointments: {
        url: "/appointments",
        templateUrl: "Scripts/Administrator/App/Appointments/appointments.html",
        roles: [Constants.roles.administrator.names, Constants.roles.operator.names]
    },
    calendar: {
        url: "/calendar",
        templateUrl: "Scripts/Administrator/App/calendar/calendario.html",
        roles: [Constants.roles.administrator.names, Constants.roles.operator.names]
    },
    requirements: {
        url: "/requirements",
        templateUrl: "Scripts/Administrator/App/requirements/requirements.html",
        roles: [Constants.roles.administrator.names]
    },
    settings: {
        url: "/settings",
        templateUrl: "Scripts/Administrator/App/settings/settings.html",
        roles: [Constants.roles.administrator.names]

    },
    catalogs: {
        url: "/catalogs",
        templateUrl: "Scripts/Administrator/App/catalogs/catalogs.html",
        roles: [Constants.roles.administrator.names]

    },
    ReportDinamic: {
        url: "/ReporteDinamico",
        templateUrl: "Administrador/ReporteDinamico",
        roles: [Constants.roles.administrator.names, Constants.roles.operator.names]
    },
    ReportStatistic: {
        url: "/ReporteEstadistico",
    templateUrl: "Administrador/ReporteEstadistico",
    roles: [Constants.roles.administrator.names, Constants.roles.operator.names]
    },
    noAccess: {
        url: "/noacceso",
        templateUrl: "Scripts/Administrator/App/error/noaccess.html"
    }

}