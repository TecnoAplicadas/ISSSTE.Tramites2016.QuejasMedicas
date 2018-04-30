var G_ModeloConsulta = {};

function jsMesActual() {
    var fecha = new Date();
    fecha = new String(fecha.getMonth() + 1);
    if (fecha.length === 1) fecha = "0" + fecha;
    return fecha;
    //     return Fecha.getFullYear();
}


function AgendaCita() {
   
    //jsResultado(G_MENSAJES.MENSAJE_ASEGURAMIENTO, "PRECAUCION");MFP 24/07/2017 UGD
    G_ModeloAgenda = {};
    GralAgendaCita();
}



function GralAgendaCita() {
    AccionActiva("AgendaCita");
    jsAjaxGenerico("../Promovente/Agenda", null, "SeccionTrabajoGeneral", null, false);
    BreadCrumb("Agendar Cita ");
}

function PasoAnterior() {
    getModeloAgendaHorario();
    GralAgendaCita();
    SetAgendaCita();
    AnimarScrollTop();//MFP 30/05/2017
}

function SetAgendaCita() {
    $("#NombrePromovente").val(G_ModeloAgenda.NombrePromovente);
    $("#Apellido1Promovente").val(G_ModeloAgenda.Apellido1Promovente);
    $("#Apellido2Promovente").val(G_ModeloAgenda.Apellido2Promovente);
    $("#CURPPromovente").val(G_ModeloAgenda.CURPPromovente);
    $("#EdadPromovente").html(G_ModeloAgenda.EdadPromovente);
    $("#SexoPromovente").html(G_ModeloAgenda.SexoPromovente);
    $("#LadaPromovente").val(G_ModeloAgenda.LadaPromovente);
    $("#TelefonoPromovente").val(G_ModeloAgenda.TelefonoPromovente);
    $("#CelularPromovente").val(G_ModeloAgenda.CelularPromovente);
    $("#CorreoPromovente").val(G_ModeloAgenda.CorreoPromovente);

    $("#NombrePaciente").val(G_ModeloAgenda.NombrePaciente);
    $("#Apellido1Paciente").val(G_ModeloAgenda.Apellido1Paciente);
    $("#Apellido2Paciente").val(G_ModeloAgenda.Apellido2Paciente);
    $("#CURPPaciente").val(G_ModeloAgenda.CURPPaciente);
    $("#EdadPaciente").html(G_ModeloAgenda.EdadPaciente);
    $("#SexoPaciente").html(G_ModeloAgenda.SexoPaciente);
    $("#LadaPaciente").val(G_ModeloAgenda.LadaPaciente);
    $("#TelefonoPaciente").val(G_ModeloAgenda.TelefonoPaciente);
    $("#CelularPaciente").val(G_ModeloAgenda.CelularPaciente);
    $("#CorreoPaciente").val(G_ModeloAgenda.CorreoPaciente);

    if (!G_ModeloAgenda.EsRepresentante) { //MFP 29/05/2017
        $("#PacienteCheckSi").removeClass("glyphicon-unchecked");
        $("#PacienteCheckSi").addClass("glyphicon-check");
        $("#PacienteCheckNo").removeClass("glyphicon-check");
        $("#PacienteCheckNo").addClass("glyphicon-unchecked");
    } else {
        $("#PacienteCheckSi").removeClass("glyphicon-check");
        $("#PacienteCheckSi").addClass("glyphicon-unchecked");
        $("#PacienteCheckNo").removeClass("glyphicon-unchecked");
        $("#PacienteCheckNo").addClass("glyphicon-check");
    }
    jsCheckUpPaciente();
}

function DaysInMonth(humanMonth, year) {
    return new Date(year || new Date().getFullYear(), humanMonth, 0).getDate();
}

function BuscarHorario() {

    //if ($("#MesAnio").val() === 0) return;
    //var valor = ValidaModeloGenerico("RegistroCita", true); MFP 
    var val = $("#MesAnio").val();
    if (val == 0) {
        $("#RegistrarCita").attr("disabled", "disabled");
        $("#SeccionCalendario").html("");
        return;
    }


    var valores = val.split(" ");
    var ultimoDiaMes = DaysInMonth(MesCadena(valores[0]), parseInt(valores[1]));

    var fechaConsulta = new Date(parseInt(valores[1]), (parseInt(valores[0]) - 1), parseInt(ultimoDiaMes));
    var fechaHoy = new Date();
    if (fechaConsulta < fechaHoy) {
        jsResultError(G_MENSAJES.MES_CURSO, true);
        return;
    }

    var modelo = jsGetModelo("RegistroCita");
    modelo.MesAnio = MesCadena(valores[0]) + "/" + valores[1];
    jsAjaxGenerico("../Promovente/Calendario", modelo, "SeccionCalendario");
}

