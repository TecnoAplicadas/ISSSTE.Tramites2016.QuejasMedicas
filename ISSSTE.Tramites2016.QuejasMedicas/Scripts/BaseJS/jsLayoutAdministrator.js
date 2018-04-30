function jsJuegoMenu() {
    $("#MenuSecundario").click(function() {
        $("#MenuPrincipal").toggle("slow");
        $("#MenuSecundario").hide();
    });
    $("#MenuPrincipalBoton").click(function() {
        $("#MenuPrincipal").toggle("slow");
        $("#MenuSecundario").show();
    });
}

function jsMenu(datos) {
    var menuNuevo = $("#demo_menu");
    var html = [];
    html.push(datos);

    menuNuevo.easytree({
        addable: false,
        editable: false,
        deletable: false,
        data: html
    });
}