function ConstruyeDiv(clase, msn, icono, conFocus, leyenda) {
    var cadena = "<div class='" +
        clase +
        " alert-dismissible' role='alert' style='z-index:9999'>" +
        "<button type='button' class='close' data-dismiss='alert' aria-label='Close'>" +
        "<span aria-hidden='true'>&times;</span>" +
        "</button>" +
        //"<i class='" + Icono + "' aria-hidden='true'></i>&nbsp;&nbsp;" //
        "<b>" +
        leyenda +
        "</b>  " +
        msn +
        "</div>";
    if (conFocus !== false) {
        $("html, body").animate({ scrollTop: 0 }, 2000);
    }
    $("#" + G_contenedorMensaje).html(cadena);
}

function jsResultExitoso(resultado, conFocus) {
    ConstruyeDiv("alert alert-success", resultado, "glyphicon glyphicon-thumbs-up", conFocus, "¡Felicidades!");
}

function jsResultSugerencia(resultado, conFocus) {
    ConstruyeDiv("alert alert-info", resultado, "glyphicon glyphicon-hand-up", conFocus, "¡Sugerencia!");
}

function jsResultPrecaucion(resultado, conFocus) {
    ConstruyeDiv("alert alert-warning", resultado, "glyphicon glyphicon-hand-right", conFocus, "¡Precaución!");
}

function jsResultError(resultado, conFocus) {
    ConstruyeDiv("alert alert-danger", resultado, "glyphicon glyphicon-thumbs-down", conFocus, "¡Error de registro!");
}

function jsModalResult(mensaje, icono, clase) {
    return '<div id="myModal" class="modal fade" tabindex="-1" style="z-index:9999;display:block;">' +
        '<div class="modal-dialog modal-lg">' +
        '<div class="modal-content bootbox-body">' +
        '<div class="modal-body bootbox-body ' +  clase + '" style="margin-bottom:0px;">' +
        '<div class="row">' +
        ((clase === "")
            ? ('<div class="col-md-12 col-lg-12">' + mensaje + "</div>")
            : ('<div class="col-md-1 col-lg-1 glyphicon ' + icono + ' "></div>' +
                '<div class="col-md-11 col-lg-11 text-center">' +
                mensaje +
                "</div>")) +
        "</div>" +
        "</div>" +
        ///'<div class="modal-footer ' + clase + '" style="margin-bottom:0px;"><button type="button" class="btn btn-sm">OK</button></div>' +
        "</div>" +
        "</div>" +
        "</div>";
}

function jsResultado(resultado, clase) {

    resultado = new String(resultado);
    resultado = resultado.replace(/&lt;/g, "<");
    resultado = resultado.replace(/&gt;/g, ">");
    var icono = '<div class="col-md-1 col-lg-1 FACTOR" style="color:ESTILO;"></div>';
    if (clase === "EXITO") {
        icono = "glyphicon glyphicon-ok-circle";
    } else if (clase === "SUGERENCIA") {
        icono = "glyphicon glyphicon-info-sign";
    } else if (clase === "PRECAUCION") {
        icono = " 	glyphicon glyphicon-question-sign";
    } else if (clase === "ERROR") {
        icono = "glyphicon glyphicon-remove-circle";
    } else {
        clase = "";
    }
    
    $("#myModal").remove(); // Quitar el modal del DOM GFB 21/06/2017    
    $(".modal-backdrop").remove();
    $("body").append(jsModalResult(resultado, icono, clase));

    $("body").css("padding-right", '0px');

    $("#myModal").modal("show");

    setTimeout(function() {
            CierreModal();
        },
        20000); // 20 Segundos que pidio el usuario

    $(".bootbox-body").click(function () {
        console.log('cierre desde el modal');
        CierreModal();
    });

    $('.modal-backdrop').click(function () {
        console.log('cierre desde el backdrop');
        CierreModal();
    });
}

function CierreModal() {
    try {
        $("#myModal").modal("hide");

        if ($(".modal-backdrop").is(":visible")) {
            $("body").removeClass("modal-open");            
        }

        $("#myModal").remove();
        $(".modal-backdrop").remove();

        $("body").css("padding-right", '0px');
        
    } catch (ex) {
    }
}

function copyToClipboard(id) {

    var mensaje = $("#" + id).html();
    mensaje = escape(mensaje);
    jsCrearXLS(null, mensaje, "Detalle_Registro.xls");
}

function jsUnEscape(valor) {

    valor = unescape(valor);
    valor = valor.replace(/%3B/g, ";").replace(/%09/, "");

    return valor;
}

function jsConfirmar(mensaje, titulo, metodoAceptar, ejecutarCadena, metodoCancelar, ejecutarCancelar) {

    mensaje = new String(mensaje);
    mensaje = mensaje.replace(/&lt;/g, "<");
    mensaje = mensaje.replace(/&gt;/g, ">");

    bootbox.dialog({
        message: mensaje,
        title: titulo,
        buttons: {
            success: {
                //label: '<i class="fa fa-times"></i> No',
                label: 'No',
                className: "btn-default",
                callback: function() {
                    //ejemplo:
                    //cadena = "nkkjkjkjskj;.,sdkjsd";
                    //cadena = escape(cadena);
                    if (ejecutarCancelar) {
                        var valor = jsUnEscape(metodoCancelar);
                        eval(valor);
                    } else if (metodoCancelar != null && metodoCancelar != undefined) {
                        metodoCancelar();
                    }
                }
            },
            danger: {
                //label: '<i class="fa fa-check"></i> Si',
                label: 'Si',
                className: "btn-primary",
                callback: function() {
                    //ejemplo:
                    //cadena = "nkkjkjkjskj;.,sdkjsd";
                    //cadena = escape(cadena);
                    if (ejecutarCadena) {
                        var valor = jsUnEscape(metodoAceptar);
                        eval(valor);
                    } else {
                        metodoAceptar();
                    }
                }
            }
        }
    });
}

function BreadGral(title) {
    $("#TitleGral").html(" \\ " + title);
}