function MesCadena(valor) {
    valor = valor.toUpperCase();
    valor = valor.trim();
    switch (valor) {
    case "ENERO":
        return "01";
    case "FEBRERO":
        return "02";
    case "MARZO":
        return "03";
    case "ABRIL":
        return "04";
    case "MAYO":
        return "05";
    case "JUNIO":
        return "06";
    case "JULIO":
        return "07";
    case "AGOSTO":
        return "08";
    case "SEPTIEMBRE":
        return "09";
    case "OCTUBRE":
        return "10";
    case "NOVIEMBRE":
        return "11";
    case "DICIEMBRE":
        return "12";
    }
    return "01";
}

function jsSituacion(id) {
    var validacion = true;
    G_NoEsUsuarioBloqueado = true;
    $("#" + G_contenedorMensaje).html("");
    var CURP = $("#CURP" + id).val()
    if (id != null && CURP.length==18) {
        //alert( $("#PacienteCheckSi").hasClass("glyphicon-check"));

        if ((!$("#PacienteCheckSi").hasClass("glyphicon-check") && id === "Promovente") ||
            (id !== "Promovente" && $("#PacienteCheckSi").hasClass("glyphicon-check"))) {
            jsBloqueo(id);
            validacion = G_NoEsUsuarioBloqueado;
        } 
    } else {
        if (!$("#PacienteCheckSi").hasClass("glyphicon-check")) {
            jsBloqueo("Promovente");
            validacion = G_NoEsUsuarioBloqueado;
        } else {
            jsBloqueo("Paciente");
            validacion = G_NoEsUsuarioBloqueado;
        }
    }
    return validacion;
}

function jsResultImageConfirm(html) {
    $("#VistaEspecial_VP_SRC").attr("src", html);
}

function jsBloqueo(id) { 
   
    var nombrePaciente = "";
    var aPaternoPaciente = "";
    var aMaternoPaciente = "";

    var esRepresentante = $("#PacienteCheckSi").hasClass("glyphicon-check");

    if (esRepresentante) {
        nombrePaciente = $("#NombrePaciente").val();
        aPaternoPaciente = $("#Apellido1Paciente").val();
        aMaternoPaciente = $("#Apellido2Paciente").val();
    } else { 
        nombrePaciente = $("#NombrePromovente").val();
        aPaternoPaciente = $("#Apellido1Promovente").val();
        aMaternoPaciente = $("#Apellido2Promovente").val();
    }

    G_ModeloConsulta = {
        CURP: $("#CURP" + id).val(),
        CatTipoTramiteId: parseInt($("#IdCatTipoSistema").html()),
        Nombre: nombrePaciente,
        Paterno: aPaternoPaciente,
        Materno: aMaternoPaciente
    };

    jsAjaxGenerico("../Promovente/BloqueoPersona", G_ModeloConsulta, null, jsSituacionBloqueo, true);
}

function jsSituacionBloqueo(html) {
    if (IndexOf(html,"ERROR") >= 0) {
        jsSituacionResult(html);
    } else {
        jsAjaxGenerico("../Promovente/CitasVigentesPorTramite", G_ModeloConsulta, null, jsSituacionResult);
    }
}

function jsSituacionResult(html) {
    html = new String(html);
    if (IndexOf(html,"ERROR") >= 0) {
        ActivarPanel("TabPromovente", "TabPaciente");
        html = html.replace("ERROR: ", "");
        jsResultError(html, true);
        G_NoEsUsuarioBloqueado = false;
    }
}

