function ClickExpansion2() {
    if ($("#ClickExpand").hasClass("expandir")) {
        $("#ClickExpand").removeClass("expandir");
        $("#ClickExpand").addClass("contraer");
        $(".container1").css("width", "95%");
        $(".container1").css("height", "580px");
        $(".container1").css("margin", "0 auto");
        $("#ResultadoTabla").css("display", "");
        $("#SeccionReporte").fadeIn("slow");
        $("#ClickExpand").mouseover(function() {
            tooltip.show("Click para ocultar esta área");
        });
        $("#ClickConsulta").css("display", "inline");
        $("#ClickDescarga").css("display", "none");
        $("#ClickDescargaJPG").css("display", "none");
        $("#ClickConsulta").css("margin-top", "10px");
        $("#ClickDescarga").css("margin-top", "10px");
        $("#ClickDescargaJPG").css("margin-top", "10px");
    } else {
        $("#ClickExpand").removeClass("contraer");
        $("#ClickExpand").addClass("expandir");
        $(".container1").css("width", "50px");
        $("#ResultadoTabla").css("display", "none");
        $("#SeccionReporte").fadeOut("fast");
        $(".container1").css("height", "50px");
        $(".container1").css("margin", "50px auto 0");
        $("#ClickExpand").mouseover(function() {
            tooltip.show("Click para revisar el reporte");
        });
        $("#ClickConsulta").css("display", "none");
        $("#ClickDescarga").css("display", "none");
        $("#ClickDescargaJPG").css("display", "none");
        $("#container").html("");

    }
}

function LimpiarFiltros() {
    $("#FiltrosReporte2 select").each(function() {
        $(this).val(0);
    });
    $("#FechaRegistro").val("");
    $("#FechaRegistro1").val("");
    $("#FechaCita").val("");
    $("#FechaCita1").val("");
}

var G_FiltrosReporteGrafico = {};

function jsFiltros2() {

    G_FiltrosReporteGrafico = {};

    var filtros = {
        UnidadAtencionId: $("#UnidadAtencionId").val(),
        //CatTipoEdoCita: ValidacionSelectMultiple("CatTipoEstado"),
        ///CatTipoTramite: ValidacionSelectMultiple("CatTipoTramite"),
        CatTipoEdoCitaId: $("#CatTipoEstadoId").val(),
        CatTipoTramiteId: $("#CatTipoTramiteId").val(),
        FechaInicioCita: ValidacionFecha("FechaCita"),
        FechaFinCita: ValidacionFecha("FechaCita1"),
        FechaInicioRegistro: ValidacionFecha("FechaRegistro"),
        FechaFinRegistro: ValidacionFecha("FechaRegistro1")
    };

    $("#ClickDescarga").css("display", "inline");
    $("#ClickDescargaJPG").css("display", "inline");
    $("#ClickConsulta").css("display", "none");

    jsAjaxGenerico("../Administrador/FiltroGrafica", filtros, "container");

    G_FiltrosReporteGrafico = filtros;
}

function jsCrearImageJPG(idDiv, html, nameFichero, esExcel) {

    if (idDiv != null) {
        html = $("#" + idDiv).html();
        html = html.replace("overflow-y:auto", "overflow-y:hidden");
        html = html.replace("overflow-y: auto", "overflow-y: hidden");
        html = html.replace("height:380px;", "");
        html = html.replace("height: 380px;", "");
    }
    var url = esExcel ? "../Forma/jsCrearExcelReporteGrafico" : "../Forma/jsCrearImageJPG";

    if (html != null && html !== "") {
        $("#formaPDF").remove();
        $("body").append('<form action="' + url + '" method="post" id="formaPDF" name="formaPDF"></form>');

        html = html.replace(/null/g, "");

        if (idDiv != null) {
            html = escape(html);
        }

        var form = $("#formaPDF");
        form.attr("data", { 'html': html });
        if (esExcel) form.append(JsParametroEnvio("JsonFiltros", JSON.stringify(G_FiltrosReporteGrafico)));
        form.append(JsParametroEnvio("inputHTML", html));
        form.append(JsParametroEnvio("nameFichero", nameFichero));
        form.attr("action", url);
        form.attr("method", "POST");
        form.submit();
    }
}