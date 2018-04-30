
var indice = 0, NumeroSecciones = 0;
var Seccion, IdDiv, NameFichero;
var Homoclave;

function Resultado() {
    var aux = indice % NumeroSecciones;
    return aux *= (indice < 0) ? (-1) : 1;
}

function Anterior() {
    $(".Indice").css("display", "none");
    $("#Indice" + Seccion + (Resultado(indice--))).fadeTo(1000, 3.0);
}

function Siguiente() {
    $(".Indice").css("display", "none");
    $("#Indice" + Seccion + (Resultado(indice++))).fadeTo(1000, 3.0);
}

function SeccionShow(prefijo) {
    NumeroSecciones = parseInt($("#NumeroSecciones" + prefijo).html());
}

function BreadCrumb(leyenda,callBackFunction) {

    var idCatTipoSistema = $("#IdCatTipoSistema").html(); 

    $("#navegacionSeccionInicio").prop("href", "../Promovente/Index?CatTipoTramiteId=" + idCatTipoSistema);

    if (IndexOf(leyenda.toUpperCase(), "INICIO") >= 0) {
        $("#navegacionSeccion").parent("li").hide();
        $("#navegacionSeccion").hide();
    } else {
        $("#navegacionSeccion").parent("li").show();
        $("#navegacionSeccion").show();
        $("#navegacionSeccion").html("<span class=''></span>" + leyenda);
        $("#navegacionSeccion").attr("cursor", "none");

        if (callBackFunction != null && callBackFunction != undefined)
            $("#navegacionSeccion").attr("onclick", callBackFunction.name + "()");

    }

    registraSeccion(leyenda); // DIGITAL ANALITIX

}

function MuestraZona(zona) {
    $("#" + zona).fadeTo(1000, 3.0);
}

function ImpresionPage1() {

    var html = $("#Body" + Seccion).html();
    html = html.replace(/height:600px;/g, "");
    html = html.replace(/height:650px;/g, "");
    html = html.replace(/height:86%;/g, "width:100%");
    html = html.replace(/height: 650px;/g, "");
    html = html.replace(/height: 86%;/g, "width:100%");
    html = html.replace(/height: 600px;/g, "");
    html = html.replace(/border:1px solid black;/g, "");
    html = html.replace(/border: 1px solid black;/g, "");
    html = html.replace(/<h5/g, "<div class='row' style='height:50px'>&nbsp;</div><h5");

    html = escape(html);
    jsCrearPDF(null, html, NameFichero,0);
}

function EliminarCita() {

    $("#IdReCaptcha").val($("#g-recaptcha-response").val());

    var modelo = {};
    modelo.SolicitudId = $("#IdSolicitud").html();
    modelo.RespuestaCaptcha = $("#g-recaptcha-response").val();

    jsAjaxGenerico("../Promovente/CancelarCita", modelo, null, jsResultDeleteReactive);
    $("#EliminarCita").css("display", "none");
}

function ConsultarSolicitud() {

    $("#ImprimirCita").css("display", "none");
    $("#EliminarCita").css("display", "none");
    $("#jsCrearFormaPDF").css("display", "none");    


    $("#IdReCaptcha").val($("#g-recaptcha-response").val());
    var valor = ValidaModeloGenerico("ZonaConsultaRecordatorio", true);
    if (valor === false) return;

    var modelo = jsGetModelo("ZonaConsultaRecordatorio");
    modelo.RespuestaCaptcha =  $("#g-recaptcha-response").val();

    G_citaFolio = modelo.NumeroFolio;
    jsAjaxGenerico("../Promovente/VerSolicitud", modelo, null, jsVerSolicitud);
    Recaptcha();

    AnimarScrollTop();//MFP 30/05/2017
}

function jsVerSolicitud(html) {
    if (IndexOf(html,"ERROR") >= 0) {
        html = html.replace("ERROR: ", "");
        jsResultError(html, true);
        OcultaBoton("EliminarCita");
        OcultaBoton("jsCrearFormaPDF");
    } else {

        $("#SeccionConsulta").hide();
        $("#I_MensajeCancelacion").remove();
        $("#SeccionVerSolicitud").html(""); //MFP 29-05-2017
        $("#SeccionVerSolicitud").html(html);
        MuestraBoton("ImprimirCita");
        MuestraBoton("jsCrearFormaPDF");
        MuestraBoton("EliminarCita");
        $("#I_DireccionCita").html(G_MENSAJES.MENSAJE_DIRECCION_CITA_AGENDADA);
        $("#I_MensajeConsulta").html(G_MENSAJES.MENSAJE_INFORMACION_CITA_CONSULTADA);
        
    }
}

