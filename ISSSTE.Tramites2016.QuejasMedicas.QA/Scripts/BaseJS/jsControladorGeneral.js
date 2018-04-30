var G_MENSAJES = {};
var G_NoEsUsuarioBloqueado = true;
var G_ConSpinner = true;
var G_ModeloAgenda = {};
var G_contenedorMensaje = "IndexRegionMensaje";
var G_citaFolio = "";

/*var G_MENSAJES = {
    CURP_VALIDO: "Favor de revisar que el CURP sea válido",
    CAMPOS_OBLIGATORIOS: "No ha llenado varios campos requeridos. Por favor verifique. ",
    INYECCION_HTML: "Favor de revisar la información capturada, se detectó Inyección HTML",
    MES_CURSO: "El mes de consulta debe estar en curso o ser posterior al actual",
    NUMERO_SOLICITUD: "Esta etapa de tu trámite ha finalizado satisfactoriamente. El número de folio de su solicitud es: ",
    CORREO_VALIDO: "Favor de revisar que el correo sea válido",
    TOOLTIP_HORARIO: "Click para seleccionar el horario"
}*/

function Recaptcha() {
    var modelo = {};
    jsAjaxGenerico("../Account/Captcha", modelo, "DivCaptcha", null);
}

function IndexOf(Frase, Palabra, pos) {
    if (Frase == null || Frase == undefined) return -1;
    if (Palabra == null || Palabra == undefined) return -1;
    try {
        Frase = new String(Frase);
        Palabra = new String(Palabra);

        if (pos != null && pos != undefined) {
            return Frase.indexOf(Palabra, pos);
        }
        return Frase.indexOf(Palabra);
    } catch (ex) {
        alert("Fallo en función nativa indexOf");
        if (pos != null && pos != undefined) {
            Frase = Frase.substring(pos);
        }
        String.prototype.indexOf = function (prefix) {
            valor = this.match(prefix + "$") == prefix;
            if (valor == true) return 0;
            else return -1;
        }
        try {
            return Frase.indexOf(Palabra, pos);
        } catch (ex) {
            alert("Fallo al hacer polimorfismo con match");
            return -1;
        }
    }
    return -1;
}

function detectIE() {
    var ua = window.navigator.userAgent;

    var msie = IndexOf(ua, 'MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, IndexOf(ua, '.', msie)), 10);
    }

    var trident = IndexOf(ua, 'Trident/');
    if (trident > 0) {
        // IE 11 => return version number
        var rv = IndexOf(ua, 'rv:');
        return parseInt(ua.substring(rv + 3, IndexOf(ua, '.', rv)), 10);
    }

    var edge = IndexOf(ua, 'Edge/');
    if (edge > 0) {
        // Edge (IE 12+) => return version number
        return parseInt(ua.substring(edge + 5, IndexOf(ua, '.', edge)), 10);
    }

    // other browser
    return false;
}

function MuestraDetallePanel(id) {

    $(".GeneralCuerpo").css("display", "none");
    $("#panel_" + id).css("display", "inline");

    $(".HeaderBtn").css("background-color", "#F6F6F6");
    $(".HeaderBtn").css("color", "black");

    $("#" + id).css("background-color", "#4D92DF");
    $("#" + id).css("color", "white");
}

function CargarMensajes(objetos) {
    for (var i = 0; i < objetos.length; i++) {
        G_MENSAJES[objetos[i].Informacion] = objetos[i].Valor;
    }
}

