function jsChangePaginador(IdTabla) {
    valor = $("#Select_" + IdTabla).val();
    if (valor == null || valor == undefined) return 10;
    return valor;
}

function KeyChangeSelect(IdTabla) {
    SetValorDataAtributo(IdTabla, "data-page", 1);
    $("#PagAct_" + IdTabla).html(1);
    SetValorDataAtributo(IdTabla, "data-select", $("#Select_" + IdTabla).val());
    ConsultaServidor = jsMuestraRows(IdTabla);
    if (ConsultaServidor) {
        eval(IdTabla + "()");
    }
}

function jsPaginaRows(IdTabla) {
    valor = GetValorBoolNull("Paginacion_" + IdTabla, "data-paginacion");
    if (valor == true) {
        return;
    } else {
        var i = 0;
        $("#" + IdTabla + " > tbody  > tr").each(function() {
            $(this).attr("id", IdTabla + "_" + i++);
        });
        SetValorDataAtributo("Paginacion_" + IdTabla, "data-paginacion", true);
    }
}

function jsFiltroPalabra(IdTabla) {

    De = GetValorDataAtributo(IdTabla, "data-page") == 0 ? 1 : GetValorDataAtributo(IdTabla, "data-page");
    De = parseInt(De);
    SelectIncremento = jsChangePaginador(IdTabla);
    SelectIncremento = parseInt(SelectIncremento);
    De = (De - 1) * SelectIncremento;

    if (GetValorBoolNull("Paginacion_" + IdTabla, "data-controlador")) {
        De = 0;
    }

    De = parseInt(De);
    Hasta = De + SelectIncremento;
    Palabra = $("#Busqueda_" + IdTabla).val();
    Borron(IdTabla);
    for (i = De; i < Hasta; i++) {
        $("#" + IdTabla + "_" + i).removeClass("hidden");
    }
    if (Palabra != "" && Palabra != null && Palabra != undefined && Palabra.length <= 3) {
        return;
    }
    for (i = De; i < Hasta; i++) {
        texto = $("#" + IdTabla + "_" + i).text();
        if (IndexOf(texto,Palabra) < 0) {
            $("#" + IdTabla + "_" + i).addClass("hidden");
        } else {
            $("#" + IdTabla + "_" + i).removeClass("hidden");
            $("#" + IdTabla + "_" + i + " td").each(function(index) {
                valor = $(this).html();
                var reg = new RegExp(Palabra, "g");
                valor = valor.replace(reg, "<span class=\"highlight\">" + Palabra + "</span>");
                $(this).html(valor);
            });
        }
    }
}

function jsMuestraRows(IdTabla) {
    valor = GetValorBoolNull("Paginacion_" + IdTabla, "data-controlador");
    if (valor) {
        return true;
    } else {
        $(".Row").addClass("hidden");
        De = GetValorDataAtributo(IdTabla, "data-page") == 0 ? 1 : GetValorDataAtributo(IdTabla, "data-page");
        De = parseInt(De);
        De = (De - 1) * jsChangePaginador(IdTabla);
        Hasta = parseInt(De) + parseInt(jsChangePaginador(IdTabla));
        for (i = De; i < Hasta; i++) {
            $("#" + IdTabla + "_" + i).removeClass("hidden");
        }
        jsFiltroPalabra(IdTabla);
        jsPage(IdTabla);
    }
}


function jsPage(IdTabla) {
    Rows = GetValorDataAtributo(IdTabla, "data-rows");
    if (Rows > 0) {
        $("#PagAct_" + IdTabla).html(GetValorDataAtributo(IdTabla, "data-page"));
        $("#Select_" + IdTabla).val(GetValorDataAtributo(IdTabla, "data-select"));
        Paginacion = jsChangePaginador(IdTabla);
        $("#PagTotal_" + IdTabla).html(Math.floor(Rows / Paginacion) + (Rows % Paginacion == 0 ? 0 : 1));
    }
}

function jsInitPaginador(IdTabla) {
    Rows = GetValorDataAtributo(IdTabla, "data-rows");

    if (Rows > 0) {
        $("#PagAct_" + IdTabla).html(GetValorDataAtributo(IdTabla, "data-page"));
        $("#Select_" + IdTabla).val(GetValorDataAtributo(IdTabla, "data-select"));
        Paginacion = jsChangePaginador(IdTabla);
        $("#PagTotal_" + IdTabla).html(Math.floor(Rows / Paginacion) + (Rows % Paginacion == 0 ? 0 : 1));
    }

    $("#Rows_" + IdTabla).html(Rows);
    DeshabilitaRetroceso(IdTabla);
    DeshabilitaAvance(IdTabla);
    jsPaginaRows(IdTabla);
    jsMuestraRows(IdTabla);
    jsFiltroPalabra(IdTabla);
    jsTrHover(IdTabla);
}

