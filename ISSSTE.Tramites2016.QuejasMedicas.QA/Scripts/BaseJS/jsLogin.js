$(document).ready(function() {
    $("*").bind("cut copy paste",
        function(e) {
            e.preventDefault();
        });

    RecalculoCaptcha();
    ResultadoIngresoSesion();

});

function RecalculoCaptcha() {
    $("#Captcha").val("");
    jsAjaxGenerico("../Account/Captcha", null, "DivImagenCaptcha");
}

function ValorDiv(id) {
    return $("#" + id).html();
}

function ValorBooleanDiv(id) {
    var valor = ValorDiv(id);
    try {
        valor = new String(valor);
        valor = valor.toUpperCase();
        valor = (valor === "TRUE") ? true : false;
    } catch (error) {
        valor = false;
    }
    return valor;
}

function GetModeloLogin() {
    var model = {
        Mensaje: ValorDiv("Mensaje"),
        ConIngresoCorrecto: ValorBooleanDiv("ConIngresoCorrecto"),
        ConSesionIniciada: ValorBooleanDiv("ConSesionIniciada")
    };
    return model;
}


function ResultadoIngresoSesion() {
    var modelo = GetModeloLogin();
    if (modelo.ConIngresoCorrecto === false) {
        if (modelo.ConSesionIniciada === true) {
            jsConfirmar(modelo.Mensaje, "", AbandonLoginSesion);
        } else if (modelo.Mensaje != null && modelo.Mensaje !== "") {
            RecalculoCaptcha();
            jsResultado(modelo.Mensaje, "ERROR");
        }
    }
}

function AbandonLoginSesion() {
    AbandonValidacion(false);
    $("#Login").submit();
}

function AbandonSesion() {
    AbandonValidacion(true);
    window.location.href = "../Account/Login";
}

function AbandonValidacion(redirect) {
    var modelo = { redirect: redirect };
    jsAjaxGenerico("../Account/AbandonValidacion", modelo, null, null);
}

$(document).ready(function() {
    $("*").bind("cut copy paste",
        function(e) {
            e.preventDefault();
        });

    RecalculoCaptcha();
    ResultadoIngresoSesion();

});

function RecalculoCaptcha() {
    $("#Captcha").val("");
    jsAjaxGenerico("../Account/Captcha", null, "DivImagenCaptcha");
}

function ValorDiv(id) {
    return $("#" + id).html();
}

function ValorBooleanDiv(id) {
    var valor = ValorDiv(id);
    try {
        valor = new String(valor);
        valor = valor.toUpperCase();
        valor = (valor === "TRUE") ? true : false;
    } catch (error) {
        valor = false;
    }
    return valor;
}

function GetModeloLogin() {
    var model = {
        Mensaje: ValorDiv("Mensaje"),
        ConIngresoCorrecto: ValorBooleanDiv("ConIngresoCorrecto"),
        ConSesionIniciada: ValorBooleanDiv("ConSesionIniciada")
    };
    return model;
}


function ResultadoIngresoSesion() {
    var modelo = GetModeloLogin();
    if (modelo.ConIngresoCorrecto === false) {
        if (modelo.ConSesionIniciada === true) {
            jsConfirmar(modelo.Mensaje, "", AbandonLoginSesion);
        } else if (modelo.Mensaje != null && modelo.Mensaje !== "") {
            RecalculoCaptcha();
            jsResultado(modelo.Mensaje, "ERROR");
        }
    }
}

function AbandonLoginSesion() {
    AbandonValidacion(false);
    $("#Login").submit();
}

function AbandonSesion() {
    AbandonValidacion(true);
    window.location.href = "../Account/Login";
}

function AbandonValidacion(redirect) {
    var modelo = { redirect: redirect };
    jsAjaxGenerico("../Account/AbandonValidacion", modelo, null, null);
}