$(document).ready(function () {
    jsEvaluarElementos();

    var height = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;
    height = height + 1200;

    $(".load-ui-QM").remove();
    ///$("body").append('<div class="load-ui-QM" style="height:' + height + 'px;z-index:9999"><div class="backdrop-ui-QM"></div><div class="cargando-ui-QM"><img src=" ../Images/Administrator/loading.gif" /></div></div>');

    $("body").append('<div class="load-ui-QM" style="height:' +
        height +
        'px;z-index:9999"><div class="backdrop-ui-QM"></div><div data-ng-controller="workingonit as vm" data-ng-show="vm.isWorking" class="dialog-container dissolve-animation">' +
        '<div class="dialog-message">' +
        '<img src="data:image/gif;base64,R0lGODlhGAAYAJECAP///5mZmf///wAAACH/C05FVFNDQVBFMi4wAwEAAAAh+QQFCgACACwAAAAAGAAYAAACQJQvAGgRDI1SyLnI5jr2YUQx10eW5hmeB6Wpkja5SZy6tYzn+g5uMhuzwW6lFtF05CkhxGQm+HKuoDPplOlDFAAAIfkEBQoAAgAsFAAGAAQABAAAAgVUYqeXUgAh+QQFCgACACwUAA4ABAAEAAACBVRip5dSACH5BAUKAAIALA4AFAAEAAQAAAIFVGKnl1IAIfkEBQoAAgAsBgAUAAQABAAAAgVUYqeXUgAh+QQFCgACACwAAA4ABAAEAAACBVRip5dSACH5BAUKAAIALAAABgAEAAQAAAIFVGKnl1IAIfkECQoAAgAsBgAAAAQABAAAAgVUYqeXUgAh+QQJCgACACwAAAAAGAAYAAACJZQvEWgADI1SyLnI5jr2YUQx10eW5omm6sq27gvH8kzX9o3ndAEAIfkECQoAAgAsAAAAABgAGAAAAkCULxFoAAyNUsi5yOY69mFEMddHluYZntyjqY3Vul2yucJo5/rOQ6lLiak0QtSEpvv1lh8l0lQsYqJHaO3gFBQAACH5BAkKAAIALAAAAAAYABgAAAJAlC8RaAAMjVLIucjmOvZhRDHXR5bmGZ7co6mN1bpdsrnCaOf6zkOpzJrYOjHV7Gf09JYlJA0lPBQ/0ym1JsUeCgAh+QQJCgACACwAAAAAGAAYAAACQJQvEWgADI1SyLnI5jr2YUQx10eW5hme3KOpjdW6XbK5wmjn+s5Dqcya2Dox1exn9PSWJeRNSSo+cR/pzOSkHgoAIfkECQoAAgAsAAAAABgAGAAAAkCULxFoAAyNUsi5yOY69mFEMddHluYZntyjqY3Vul2yucJo5/rOQ6nMmtg6MdXsZ/T0liXc6jRbOTHR15SqfEIKACH5BAkKAAIALAAAAAAYABgAAAJAlC8RaAAMjVLIucjmOvZhRDHXR5bmGZ7co6mN1bpdsrnCaOf6zkO4/JgBOz/TrHhC9pYRpNJnqURLwtdT5JFGCgAh+QQJCgACACwAAAAAGAAYAAACPpQvEWgADI1SyLnI5jr2YUQx10eW5jme3NOpTWe5Qpu6tYzn+l558tWywW4lmk/IS6KOr2UtSILOYiYiUVAAADs=">' +
        '<span class="ms-accentText dialog-accentText">' +
        "&nbsp;Trabajando en ello..." +
        "</span>" +
        "</div>" +
        "</div></div></div>");

    $(document).ajaxStart(function () {
        if (G_ConSpinner) {
            //alert("start spiner");
            $(".load-ui-QM").show();
        }
    });

    $(document).ajaxStop(function () {
        if (G_ConSpinner) {
            //alert("stop spiner");
            $(".load-ui-QM").hide();
            G_ConSpinner = false;
        }
    });
});

function jsBase() {

    var idBusqueda = "body";

    $(idBusqueda + " input[type='text']").each(function () {
        var color = $(this).css("border-color");
    });

    $(idBusqueda + " select").each(function () {
        var color = $(this).css("border-color");
    });
}

