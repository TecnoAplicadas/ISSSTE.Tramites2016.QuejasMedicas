var General = " Consulte a su administrador. ";
var Messages = {
    error: {
        statusList: "Ocurrió un error al obtener la lista de estatus." + General,
        requests: "Ocurrió un error al consultar las solicitudes." + General,
        requestDetail: "Ocurrió un error al obtener el detalle de la solicitud." + General,
        nextStatus: "Ocurrió un error al obtener la lista de estatus siguientes para la solicitud." + General,
        requestNotReviewd: "Te faltan algunas cosas, asegúrate de completarlas para poder continuar.",
        nextStatusNotSelected: "No has seleccionado el siguiente estatus de la solicitud.",
        validateDocuments:
            "Ocurrió un error al actualizar el estado de los documentos de la solicitud, inténtalo nuevamente." +
                General,
        updateRequest: "Ocurrió un error al pasar la solicitud al siguiente estatus." + General,
        catalogList: "Ocurrió un error al obtener el listado de elementos del catálogo." + General,
        catalogItemDetail: "Ocurrió un error al obtener el detalle del elemento del catálogo." + General,
        addOrUpdateItem: "Ocurrió un error al guardar los cambios." + General,
        selectStatusAndOpinion: "Selecciona el estatus del diagnóstico y mensaje para el usuario",
        errorSave: "Error al guardar la solicitud" + General,
        errorGetCalendarData: "Error al obtener los datos del calendario." + General,
        errorGetAppointmentData: "Error al obtener datos de las citas." + General,
        errorGetDelegations: "Error al obtener los datos de las delegaciones." + General,
        errorGetRequestTypes: "Error al obtener los tipos de trámites." + General,
        errorGetEntityTypes: "Error al obtener los tipos de entidades federativas." + General,
        errorGetSettings: "Error al obtener las configuraciones." + General,
        errorGetCatalogs: "Error al obtener la lista de catálogos." + General,
        errorGetRequirements: "Error al obtener la lista de requisitos." + General,
        errorGetNotifications: "Error al obtener la lista de notificaciones." + General,
        errorGetWeekdays: "Error al obtener los días de la semana." + General,
        errorSaveCalendar: "Error al guardar los datos de calendario." + General,
        errorGettUADyCSAllocations: "Error al obtener las UADyCS asignadas al trámite" + General,
        errorCalendarSchedule: "Verifique los datos",
        errorCalendarScheduleAndDayRepet: "Ya existe un dia con el horario especificado.",
        errorCalendarScheduleRepet: "Ya existe el horario especificado.",
        errorCalendarDayRepet: "La fecha especificada ya exite.",
        selectStatusAndOpinion: "Selecciona el estatus del diagnóstico y mensaje para el usuario",
        errorSave: "Error al guardar la solicitud" + General,
        errorCalendar: "Ocurrió un error, la fecha y/u hora ya fueron ingresadas" + General,
        errorCalendarDaysNoWorking: "Error, la fecha ya fue ingresada en la sección de días no laborables",
        errorCalendarDaysSpecial: "Error, la fecha ya fue ingresada en la sección de días especiales",
        inyeccionHTML: "Se ha detectado código HTML, favor de revisar"
    },
    success: {
        hourItemAdded: "El horario se ha agregado a la lista.",
        nonLaboralDayAdded: "El dia especial se ha agregado a la lista.",
        specialDayAdded: "El dia especial se ha agregado a la lista.",
        itemUpdated: "Cambios guardados correctamente.",
        unlockUser: "Paciente desbloqueado correctamente.",
        deleteHorarioDiaEspecialDTOs: "Horario eliminado correctamente."
    },
    info: {       
        controllerLoaded: "Controller loaded.",
        workingOnItToggle: "Working on it toogled.",
        navigationMenuOverrided: "Navigation menu overrided.",
        changeRequestId: "Requestid changed.",
        yes: "Si",
        no: "No"
    },
    validation: {
        filtersRequiered: "Debe ingresar al menos un filtro de búsqueda.",
        invalidDate:"La fecha inicio o fin no es valida.",
        required: "El campo es requerido.",
        email: "El formato de un correo eletrónico es \"nombre@dominio.com\".",
        numbers: "Debe de introducir únicamente números.",
        minLenght: "La longitud del campo debe de ser por lo menos de {0} caracteres.",
        rfc: "Ingresa una RFC válido.",
        curp: "Ingresa una CURP válida."
    }
};