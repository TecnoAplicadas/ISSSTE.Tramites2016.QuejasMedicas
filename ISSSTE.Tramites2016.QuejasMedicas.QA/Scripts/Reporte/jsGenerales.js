function jsValidaHora(id) {
    valor = $("#" + id).val();
    var patron = /(([01]\d)|(2[0-3])):([0-5]\d)/;
    if (valor.match(patron)) {
        return;
    }
    if (valor !== "") {
        valor = $("#" + id).val("00:00");
    }
}

function MostrarFiltros(clase) {
    $("#" + clase + " .DivGrupo").css("display", "inline");
}

function jsConfiguracionInicialGeneral() {
    $("#HoraCita").blur(function() {
        jsValidaHora("HoraCita");
    });
    $("#HoraCita1").blur(function() {
        jsValidaHora("HoraCita1");
    });
    $("#HoraSolicitud").blur(function() {
        jsValidaHora("HoraSolicitud");
    });
    $("#HoraSolicitud1").blur(function() {
        jsValidaHora("HoraSolicitud1");
    });
    /*$("#FechaRegistro").datepicker();
    $("#FechaRegistro1").datepicker();
    $("#FechaCita").datepicker();
    $("#FechaCita1").datepicker();*/

    $("#FechaRegistro").datepicker({
        onClose: function(selectedDate) {
            $("#FechaRegistro1").datepicker("option", "minDate", selectedDate);
        }
    });
    $("#FechaRegistro1").datepicker({
        onClose: function(selectedDate) {
            $("#FechaRegistro").datepicker("option", "maxDate", selectedDate);
        }
    });
    $("#FechaCita").datepicker({
        onClose: function(selectedDate) {
            $("#FechaCita1").datepicker("option", "minDate", selectedDate);
        }
    });
    $("#FechaCita1").datepicker({
        onClose: function(selectedDate) {
            $("#FechaCita").datepicker("option", "maxDate", selectedDate);
        }
    });
}

function jsCloseMakerClick() {
    $(".closeMaker").click(function() {
        var id = $(this).attr("id").replace("Close_", "");
        if (document.getElementById(id) != null && document.getElementById(id) != undefined) {
            var tipo = document.getElementById(id).tagName.toUpperCase();
            if (tipo == "INPUT") {
                $("#" + id).val("");
                $("#" + id + "1").val("");
            } else {
                var tieneIndistinto = $("#"+id+" option[value='-1']").length;
                //alert(tieneIndistinto);
                if (tieneIndistinto!=0)
                { $("#" + id).val(-1); }
                else { $("#" + id).val(0); }
            }
        }
        $("#DivGrupo_" + id).css("display", "none");
    });
}

function jsMouseOverFiltro() {
    //$(".DivGrupo").mouseover(function() {
    //    var id = $(this).attr("id");
    //    id = id.replace("DivGrupo_", "");
    //    $(".dropdown-toggle[data-id ='" + id + "']").css("border", "1px solid #6A0888");
    //    $("#" + id).css("border", "1px solid #6A0888");
    //    $("#" + id + "1").css("border", "1px solid #6A0888");
    //    $("#" + id + "2").css("border", "1px solid #6A0888");
    //    $("#Label_" + id).css("color", "#6A0888");
    //    $("#Label_" + id + "1").css("color", "#6A0888");
    //    $("#Label_" + id + "2").css("color", "#6A0888");
    //    $("#Calendar_" + id).css("color", "#6A0888");
    //    $("#Calendar_" + id + "1").css("color", "#6A0888");
    //    $("#Calendar_" + id).css("border", "1px solid #6A0888");
    //    $("#Calendar_" + id + "1").css("border", "1px solid #6A0888");
    //});
}

function jsMouseOutFiltro() {
    //2E9AFE => color anterior
    //$(".DivGrupo").mouseout(function() {
    //    var id = $(this).attr("id");
    //    id = id.replace("DivGrupo_", "");
    //    $(".dropdown-toggle[data-id ='" + id + "']").css("border", "1px solid #000000");
    //    $("#" + id).css("border", "0px inherit #000000");
    //    $("#" + id + "1").css("border", "0px inherit #000000");
    //    $("#" + id + "2").css("border", "0px inherit #000000");
    //    $("#Label_" + id).css("color", "#000000");
    //    $("#Label_" + id + "1").css("color", "#000000");
    //    $("#Label_" + id + "2").css("color", "#000000");
    //    $("#Calendar_" + id).css("color", "#000000");
    //    $("#Calendar_" + id + "1").css("color", "#000000");
    //    $("#Calendar_" + id).css("border", "0px inherit #000000");
    //    $("#Calendar_" + id + "1").css("border", "0px inherit #000000");

    //});
}