function jsEvalElements(objeto) {
    var displayDiv = $("#div_" + objeto.attr("id")).css("display");
    var claseCampo = objeto.attr("class");
    claseCampo = claseCampo.toUpperCase();
    var esCampoInvalido = false;

    if (displayDiv == undefined) {
        displayDiv = "";      
    }

    if (claseCampo.search("ESCAMPONOVALIDO_CSS") >= 0 || claseCampo.indexOf("ESCAMPONOVALIDO_CSS")>=0) {
        esCampoInvalido = true;
    }

    displayDiv = displayDiv.toUpperCase();

    if (displayDiv === "BLOCK" || esCampoInvalido) 
        return true;
     else return false;
}

function jsEvaluarElementos() {
    $("body input[type='text']").each(function () {
        if ($(this).hasClass("Numero")) {
            jsEventValidarNumeros($(this).attr("id"));
        }
        if ($(this).hasClass("LetraMayus", true)) {
            jsEventValidarLetras($(this).attr("id"), true);
        }
        if ($(this).hasClass("EnMayus", true)) {
            jsEventValidarLetrasNumero($(this).attr("id"), true);
        }
        if ($(this).hasClass("Letra", true)) {
            jsEventValidarLetras($(this).attr("id"), null);
        }
        if ($(this).hasClass("LetraNumero", true)) {
            jsEventValidarLetrasNumero($(this).attr("id"), false);
        }

        $(this).keyup(function () {
            if (jsEvalElements($(this))) {ValidaPromoventePaciente(); } 
        });

    });
}

function jsEventValidarLetrasNumero(referencia, conEspacio) {
    try {
        $("#" + referencia).change(function () {
            jsSoloLetraNumero(referencia, conEspacio);
        });
        $("#" + referencia).keyup(function () {
            jsSoloLetraNumero(referencia, conEspacio);
        });
        $("#" + referencia).keypress(function () {
            jsSoloLetraNumero(referencia, conEspacio);
        });
        $("#" + referencia).keydown(function () {
            jsSoloLetraNumero(referencia, conEspacio);
        });
    } catch (error) {
    }
}

function jsEventValidarLetras(referencia, conMayusculas) {
    try {
        $("#" + referencia).change(function () {
            jsSoloLetra(referencia, conMayusculas);
        });
        $("#" + referencia).keyup(function () {
            jsSoloLetra(referencia, conMayusculas);
        });
        $("#" + referencia).keypress(function () {
            jsSoloLetra(referencia, conMayusculas);
        });
        $("#" + referencia).keydown(function () {
            jsSoloLetra(referencia, conMayusculas);
        });
    } catch (error) {
    }
}

function jsEventValidarNumeros(referencia) {
    try {
        $("#" + referencia).change(function () {
            jsSoloEntero(referencia);
        });
        $("#" + referencia).keyup(function () {
            jsSoloEntero(referencia);
        });
        $("#" + referencia).keypress(function () {
            jsSoloEntero(referencia);
        });
        $("#" + referencia).keydown(function () {
            jsSoloEntero(referencia);
        });
    } catch (error) {
    }
}

function jsSoloLetra(id, conMayusculas) {
    var cadena = $("#" + id).val();
    $("#" + id).val("");

    if (conMayusculas) {
        cadena = cadena.toUpperCase();
        cadena = cadena.replace(/ä/g, "A");
        cadena = cadena.replace(/ë/g, "E");
        cadena = cadena.replace(/ï/g, "I");
        cadena = cadena.replace(/ö/g, "O");
        cadena = cadena.replace(/ü/g, "U");
        cadena = cadena.replace(/Ä/g, "A");
        cadena = cadena.replace(/Ë/g, "E");
        cadena = cadena.replace(/Ï/g, "I");
        cadena = cadena.replace(/Ö/g, "O");
        cadena = cadena.replace(/Ü/g, "U");
        cadena = cadena.replace(/á/g, "A");
        cadena = cadena.replace(/é/g, "E");
        cadena = cadena.replace(/í/g, "I");
        cadena = cadena.replace(/ó/g, "O");
        cadena = cadena.replace(/ú/g, "U");
        cadena = cadena.replace(/Á/g, "A");
        cadena = cadena.replace(/É/g, "E");
        cadena = cadena.replace(/Í/g, "I");
        cadena = cadena.replace(/Ó/g, "O");
        cadena = cadena.replace(/Ú/g, "U");
        cadena = cadena.replace(/  /g, " ");
    }
    if (conMayusculas == null) {
        cadena = cadena.replace(/ /g, "");
    }

    cadena = cadena.replace(/[^a-zA-Záéíóú ñÁÉÍÓÚÑ]/g, "");

    $("#" + id).val(cadena);
    return cadena;
}