function ValidaPromoventePaciente(conFocusCURP) {

    var esRepresentante = $("#PacienteCheckSi").hasClass("glyphicon-check");
    var valor = false;
    if (esRepresentante) {
        valor = ValidaModeloGenerico("SeccionPromovente", false);
        if (!valor) {
            ActivarPanel("TabPromovente", "TabPaciente");
            return valor;
        }
        valor = ValidaModeloGenerico("SeccionPaciente", false);
        if (!valor) {
            ActivarPanel("TabPaciente", "TabPromovente");
            return valor;
        }
        valor = jsValidaCorreo("CorreoPromovente");
        if (!valor) {
            ActivarPanel("TabPromovente", "TabPaciente");
            return valor;
        }
        valor = jsValidaCorreo("CorreoPaciente");
        if (!valor) {
            ActivarPanel("TabPaciente", "TabPromovente");
            return valor;
        }
        valor = jsValidaCURP("CURPPromovente", conFocusCURP);
        if (!valor) {
            ActivarPanel("TabPromovente", "TabPaciente");
            return valor;
        }
        valor = jsSituacion("Promovente");
        if (!valor) {
            ActivarPanel("TabPromovente", "TabPaciente");
            return valor;
        }
        valor = jsValidaCURP("CURPPaciente",conFocusCURP);
        if (!valor) {
            ActivarPanel("TabPaciente", "TabPromovente");
            return valor;
        }
        valor = jsSituacion("Paciente");
        if (!valor) {
            ActivarPanel("TabPaciente", "TabPromovente");
            return valor;
        }
    } else {
        valor = ValidaModeloGenerico("SeccionPromovente", false);
        if (!valor) {
            return valor;
        }
        valor = jsValidaCorreo("CorreoPromovente");
        if (!valor) {
            return valor;
        }
        valor = jsValidaCURP("CURPPromovente", conFocusCURP);
        if (!valor) {
            return valor;
        }
        valor = jsSituacion("Promovente");
        if (!valor) {
            ActivarPanel("TabPromovente", "TabPaciente");
            return valor;
        }
    }
    return valor;
}

function jsChange(id) {

    $("#Sexo" + id).html("");
    $("#Edad" + id).html("");
    var validacion = jsValidaCURP("CURP" + id);
    if (!validacion) return false;

    var valor = $("#CURP" + id).val();
    valor = valor[10];

    if (valor == "H") $("#Sexo" + id).html("Masculino");
    if (valor == "M") $("#Sexo" + id).html("Femenino");

    valor = $("#CURP" + id).val();
    var prefijo = valor[16];
    valor = valor.substring(4, 10);

    var Mes = valor.substring(2, 4);
    var Dia = valor.substring(4, 6);
    valor = valor.substring(0, 2);

    var A_no = (is_numeric(prefijo)) ? "19" + valor : "20" + valor;
    
    if (prefijo != undefined || Mes.length != 0 || Dia.length != 0) {

        var FechaNacimiento = new Date(A_no, Mes - 1, Dia);
        var Hoy = new Date();
        var Edad = parseInt((Hoy - FechaNacimiento) / 365 / 24 / 60 / 60 / 1000);
        $("#Edad" + id).html(Edad);
    }

    validacion = jsSituacion(id);
    return validacion;
}

function is_numeric(str) {
    return $.isNumeric(str);
}

function jsValidaCorreo(id) {
    var valor = true;
    if ($("#" + id).val() !== "") {
        valor = jsValidaCorreoElectronico($("#" + id).val(), id);
        if (!valor) {
            jsResultError(G_MENSAJES.CORREO_VALIDO, true);
        }
    }
    return valor;
}

function ActivarPanel(divActivo, divInactivo) {
    $("#" + divInactivo).removeClass("active");
    $("#" + divActivo).addClass("active");
    $("#" + divInactivo + "Div").removeClass("active");
    $("#" + divActivo + "Div").addClass("active");
}

function LimpiarCampos() {

    if ($("#TabPaciente").hasClass("active")) {
        LimpiarModelo("SeccionPaciente");
        $("#SexoPaciente").html("");
        $("#EdadPaciente").html("");
    } else {
        LimpiarModelo("SeccionPromovente");
        //alert("limpio")
        //$("#SexoPromovente").html("");
        //$("#EdadPromovente").html("");
    }
}