function RecargaUADyCS() {
    modelo = { SinTodos: true };
    jsAjaxGenerico("../Catalogo/GetUserUADyCS", modelo, "UnidadAtencionId");
    var unidadAtencionIds = $("#UnidadAtencionId option").length;

    if (unidadAtencionIds <= 2)
    {
        $("#UnidadAtencionId option").each(function () {
            var valor = $(this).val();
            if (parseInt(valor) > 0) {
                $('#UnidadAtencionId').val(valor);
                $('#UnidadAtencionId').prop("disabled", true);
            }

        })

    }

}

function jsResultConsultaDesarrollador() {
    var texto = $("#PeticicionJson").html();
    texto = texto.replace(/,/g, ", ");
    texto = "<i class='fa fa-2x fa-file-text-o pull-right' aria-hidden='true' " +
        " onmouseover='tooltip.show(\"Click para descargar el contenido en formato Txt\")' " +
        " onmouseout='tooltip.hide();' onclick='copyToClipboardTXT(\"contenidoMensajeC\");' " +
        ">&nbsp;</i><div id='contenidoMensajeC'>" +
        texto +
        "</div>";
    texto = "<H1>Script de consulta</H1></br>" + texto;
    jsResultado(texto, "");
}

function copyToClipboardTXT(id) {

    var mensaje = $("#" + id).text();
    mensaje = escape(mensaje);
    jsCrearTXT(null, mensaje, "Detalle_Registro.txt");
}

function jsResultConsulta() {
    var texto = $("#PeticionNormalizada").html();
    jsResultado(texto, "");
}

function ClickExpansion() {
    if ($("#ClickExpand").hasClass("expandir")) {
        $("#ClickExpand").removeClass("expandir");
        $("#ClickExpand").addClass("contraer");
        $(".container1").css("width", "95%");
        $(".container1").css("height", "600px");
        $(".container1").css("margin", "0 auto");
        $("#ResultadoTabla").css("display", "");
        $("#SeccionReporte").fadeIn("slow");
        $("#ClickExpand").mouseover(function() {
            tooltip.show("Click para ocultar esta área");
        });

        $("#ClickConsulta").css("display", "inline");
        $("#ClickConsulta").css("padding-top", "11px");
        $("#ClickTools").css("padding-top", "13px");
        $("#ClickResume").css("padding-top", "13px");
        $("#ClickExcel").css("padding-top", "13px");
        $("#ClickTools").css("color", "yellowgreen");
        $("#ClickResume").css("color", "blueviolet");
        $("#ClickExcel").css("color", "green");
        $("#SeccionReporte").html("");

    } else {
        $("#ClickExpand").removeClass("contraer");
        $("#ClickExpand").addClass("expandir");
        $(".container1").css("width", "50px");
        $("#ResultadoTabla").css("display", "none");
        $("#SeccionReporte").fadeOut("fast");
        $(".container1").css("height", "50px");
        $(".container1").css("margin", "50px auto 0");
        $("#ClickExpand").mouseover(function() {
            tooltip.show("Click para mostrar el detalle del reporte");
        });

        $("#ClickConsulta").css("display", "none");
        $("#ClickTools").css("display", "none");
        $("#ClickResume").css("display", "none");
        $("#ClickExcel").css("display", "none");

    }
}

function jsDetalle(renglon) {
    var auxiliar = "";
    var idRenglon = renglon.attr("id");
    $("#CompendioTable1 thead tr th").each(function() {
        $("#auxiliar").html($(this).html());
        var clases = $("#auxiliar > div").attr("class");
        var header = $("#auxiliar > div").html();
        header = header.replace("</div>", "CUERPO</div>");
        var cuerpo = $("#" + idRenglon + " > ." + clases).html();
        header = "<b>" + header + ": </b>" + cuerpo + "</br>";
        auxiliar += header;
    });
    auxiliar = "<b><h1>Detalle del registro</h1></b></br>" + auxiliar;

    auxiliar = "<i class='fa fa-2x fa-file-excel-o pull-right' aria-hidden='true' " +
        " onmouseover='tooltip.show(\"Click para descargar el contenido en formato Excel\")' " +
        " onmouseout='tooltip.hide();' onclick='copyToClipboard(\"contenidoMensaje\");' " +
        ">&nbsp;</i><div id='contenidoMensaje' " +
        auxiliar +
        "</div>";

    jsResultado(auxiliar, "");
    $("#auxiliar").html("");
}