function jsSoloLetraNumero(id, conEspacios) {
    var cadena = $("#" + id).val();
    $("#" + id).val("");

    cadena = (conEspacios) ? cadena.replace(/[^a-zA-Z 0-9\s]/g, "") : cadena.replace(/[^a-zA-Z0-9\s]/g, "");
    cadena = cadena.toUpperCase();
    if (!conEspacios) {
        cadena = cadena.replace(/ /g, "");
    }

    $("#" + id).val(cadena);
    return cadena;
}

function jsSoloEntero(id) {
    var num = $("#" + id).val();
    $("#" + id).val("");
    num = num.replace(/[^\d\,]*/g, "");
    num = num.replace(/\,/g, "");
    $("#" + id).val(num);
    return num;
}

function jsValidaCorreoElectronico(correoElectronico, id) {
    // var patron = /^([a-z]+[a-z1-9._-]*)@{1}([a-z1-9\.]{2,})\.([a-z]{2,3})$/;
    // if (correoElectronico.search(patron)) {

    var validacion =
    (
        /^(([^<>()[\]\.,;:\s@\"]+(\.[^<>()[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()[\]\.,;:\s@\"]+\.)+[^<>()[\]\.,;:\s@\"]{2,})$/i
            .test(correoElectronico));
    if (!validacion) {
        $("#" + id).css("border-color", "#D0021B");
        OnFailure("AgendarCitaForm", id, "El correo no es valido"); //MFP 11/04/2017
        return false;
    }

    $("#" + id).css("border-color", "#DDDDDD");
    return true;
}

//// AAAA######AAAAAAA##
//
function MarcadoCURPIncorrecto(id, conFocusCURP) {

    var curp = $("#" + id).val();

    $("#" + id).css("border-color", "#D0021B");   

    if (curp.length >= 18) {
        jsResultError(G_MENSAJES.CURP_VALIDO, conFocusCURP);
        OnFailure("AgendarCitaForm", id, "El CURP no es valido"); //MFP 11/04/2017
    }else {
        //alert(id)
        $("#div_Novalido" + id).removeClass("hidden");
        $("#div_Novalido" + id).css("color", "#D0021B");
        $("#" + id).css("border-color", "#D0021B");   
    }

    return false;
}

function jsValidaCURP(id,conFocusCURP) {

    var curp = $("#" + id).val();

    if (curp === "") {
        $("#" + id).css("border-color", "#DDDDDD");
        $("#div_Novalido" + id).addClass("hidden");
        return true;
    }

    var esCorrecto = curp.match(/^([a-z]{4})([0-9]{6})([a-z]{7})([0-9]{1})$/i) ||
        curp.match(/^([a-z]{4})([0-9]{6})([a-z]{6})([0-9]{2})$/i);

    if (!esCorrecto) {
        MarcadoCURPIncorrecto(id,conFocusCURP);
        return false;
    }

    esCorrecto = ValidacionFechaCURP(curp,conFocusCURP);
    if (!esCorrecto) {
        MarcadoCURPIncorrecto(id);
        return false;
    }

    var sexo = curp[10];
    if (sexo !== "H" && sexo !== "M") {
        MarcadoCURPIncorrecto(id, conFocusCURP);
        return false;
    }

    var valor = $("#" + id).val();

    var prefijo = valor[17];
    valor = valor.substring(4, 10);
    var mes;
    var mesAux = mes = valor.substring(2, 4);
    var dia = valor.substring(4, 6);
    valor = valor.substring(0, 2);
    var aNo = (is_numeric(prefijo)) ? "19" + valor : "20" + valor;

    if (is_numeric(aNo) && is_numeric(mes) && is_numeric(dia)) {
        var fechaNacimiento = new Date(aNo, mes - 1, dia);
        var hoy = new Date();
        if (fechaNacimiento > hoy) {
            MarcadoCURPIncorrecto(id,conFocusCURP);
            return false;
        }
        mes = "" + (fechaNacimiento.getMonth() + 1);
        if (mes.length === 1) mes = "0" + mes;
        if (mesAux !== mes) {
            MarcadoCURPIncorrecto(id, conFocusCURP);
            return false;
        }
    } else {
        MarcadoCURPIncorrecto(id,conFocusCURP);
        return false;
    }

    $("#" + G_contenedorMensaje).html("");
    $("#" + id).css("border-color", "#DDDDDD");
    $("#div_Novalido" + id).addClass("hidden");


    $("#S_" + id).css("color", "black");
    $("#S_" + id).addClass("hidden");


    return true;
}

function ValidacionFechaCURP(curp) {

    var valor = curp.substring(4, 10);
    ///var anio = parseInt(valor.substring(0, 2));
    var mes = parseInt(valor.substring(2, 4));
    var dia = parseInt(valor.substring(4, 6));
    if (dia < 0 || dia > 31) {
        return false;
    }
    if (mes < 0 || mes > 12) {
        return false;
    }
    return true;
}

function OcultaBoton(idDiv) {
    $("#" + idDiv).css("display", "none");
    $("#" + idDiv).css("opacity", 0);
}

function MuestraBoton(idDiv) {
    $("#" + idDiv).css("display", "none");
    $("#" + idDiv).css("opacity", 1);
    $("#" + idDiv).fadeIn("slow");
}

function CampoNormal(elemento) {
    var id = elemento.attr("id");
    id = "div_" + id;
    elemento.css("border-color", "#DDDDDD");
    $("#" + id).addClass("hidden");
    $("#S_" + id).css("color", "black");
    $("#div_Novalido" + id).addClass("hidden");

    elemento.removeClass("ESCAMPONOVALIDO_CSS");
}

function CampoVacio(elemento) {

    clase = elemento.attr("class");
    clase = clase.toUpperCase();

    var id = elemento.attr("id");
    id = "div_" + id;
    elemento.css("border-color", "#D0021B");
    $("#S_" + id).css("color", "#D0021B");

    if (clase.search("SINMENSAJE") < 0 || clase.indexOf(clase, "SINMENSAJE") < 0)
    {
        $("#" + id).removeClass("hidden");
        $("#div_Novalido" + id).addClass("hidden");
        $("#" + id).css("color", "#D0021B");
    }

    elemento.addClass("ESCAMPONOVALIDO_CSS");

    OnFailure("AgendarCitaForm", id, "Este campo es obligatorio"); //MFP 11/04/2017
}

//MFP 11/04/2017
function OnFailure(form, id, message) {
    //if ((form == null || form == undefined) || (id == null || id == undefined)){return;}    
    try {
        ns_.Form.onFailure(form, id, message);

    } catch (ex) {

    }
}

/// ConHeaderFooter --> 0 sin, 1 con
function jsCrearPDF(idDiv, html, nameFichero, ConHeaderFooter) {

    if (ConHeaderFooter == null || ConHeaderFooter == undefined) ConHeaderFooter = 0;

    if (idDiv != null) {
        html = $("#" + idDiv).html();
    }
    var url = "../Forma/jsCrearPDF";

    if (html != null && html !== "") {
        $("#formaPDF").remove();
        $("body").append('<form action="' + url + '" method="post" id="formaPDF" name="formaPDF"></form>');

        html = html.replace(/null/g, "");

        if (idDiv != null) {
            html = escape(html);
        }

        var form = $("#formaPDF");
        form.attr("data", { 'html': html });
        form.append(JsParametroEnvio("inputHTML", html));
        form.append(JsParametroEnvio("ConHeaderFooter", ConHeaderFooter));
        form.append(JsParametroEnvio("nameFichero", nameFichero));
        form.attr("action", url);
        form.attr("method", "POST");
        form.submit();
    }
}

function jsCrearTXT(idDiv, html, nameFichero) {

    if (idDiv != null) {
        html = $("#" + idDiv).html();
    }
    var url = "../Forma/jsCrearTXT";

    if (html != null && html !== "") {
        $("#formaTXT").remove();
        $("body").append('<form action="' + url + '" method="post" id="formaTXT" name="formaTXT"></form>');

        html = html.replace(/null/g, "");

        if (idDiv != null) {
            html = escape(html);
        }

        var form = $("#formaTXT");
        form.attr("data", { 'html': html });
        form.append(JsParametroEnvio("inputHTML", html));
        form.append(JsParametroEnvio("nameFichero", nameFichero));
        form.attr("action", url);
        form.attr("method", "POST");
        form.submit();
    }
}

function jsCrearXLS(idDiv, html, nameFichero) {

    if (idDiv != null) {
        html = $("#" + idDiv).html();
    }
    var url = "../Forma/jsCrearXLS";

    if (html != null && html !== "") {
        $("#formaXLS").remove();
        $("body").append('<form action="' + url + '" method="post" id="formaXLS" name="formaXLS"></form>');

        html = html.replace(/null/g, "");

        if (idDiv != null) {
            html = escape(html);
        }

        var form = $("#formaXLS");
        form.attr("data", { 'html': html });
        form.append(JsParametroEnvio("inputHTML", html));
        form.append(JsParametroEnvio("nameFichero", nameFichero));
        form.attr("action", url);
        form.attr("method", "POST");
        form.submit();
    }
}

function JsParametroEnvio(id, valor) {
    var inputHtml = $('<input type="hidden" name="' + id + '" id="' + id + '" value="" />');
    inputHtml.val(valor);
    return inputHtml;
}

function jsEsValorDefinido(valor) {
    if (valor == undefined) return false;
    return true;
}

function jsEsCampoConValor(valor) {
    if (!jsEsValorDefinido(valor)) return false;
    if (valor.length === 0) return false;
    try {
        if (valor.trim().length === 0) return false;
    } catch (error) {
    }
    return true;
}

function EsObligatorio(elemento) {

    var clase = GetAttrUpper(elemento, "class");
    var disabled = GetAttrUpper(elemento, "disabled");
    var oculto = elemento.css("display");
    if (oculto == undefined) oculto = "";
    oculto = oculto.toUpperCase();

    if (IndexOf(oculto, "NONE") >= 0 && IndexOf(clase, "SINHIDDENCHECK") ==-1) {
        return false;
    }

    if (IndexOf(clase, "CAMPOOBLIGATORIO") >= 0 && disabled !== "DISABLED") {
        return true;
    }
    return false;
}

function GetAttrUpper(elemento, attr) {
    attr = elemento.attr(attr);
    if (attr == undefined) attr = "";
    attr = attr.toUpperCase();
    return attr;
}

function ValidaModeloGenerico(idBusqueda, conFocus, leyenda) {
    //alert("Validacion del modelo")
    if (idBusqueda == null) {
        idBusqueda = "body";
    } else idBusqueda = "#" + idBusqueda;

    var conError1 = ValidacionTipo(idBusqueda + " input[type='text']", 3);
    var conError2 = ValidacionTipo(idBusqueda + " textarea", 2);
    var conError3 = ValidacionTipo(idBusqueda + " select", 1);
    var conErrorHidden = ValidacionTipo(idBusqueda + " input[type='hidden']", 3);
    var conError = conError1 || conError2 || conError3 || conErrorHidden;
    if (conError) {
        leyenda = (leyenda == null) ? "" : leyenda;
        jsResultError(G_MENSAJES.CAMPOS_OBLIGATORIOS + leyenda, conFocus);
        return false;
    } else {
        $("#" + G_contenedorMensaje).html("");
    }
    return true;
}

function LimpiarModelo(idBusqueda) {

    $("#" + idBusqueda + " input[type='text']").each(function () {
        $(this).val("");
    });
    $("#" + idBusqueda + " input[type='check']").each(function () {
        $(this).prop("checked", "");
    });
    $("#" + idBusqueda + " textarea").each(function () {
        $(this).val("");
    });
    $("#" + idBusqueda + " select").each(function () {
        $(this).val("0");
    });
}

function ValidacionTipo(estereotipo, caso) {
    var conError = false;
    var i = 0;
    $(estereotipo).each(function () {
        CampoNormal($(this));
        if (EsObligatorio($(this))) {
            var valor = $(this).val();
            switch (caso) {
                case 1: /// Campo Entero
                    valor = parseInt(valor);
                    if (valor === 0) {
                        EsCampoVisibleSinValor(this);
                        conError = true;
                    }
                    break;
                case 2: /// Campo con Valor
                    if (!jsEsCampoConValor(valor)) {
                        EsCampoVisibleSinValor(this);
                        conError = true;
                    }
                    break;
                case 3: /// Moneda & Mixto
                    if (!jsEsCampoConValor(valor) || valor === "dd/mm/yyyy") {
                        EsCampoVisibleSinValor(this);
                        conError = true;
                    } else {
                        var clase = GetAttrUpper($(this), "class");
                        if (IndexOf(clase, "MONEDA") >= 0 || clase.indexOf("NUMERO") >= 0) {
                            valor = valor.replace(/$/g, "");
                            valor = valor.replace(/ /g, "");
                            valor = valor.replace(/\,/g, "");
                            if (parseFloat(valor) === 0) {
                                EsCampoVisibleSinValor(this);
                                conError = true;
                            }
                        }
                    }
                    break;
            }
        }
        i++;
    });
    return conError;
}

function EsCampoVisibleSinValor(elemento) {
    CampoVacio($(elemento));
    return $(elemento).attr("placeholder");
}

function jsGetModelo(idBusqueda) {
    if (idBusqueda == null) {
        idBusqueda = "body";
    } else idBusqueda = "#" + idBusqueda;
    var cadena = "";

    $(idBusqueda + " input[type='hidden']").each(function () {
        var idObject = $(this).attr("id");
        var valor = $(this).val();
        if ($(this).hasClass("MONEDA")) {
            valor = valor.replace(/$/g, "");
            valor = valor.replace(/ /g, "");
            valor = valor.replace(/\,/g, "");
        }
        cadena += "\"" + idObject + "\":\"" + valor + "\",";
    });

    $(idBusqueda + " input[type='text']").each(function () {
        var idObject = $(this).attr("id");
        var valor = $(this).val();
        if ($(this).hasClass("MONEDA")) {
            valor = valor.replace(/$/g, "");
            valor = valor.replace(/ /g, "");
            valor = valor.replace(/\,/g, "");
        }

        cadena += "\"" + idObject + "\":\"" + valor + "\",";
    });

    $(idBusqueda + " input[type='checkbox']").each(function () {
        var idObject = $(this).attr("id");
        var valor = $(this).is(":checked") ? true : false;
        cadena += "\"" + idObject + "\":\"" + valor + "\",";
    });

    $(idBusqueda + " textarea").each(function () {
        var idObject = $(this).attr("id");
        cadena += "\"" + idObject + "\":\"" + $(this).val() + "\",";
    });

    $(idBusqueda + " select").each(function () {
        var idObject = $(this).attr("id");
        valor = $("#" + $(this).attr("id")).val();
        if (!isNaN(valor)) {
            cadena += "\"" + idObject + "\":" + valor + ",";
        }
    });

    /// Falta incorporar los radio botones
    if (cadena !== "") cadena = cadena.substring(0, cadena.length - 1);
    cadena = cadena.replace(/undefined/g, "LLAVE");
    cadena = cadena.replace(/:LLAVE/g, ":0");
    return JSON.parse("{" + cadena + "}");
}

function jsValidaModeloSinHtml(target) {
    for (var k in target) {
        if (target.hasOwnProperty(k)) {
            var cadena = target[k];
            if (cadena == null) cadena = "";
            cadena = new String(cadena);
            var resultSinHtml = cadena.replace(/(?:<[^>]+>)/gi, "");
            if (cadena != resultSinHtml) {
                if (G_MENSAJES.INYECCION_HTML != undefined || G_MENSAJES.INYECCION_HTML != null)
                { jsResultado(G_MENSAJES.INYECCION_HTML, "ERROR"); }
                else
                { jsResultado("Se ha detectado código HTML, favor de revisar", "ERROR"); }

                return false;
            }
        }
    }
    return true;
}

function jsAjaxGenericoPOST(controlador, modelo, divActualizar, callback) {

    //try{
    //    jsValidaSesion();
    //}catch(error){}

    var esModeloCorrecto = jsValidaModeloSinHtml(modelo);
    if (!esModeloCorrecto) return;

    $.ajax({
        url: controlador,
        data: modelo,
        async: false,
        type: "POST",
        cache: false,
        success: function (htmlResult) {
            if (divActualizar != null && divActualizar != undefined) {
                $("#" + divActualizar).html(htmlResult);
            }
            if (callback != null && callback != undefined) {
                callback(htmlResult);
            }
        },
        error: function (errorHtml) {

            //alert(JSON.stringify(errorHtml));
            jsResultado(G_MENSAJES.ERROR_GENERAL, "ERROR");
            ///(JSON.stringify(errorHtml));
        },
        complete: function () {
        }
    });
}

function jsAjaxGenerico(controlador, modelo, divActualizar, callback, conTrabajandoEnEllo) {

    G_ConSpinner = (conTrabajandoEnEllo == undefined) ? true : conTrabajandoEnEllo;

    var esModeloCorrecto = jsValidaModeloSinHtml(modelo);
    if (!esModeloCorrecto) return;

    $.ajax({
        url: controlador,
        data: modelo,
        async: false,
        cache: false,
        success: function (htmlResult) {
            if (divActualizar != null && divActualizar != undefined) {
                $("#" + divActualizar).html(htmlResult);
            }
            if (callback != null && callback != undefined) {
                callback(htmlResult);
            }
        },
        error: function (errorHtml) {
            //alert(errorHtml);
            jsResultado(G_MENSAJES.ERROR_GENERAL, "ERROR");
            ///(JSON.stringify(errorHtml));
        },
        complete: function () {
        }
    });
}

function PalabraCifrada(palabra) {
    var modelo = { Palabra: palabra };
}

function DeleteGenerico(table, id) {
    var modelo = {
        Id: id,
        Valor: table,
        Reactivar: false
    };
    jsAjaxGenerico("../Generico/DeleteReactive", modelo, null, jsResultDeleteReactive);
}

function jsResultDeleteReactive(html) {
    if (IndexOf(html, "ERROR") >= 0) {
        html = html.replace("ERROR: ", "");
        jsResultPrecaucion(html, true);

    } else {
        jsResultExitoso(html, true);
    }
}

function AnimarScrollTop() {
    if ($('html').scrollTop()) {
        $('html').animate({ scrollTop: 0 });
        return;
    }

    if ($('body').scrollTop()) {
        $('body').animate({ scrollTop: 0 });
        return;
    }
}


//function MeasurementForm(idForm)
//{

//    var fields = [];

//    new ns_.Form(idForm, {


//    });


//}

//function MeasurementField(idForm, fieldType, fields)
//{
//    $('#'+idForm + fieldType).each(function () {

//        var fieldName = $(this).prop('id');

//        var field = "{\"" + fieldName + "\":{submit:true}}";

//        fields.push(JSON.parse(field));


//    });


//    //var jsonText =JSON.stringify(fields);

//}