function getModeloAgendaDatosIniciales() {

    G_ModeloAgenda = (G_ModeloAgenda == undefined || G_ModeloAgenda == null) ? {} : G_ModeloAgenda;
    var modeloAux = G_ModeloAgenda;

    G_ModeloAgenda = jsGetModelo("SeccionPrevioRegistro");
    G_ModeloAgenda.EsRepresentante = $("#PacienteCheckSi").hasClass("glyphicon-check");

    G_ModeloAgenda.EdadPromovente = $("#EdadPromovente").html();
    G_ModeloAgenda.SexoPromovente = $("#SexoPromovente").html();

    G_ModeloAgenda.EdadPaciente = $("#EdadPaciente").html();
    G_ModeloAgenda.SexoPaciente = $("#SexoPaciente").html();

    G_ModeloAgenda.FechaFinal = modeloAux.FechaFinal;
    G_ModeloAgenda.HorarioFinal = modeloAux.HorarioFinal;

    G_ModeloAgenda.Unidad_Medica = modeloAux.Unidad_Medica;
    G_ModeloAgenda.Unidad_Atencion = modeloAux.Unidad_Atencion;

    G_ModeloAgenda.HorarioId = modeloAux.HorarioId;

    G_ModeloAgenda.TramiteUnidadAtencionId = modeloAux.TramiteUnidadAtencionId;
    G_ModeloAgenda.UnidadMedicaId = modeloAux.UnidadMedicaId;
    G_ModeloAgenda.MesAnio = modeloAux.MesAnio;
}

function getModeloAgendaHorario() {

    G_ModeloAgenda = (G_ModeloAgenda == undefined || G_ModeloAgenda == null) ? {} : G_ModeloAgenda;

    G_ModeloAgenda.FechaFinal = $("#FechaFinal").html();
    G_ModeloAgenda.HorarioFinal = $("#HorarioFinal").html();

    G_ModeloAgenda.Unidad_Medica = $("#UnidadMedicaId option:selected").text();
    G_ModeloAgenda.Unidad_Atencion = $("#TramiteUnidadAtencionId option:selected").text();

    G_ModeloAgenda.HorarioId = $("input:radio[name=HorarioSeleccionado]:checked").val();

    G_ModeloAgenda.TramiteUnidadAtencionId = parseInt($("#TramiteUnidadAtencionId").val());
    G_ModeloAgenda.UnidadMedicaId = parseInt($("#UnidadMedicaId").val());


    G_ModeloAgenda.MesAnio = $("#MesAnio").val();

}

function RegistrarCita() {

    var valor = ValidaModeloGenerico("RegistroCita", true);
    
    if (!valor) { return; }

    if ($('#MesAnio').val()==0) {
        $("#SeccionCalendario").html("");
        return;
    }

    getModeloAgendaHorario();
    jsAjaxGenerico("../Promovente/Confirmacion", null, "SeccionTrabajoGeneral", null, false);
    Recaptcha();
    AnimarScrollTop();
}

function jsGetEncuentaSatisfaccion(time) {
    var modelo = { CatTipoTramiteId: parseInt($("#IdCatTipoSistema").html()) };
    jsAjaxGenerico("../Promovente/ObtenerHomoclaveTramite", modelo, null, jsResultGetHomoclave);

    try {
        startEncuestaHC(time, Homoclave); //  Llamada a la encuesta después  de 2 segundos     	                  
    } catch (ex) {
    }
}

function jsResultadoAgenda(html) {

    if (IndexOf(html,"ERROR") >= 0) {
        html = html.replace("ERROR: ", "");
        jsResultError(html, true);
        Recaptcha();
    } else {
        G_citaFolio = html;
        $("#Div_FolioGeneral").css("display", "");
        $("#FolioGeneral").html(html);

        G_ModeloAgenda = {};
        jsResultExitoso(G_MENSAJES.NUMERO_SOLICITUD + html+".", true);

        $("#ResultadoAgenda").addClass("success");
        $("#ImprimirCita").css("display", "");
        $("#ConfirmarHorario").css("display", "none");
        $("#jsGuardado").css("display", "none");
        $("#PasoAnterior1").css("display", "none");
        $("#DivCaptcha").html("");

        jsGetEncuentaSatisfaccion(400);// Inicia la encuesta de satisfaccion

        //Se cambia el mensaje de I_Confirmacion una vez agendada la cita,
        //para advertir al usuario de asistir con la documentacion
        $("#I_Confirmacion").html(G_MENSAJES.SOLICITUD_COMPLETA);

        // Se notifica el lugar donde fue agendada al cita, se cambia el mensaje anterior
        $("#I_DireccionCita").html(G_MENSAJES.MENSAJE_DIRECCION_CITA_AGENDADA);

        //Descarga del comprbante de cita automaticamente
        ImprimirCita();
    }


}