function ImprimirCita() {

    $("#ZonaImpresion").remove();
    $("body").append('<div id="ZonaImpresion" style="display:none;"></div>');
    
    modelo = { NumeroFolio: G_citaFolio }; 
     
    jsAjaxGenerico("../Promovente/VerSolicitudCompleta", modelo, "ZonaImpresion");

    var html = $("#ZonaImpresion").html();
    html = escape(html);
    $("#ZonaImpresion").remove();

   jsCrearPDF(null, html, "Cita_" + G_citaFolio, 1);
 
}


function ImprimirCitaGeneral(NumeroFolioParam) {

    $("#ZonaImpresion").remove();
    $("body").append('<div id="ZonaImpresion" style="display:none;"></div>');

    var modelo = { NumeroFolio: NumeroFolioParam }; 

    jsAjaxGenerico("../Administrador/VerSolicitudCompleta", modelo, "ZonaImpresion");
    
    $("#VSC").remove();
    $("#I_DireccionCitaAgendada").remove();
    $("#I_SolicitudCompleta").remove();
    
    var html = $("#ZonaImpresion").html();

    html = escape(html);
    $("#ZonaImpresion").remove();

    jsCrearPDF(null, html, "Cita_" + NumeroFolioParam, 1);

}


////////////////////////////----------------------------------------------------------------------------------------------------------

function VerSolicitud() {
    G_ModeloAgenda = {};
    AccionActiva("VerSolicitud");
    jsAjaxGenerico("../Promovente/Consulta", null, "SeccionTrabajoGeneral", null, false);
    BreadCrumb("Consultar Cita ");
    Recaptcha();
}

function AccionActiva(id) {

    $(".EventMain").removeClass("btn-primary");
    $(".EventMain").addClass("btn-default");
    $("#" + id).removeClass("btn-default");
    $("#" + id).addClass("btn-primary");

}

function CancelaCita() {

    G_ModeloAgenda = {};
    AccionActiva("CancelaCita");
    jsAjaxGenerico("../Promovente/Cancelacion", null, "SeccionTrabajoGeneral", null, false);
    BreadCrumb("Cancelar cita ");
    Recaptcha();
}

function AcercaTramite() {

    AccionActiva("AcercaTramite");
    var modelo = { CatTipoTramiteId: parseInt($("#IdCatTipoSistema").html()) };
    jsAjaxGenerico("../Promovente/Informacion", modelo, "SeccionTrabajoGeneral", null, false); // MFP 31/07/2017
    ConfiguraInfo("Inicio ", "P", "SeccionInicio", "Indicaciones");
    BreadCrumb("Inicio");
    Recaptcha();
}

function InformacionAdicional() {

    AccionActiva("InformacionAdicional");
    jsAjaxGenerico("../Promovente/InformacionAdicional", null, "SeccionTrabajoGeneral", null, false);
    ConfiguraInfo("Más Información ", "S", "SeccionAdicional", "Adicionales");
    BreadCrumb("Más Información");

}

function ConfiguraInfo(secc, prefijo, divImpresion, fichero) {

    $(".Indice").css("display", "none");
    //BreadCrumb(secc);
    Seccion = prefijo;
    indice = 0;
    IdDiv = divImpresion;
    NameFichero = fichero;
    MuestraZona(IdDiv);
    $("#Indice" + prefijo + "0").fadeTo(1000, 3.0);
    SeccionShow(prefijo);
}

function jsConfigPicker(fechaLimite) {

    fechaLimite = fechaLimite.split(",");
    var anioFin = parseInt(fechaLimite[0]);
    var mesFin = parseInt(fechaLimite[1]);

    var meses = [
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre",
        "Diciembre"
    ];

    var mesActual = new Date().getMonth();
    mesActual = mesActual + 1;
    var anioActual = new Date().getFullYear();

    var cadena = "<option value='0'>Selecciona</option>";

    var i = anioActual;
    var j = mesActual;
    while (true) {

        var aux = meses[j - 1] + " " + anioFin;
        cadena += "<option value='" + aux + "'>" + aux + "</option>";
        j++;
        if (j > 12) {
            j = 1;
            i++;
        }
        if (i == anioFin && j >= mesFin) {
            break;
        }
    }
    $("#MesAnio").html(cadena);
}

function jsCrearFormaPDF() {

    var url = "../Forma/jsCrearPDFTramite";

    $("#formaPDF").remove();
    $("body").append('<form action="' + url + '" method="post" id="formaPDF" name="formaPDF"></form>');

    var form = $("#formaPDF");
    form.attr("action", url);
    form.attr("method", "POST");
    form.submit();
}

function jsCrearFormaDOCX() {
    window.location = "../Forma/jsCrearDOCXTramite";
}

function jsResultGetHomoclave(data) {
    Homoclave = data;
}