function jsTrHover(IdTabla) {
    $("#" + IdTabla + " > tbody > tr").mouseover(function() {
        $(this).css("background-color", "DAF7A6");
        $(this).css("color", "white");
    });
    $("#" + IdTabla + " > tbody > tr").mouseout(function() {
        id = $(this).attr("id");
        id = id.replace(IdTabla + "_", "");
        id = parseInt(id);

        $(this).css("background-color", (id % 2 ? "white" : "#e5ecf9"));
        $(this).css("color", "black");
    });
    $("#" + IdTabla + " > tbody > tr > td").each(function() {
        $(this).css("width", "500px; !important");
        $(this).css("cursor", "pointer");
    });
}

function KeyBusqueda(e, IdTabla) {

    if (parseInt(e.keyCode) == 13) {
        PaginaBusqueda = $("#Captura_" + IdTabla).val();
        if (PaginaBusqueda != "" && PaginaBusqueda != null && PaginaBusqueda != undefined) {
            PaginaBusqueda = parseInt(PaginaBusqueda);
            if (PaginaBusqueda >= 1 && PaginaBusqueda <= PaginaTotal(IdTabla)) {
                SetValorDataAtributo(IdTabla, "data-page", PaginaBusqueda);
                $("#PagAct_" + IdTabla).html(PaginaBusqueda);
                ConsultaServidor = jsMuestraRows(IdTabla);
                if (ConsultaServidor) {
                    eval(IdTabla + "()");
                }
            }
        }
    }
}

function KeyBusquedaFrase(e, IdTabla) {
    if (parseInt(e.keyCode) == 13) {
        Palabra = $("#Busqueda_" + IdTabla).val();
        jsFiltroPalabra(IdTabla);
    }
}

function DeshabilitaRetroceso(IdTabla) {
    PagAct = PaginaActual(IdTabla);
    if (PagAct > 1) {
        $("#ZonaAnt_" + IdTabla).removeClass("disabled");
    } else {
        $("#ZonaAnt_" + IdTabla).addClass("disabled");
    }
}

function jsAnterior(IdTabla) {
    DeshabilitaRetroceso(IdTabla);
    PagAct = PaginaActual(IdTabla);
    if (PagAct > 1) {
        Pagina = PaginaActual(IdTabla) - 1;
        SetValorDataAtributo(IdTabla, "data-page", Pagina);
        $("#PagAct_" + IdTabla).html(Pagina);
        ConsultaServidor = jsMuestraRows(IdTabla);
        if (ConsultaServidor) {
            eval(IdTabla + "()");
        }
    }
}

function DeshabilitaAvance(IdTabla) {
    if (PaginaTotal(IdTabla) > PaginaActual(IdTabla)) {
        $("#ZonaSig_" + IdTabla).removeClass("disabled");
    } else {
        $("#ZonaSig_" + IdTabla).addClass("disabled");
    }
}

function jsSiguiente(IdTabla) {
    DeshabilitaAvance(IdTabla);
    if (PaginaTotal(IdTabla) > PaginaActual(IdTabla)) {
        Pagina = PaginaActual(IdTabla) + 1;
        SetValorDataAtributo(IdTabla, "data-page", Pagina);
        $("#PagAct_" + IdTabla).html(Pagina);
        ConsultaServidor = jsMuestraRows(IdTabla);
        if (ConsultaServidor) {
            eval(IdTabla + "()");
        }
    }
}

function PaginaActual(IdTabla) {
    valor = $("#PagAct_" + IdTabla).html();
    return parseInt(valor);
}

function PaginaTotal(IdTabla) {
    valor = $("#PagTotal_" + IdTabla).html();
    return parseInt(valor);
}

function GetValorDataAtributo(IdTabla, Atributo) {
    valor = $("#" + IdTabla).attr(Atributo);
    if (valor == null || valor == undefined) {
        valor = 0;
        $("#" + IdTabla).attr(Atributo, 0);
    }
    return parseInt(valor);
}

function ValorBooleanDiv(valor) {
    try {
        valor = new String(valor);
        valor = valor.toUpperCase();
        if (valor == "UNDEFINED") return null;
        valor = (valor == "TRUE") ? true : false;
    } catch (ERROR) {
        valor = null;
    }
    return valor;
}

function GetValorBoolNull(IdTabla, Atributo) {
    valor = $("#" + IdTabla).attr(Atributo);
    valor = ValorBooleanDiv(valor);
    return valor;
}

function SetValorDataAtributo(IdTabla, Atributo, Valor) {
    $("#" + IdTabla).attr(Atributo, Valor);
}

function Borron(IdTabla) {
    $("#" + IdTabla + " .highlight").replaceWith(function() {
        return $(this).html();
    });
}

function jsPeticion(IdTabla, modelo, ConPaginadorController) {
    if (modelo == null || modelo == undefined) modelo = {};
    modelo.ConPaginadorController = ConPaginadorController;
    modelo.RegistrosPorPagina = jsChangePaginador(IdTabla);
    modelo.PaginaSolicitada = GetValorDataAtributo(IdTabla, "data-page") == 0
        ? 1
        : GetValorDataAtributo(IdTabla, "data-page");
    modelo.TextoBusqueda = $("#Busqueda_" + IdTabla).val();
}