function jsPrevio(html) {
    $("#VistaEspecial").html(html);
}

function RecargaSelect() {
    var modelo = {};
    $("#RegistrarCita").attr("disabled", "disabled");
    modelo.Concepto = $("#PalabraClave").val();
    jsAjaxGenerico("../Catalogo/GetBusqueda", modelo, null, jsRespuestaBusqueda);
}

function jsRespuestaBusqueda(html) {

    $("#TramiteUnidadAtencionId").val(html.TramiteUnidadAtencionId);
    RecargaUnidadMedica();

    $("#UnidadMedicaId").val(html.UnidadMedicaId);

    CambioTramite();
}

function CambioTramite() {

    //alert("Cambio tramite");
    if ($("#TramiteUnidadAtencionId").val() !== 0) {

        //$("#MesAnio").val(0);
        var modelo = {};
        modelo.TramiteUnidadAtencionId = $("#TramiteUnidadAtencionId").val();

        jsAjaxGenerico("../Catalogo/GetBusquedaHorarioUADyCS", modelo, null, jsConfigPicker);

    } else {

       $("#MesAnio").val(0);
       $("#MesAnio").attr("disabled", "disabled");
    }

    BloqueaUnidadMedicaMesAnio();
}

function BloqueaUnidadMedicaMesAnio()
{

    if ($("#TramiteUnidadAtencionId").val() == 0) {
        $("#UnidadMedicaId").attr("disabled", "disabled");
        $("#MesAnio").attr("disabled", "disabled");
    } else {
        $("#UnidadMedicaId").attr("disabled", null);
        $("#MesAnio").removeAttr("disabled");
    }
}

function CambioUnidadMedica() {

    if (!$("#UnidadMedicaId").val() == 0) {
        var UnidadAtencionIdCorrespondiente = $("#UnidadMedicaId option:selected").data("auxid");
        if (UnidadAtencionIdCorrespondiente != 0)
        {
            var tramUni_Actual = $("#TramiteUnidadAtencionId").val();
            $("#TramiteUnidadAtencionId").val(UnidadAtencionIdCorrespondiente);

            if (tramUni_Actual != UnidadAtencionIdCorrespondiente) {
                CambioTramite();// Si desea dear de recargarse debe cambiarse <GetBusquedaHorarioUADyCS>
                BuscarHorario();// Se recarga el calendario
            }

        }
    }

    BloqueaUnidadMedicaMesAnio();
}

function RecargaUADyCS() {
    var modelo = {};
    modelo.SinTodos = false;
    jsAjaxGenerico("../Catalogo/GetUADyCS", modelo, "TramiteUnidadAtencionId");
}

function jsGetHorarioModelo() {

    if (G_ModeloAgenda.TramiteUnidadAtencionId != undefined && G_ModeloAgenda.TramiteUnidadAtencionId !== 0) {
        $("#TramiteUnidadAtencionId").val(G_ModeloAgenda.TramiteUnidadAtencionId);
    }
    if (G_ModeloAgenda.UnidadMedicaId != undefined && G_ModeloAgenda.UnidadMedicaId !== 0) {
        RecargaUnidadMedica();
        $("#UnidadMedicaId").val(G_ModeloAgenda.UnidadMedicaId);
    }
    //alert("Mes año: " + G_ModeloAgenda.MesAnio)
    if (G_ModeloAgenda.MesAnio != undefined && G_ModeloAgenda.MesAnio != 0) {//MFP 29/05/2017 para evitar validacion sin presionar siguiente
        CambioTramite();
        $("#MesAnio").val(0);
        $("#MesAnio").val(G_ModeloAgenda.MesAnio);
        BuscarHorario();
        $("#MesAnio").removeAttr("disabled");
    } else {
        if ($("#TramiteUnidadAtencionId").val() != 0 ) {
            $("#MesAnio").removeAttr("disabled");
            CambioTramite();
        }
    }
}

