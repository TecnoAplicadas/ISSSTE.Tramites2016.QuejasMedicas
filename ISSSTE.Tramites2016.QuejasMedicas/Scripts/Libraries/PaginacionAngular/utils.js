//Funciones generales
var Utils =
{
    //Funcion para obtener QueryString de la URL
    getQueryStringValue: function(name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)", "gi"),
            results = regex.exec(window.location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },
    escapeRegExp: function(string) {
        return string.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1");
    },
    S4: function() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }
};

//Funciones de cadenas
String.prototype.format = function() {
    var string = this;
    var currentParamIndex = arguments.length;

    while (currentParamIndex--) {
        string = string.replace(new RegExp("\\{" + currentParamIndex + "\\}", "gm"), arguments[currentParamIndex]);
    }
    return string;
};

String.prototype.replaceAll = function(stringToFind, replaceString) {
    var string = this;

    return string.replace(new RegExp(Utils.escapeRegExp(stringToFind), "g"), replaceString);
};

String.prototype.endsWith = function(suffix) {
    return this.indexOf(suffix, this.length - suffix.length) !== -1;
};

//Funciones para generar Guids
var Guid = {
    newGuid: function() {
        return (Utils.S4() +
            Utils.S4() +
            "-" +
            Utils.S4() +
            "-4" +
            Utils.S4().substr(0, 3) +
            "-" +
            Utils.S4() +
            "-" +
            Utils.S4() +
            Utils.S4() +
            Utils.S4()).toLowerCase();
    }
};

//Funciones para imprimr
Window.prototype.managedPrint = function(winPrint) {
    var is_chrome = Boolean(window.chrome);
    if (is_chrome) {
        setTimeout(function() {
                winPrint.print();
                winPrint.close();
            },
            500);
        //give them 10 seconds to print, then close
    } else {
        winPrint.document.close();
        winPrint.focus();
        winPrint.print();
        winPrint.close();
    }
};

//Funciones para objetos
var ObjectCollections =
{
    forEach: function(object, action) {
        for (var key in object) {
            if (object.hasOwnProperty(key)) {
                var prop = object[key];
                action(prop);
            }
        }
    },
    length: function(object) {
        var propertiesCount = 0;

        for (var key in object) {
            propertiesCount++;
        }

        return propertiesCount;
    }
};