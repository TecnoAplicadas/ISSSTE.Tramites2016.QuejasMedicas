var ALERT_TEMPLATE =
    "<div class=\"alert alert-{0} {1}\" role=\"alert\"><button type=\"button\" class=\"close\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><span>{2}</span></div>";
var ALERT_TEMPLATE_WITH_TITLE =
    "<div class=\"alert alert-{0} {1}\" role=\"alert\"><button type=\"button\" class=\"close\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><strong>{2}</strong><br/><span>{3}</span></div>";
var ALERT_ID_TEMPLATE = "alert-{0}";

var UI =
{
    getBaseUrl: function() {
        return $("#baseUrl").val();
    },
    initTabs: function() {
        $(".nav-tabs a").click(function(e) {
            e.preventDefault();
            $(this).tab("show");
        });
    },
    initStatusDropdown: function() {
        $("#status-dropdow li a").click(function() {

            $(this).parent().parent().parent().find(".btn:first-child")
                .html("Mostrar " + $(this).text() + " <span class=\"caret\"></span>");
            $(this).parent().parent().parent().find(".btn:first-child").val($(this).val());

        });
    },
    initDropdown: function() {
        $("#status-dropdow li a").click(function() {

            $(this).parent().parent().parent().find(".btn:first-child")
                .html($(this).text() + " <span class=\"caret\"></span>");
            $(this).parent().parent().parent().find(".btn:first-child").val($(this).val());

        });
    },
    createInfoMessage: function(message, title) {
        UI.createMessage("info", message, title, false);
    },
    createWarningMessage: function(message, title) {
        //message = "";
        UI.createMessage("warning", message, title, false);
    },
    createSuccessMessage: function(message, title) {
        UI.createMessage("success", message, title, true);
    },
    createErrorMessage: function(message, title) {
        //message = "";
        UI.createMessage("danger", message, title, false);
    },
    createMessage: function(alertClass, message, title) {
        var alertsContainer = $(".alerts");

        if (alertsContainer) {

            if (title == null) {
                title = message;
            }

            var clase = "";
            if (alertClass === "info") {
                clase = "SUGERENCIA";
            } else if (alertClass === "warning") {
                clase = "PRECAUCION";
            } else if (alertClass === "success") {
                clase = "EXITO";
            } else if (alertClass === "danger") {
                clase = "ERROR";
            }
            jsResultado(title, clase);
            /*if (fadeOut) {
                window.setTimeout(function() {
                    $(".{0}".format(alertId)).fadeTo(500, 0, function() {
                        $(this).remove();
                    });
                }, 3000);
            }

            $(".{0}>.close".format(alertId)).on("click", function() {
                $(".{0}".format(alertId)).remove();
            });*/
        }
    },
    selectableTable: function(tableId) {
        var id = "#" + tableId;
        $(id).on("click",
            "tbody tr",
            function() {
                //$(this).addClass("active").siblings().removeClass("active");
                $(this).css("background-color","#c1c9f9").siblings().css("background-color",'');
            });
    }
};

function getRegex(caseStr) {

    var expresion = angular.lowercase(caseStr);
    switch (expresion) {
        case "ruta":
            return "^(?:[a-zA-Z]\\:(\\\\|\\/)|file\\:\\/\\/|\\\\\\\\|\\.(\\/|\\\\))([^\\\\\\/\\:\\*\\?\\<\\>\\\"\\\|]+(\\\\|\\/){0,1})+$";
        case "numero":
            return "^(\\d)+$";
            break;
        case "texto":
            return "^(\\D)+$";
            break;
        default: return null;
        
    }
};

function inArrayFindBy(searchFor, property) {
    if (this == null) return null;
    var retVal = -1;
    var self = this;
    for (var index = 0; index < self.length; index++) {
        var item = self[index];
        if (item.hasOwnProperty(property)) {
            if (item[property].toString().toLowerCase() === searchFor.toString().toLowerCase()) {
                retVal = index;
                return retVal;
            }
        }
    };
    return retVal;
}


function isValidDate(dateString) {

    // revisar el patrón
    if (!(/^(\d{2})\/(\d{2})\/(\d{4})$/).test(dateString))
        return false;

    // convertir los numeros a enteros formato dd/MM/yyyy
    var parts = dateString.split("/");
    var day = parseInt(parts[0], 10);
    var month = parseInt(parts[1], 10);
    var year = parseInt(parts[2], 10);

    // Revisar los rangos de año y mes
    if ((year < 1000) || (year > 3000) || (month == 0) || (month > 12))
        return false;

    var monthLength = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // Ajustar para los años bisiestos
    if (year % 400 == 0 || (year % 100 != 0 && year % 4 == 0))
        monthLength[1] = 29;

    // Revisar el rango del dia
    return day > 0 && day <= monthLength[month - 1];
}