function RecargaUnidadMedica() {

    var modelo = jsGetModelo("RegistroCita");
    modelo.CatTipoTramiteId = parseInt($("#IdCatTipoSistema").html());

    if ($("#PalabraClave").val().length >= 3) {
        modelo.PalabraBusqueda = $("#PalabraClave").val();
        modelo.TramiteUnidadAtencionId = 0;

    }

    jsAjaxGenerico("../Catalogo/GetTramiteUnidadMedica", modelo, "UnidadMedicaId");

}



function PasoAnterior1() {
    jsAjaxGenerico("../Promovente/Horario", null, "SeccionTrabajoGeneral", null, false);
    AnimarScrollTop();//MFP 30/05/2017
}

function PasoSiguiente() {

    var valor = ValidaPromoventePaciente(true);
    if (valor || valor == null) {
        AccionActiva("AgendaCita");
        getModeloAgendaDatosIniciales();
        jsAjaxGenerico("../Promovente/Horario", null, "SeccionTrabajoGeneral", null, false);
        BreadCrumb("Agendar Cita "); 
    }

    AnimarScrollTop();
}

function jsGuardado() {


    $("#IdReCaptcha").val($("#g-recaptcha-response").val());
    var valor = ValidaModeloGenerico("reCaptchaContainer", true);
    if (valor === false) return;

    G_ModeloAgenda.RespuestaCaptcha = $("#g-recaptcha-response").val();
    G_ModeloAgenda.CatTipoTramiteId = parseInt($("#IdCatTipoSistema").html());
    jsAjaxGenerico("../Promovente/AgendarCita", G_ModeloAgenda, null, jsResultadoAgenda);
    /*/// jsAjaxGenerico("../Promovente/AgendarCita", modelo, null, jsResultadoAgenda);
    /// return;

    $('#MedicionFormulario').submit(function () {
        ///jsAjaxGenerico($(this).attr('action'), G_ModeloAgenda, null, jsResultadoAgenda);
        jsAjaxGenerico("../Promovente/AgendarCita", G_ModeloAgenda, null, jsResultadoAgenda);
        //$.post($(this).attr('action'), Modelo, function (json) {
        //    jsResultadoAgenda(json);
        //}, 'json');
        return false;
    });

    var form = $('#MedicionFormulario');
    form.attr('action', "../Promovente/AgendarCita");
    form.attr('method', 'GET');
    form.submit();*/
}

function jsChangeTramiteUnidadAtencion() {
    $("#SeccionCalendario").html("");
    $("#RegistrarCita").attr("disabled", "disabled");

    BloqueaUnidadMedicaMesAnio();
}

function jsCheckUpPaciente() {

    if ($("#PacienteCheckSi").hasClass("glyphicon-unchecked")) {
        $("#PacienteCheckSi").removeClass("glyphicon-unchecked");
        $("#PacienteCheckSi").addClass("glyphicon-check");
        $("#PacienteCheckNo").removeClass("glyphicon-check");
        $("#PacienteCheckNo").addClass("glyphicon-unchecked");
    } else {
        $("#PacienteCheckSi").removeClass("glyphicon-check");
        $("#PacienteCheckSi").addClass("glyphicon-unchecked");
        $("#PacienteCheckNo").removeClass("glyphicon-unchecked");
        $("#PacienteCheckNo").addClass("glyphicon-check");
    }

    $("#IndexRegionMensaje").html("");

    var esRepresentante = $("#PacienteCheckSi").hasClass("glyphicon-check");

    $("#TabPromovente").addClass("active");
    $("#TabPromovente").removeClass("hidden");
    $("#SeccionPromovente").addClass("active");
    $("#SeccionPromovente").removeClass("hidden");
    $("#TabPromoventeDiv").addClass("active");
    $("#TabPromoventeDiv").removeClass("hidden");

    $("#TabPaciente").removeClass("active");
    $("#SeccionPaciente").removeClass("active");
    $("#TabPacienteDiv").removeClass("active");

    if (esRepresentante) {
        $("#TabPaciente").removeClass("hidden");
        $("#SeccionPaciente").removeClass("hidden");
        $("#TabPacienteDiv").removeClass("hidden");
    } else {

        $("#TabPaciente").addClass("hidden");
        $("#SeccionPaciente").addClass("hidden");
        $("#TabPacienteDiv").addClass("hidden");
        LimpiarModelo("SeccionPaciente");
    }
}