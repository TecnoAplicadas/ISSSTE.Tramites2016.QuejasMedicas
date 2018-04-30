function ConfiguracionDatePicker(id) {
    $("#" + id).daterangepicker({
        "locale": {
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "Guardar",
            "cancelLabel": "Cancelar",
            "fromLabel": "Desde",
            "toLabel": "Hasta",
            "customRangeLabel": "Personalizar",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
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
                "Diciembre"
            ],
            "firstDay": 1
        },
        "startDate": $.now(),
        "endDate": $.now(),
        "opens": "center"
    });
}


function ToggleMenu() {
    document.querySelector.bind(document);
    var menu = document.querySelector("#side-menu");
    $("#demo_menu").toggle();
    menu.classList.toggle("vertical_nav__minify");

    $("#collapse_menu").click(function() {
        $("#demo_menu").toggle();
        menu.classList.toggle("vertical_nav__minify");
    });
}


function MenuHide() {
    document.querySelector.bind(document);
    var menu = document.querySelector("#side-menu");

    $("#demo_menu").toggle();
    menu.classList.toggle("vertical_nav__minify");
}

function getColorHex(colorDecimal) {
    var hexColor = colorDecimal.toString(16);
    while (hexColor.length < 6) {
        hexColor = "0" + hexColor;
    }
    var color = "#" + hexColor;
    return color;
}

function getColorDecimal(hexString) {
    var color = parseInt(hexString.replace("#", ""), 16);
    return color;
}
