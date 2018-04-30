function startTime() {
    try {
        var today = new Date();
        var h = today.getHours();
        var m = today.getMinutes();
        var s = today.getSeconds();
        m = checkTime(m);
        s = checkTime(s);
        document.getElementById("reloj").innerHTML = h + ":" + m + ":" + s;
        setTimeout("startTime()", 500);
    } catch (error) {
    }
}

function checkTime(i) {
    if (i < 10) {
        i = "0" + i;
    }
    return i;
}

function fecha() {
    var f = new Date();
    var meses = new Array("Enero",
        "Febrero",
        "Marzo",
        "Abril",
        "Mayo",
        "Junio",
        "Julio",
        "Agosto",
        "Septiembre",
        "Octubre",
        "Noviembre",
        "Diciembre");
    //$(".ano").html(f.getFullYear());
    $(".dia").html(f.getDate());
    $(".mes").html(meses[f.getMonth()]);
}

$(document).ready(function() {
    fecha();
    window.onload = function() { startTime(); };
});