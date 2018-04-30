function ConsultarReporte() {

    var filtros = {};

    ///Columnas dinámicas
    filtros.Llaves = ObtenerEncabezados();
    filtros.Encabezados = ObtenerLlaves(filtros.Llaves);

    if (filtros.Llaves.length === 0) {
        jsResultado("Debe seleccionar al menos una columna de salida para generar el reporte", "PRECAUCION");
        return;
    }

    /// Filtros del Paciente
    filtros.NombrePaciente = $("#NombrePaciente").val();
    filtros.Apellido1Paciente = $("#Apellido1Paciente").val();
    filtros.Apellido2Paciente = $("#Apellido2Paciente").val();
    filtros.CURPPaciente = $("#CURPPaciente").val();
    filtros.CorreoPaciente = $("#CorreoPaciente").val();
    filtros.TelefonoPaciente = $("#TelefonoPaciente").val();
    filtros.CelularPaciente = $("#CelularPaciente").val();

    filtros.EdadInicioPaciente = ValidacionEntero("EdadPaciente");
    filtros.EdadFinPaciente = ValidacionEntero("EdadPaciente1");
    if (filtros.EdadFinPaciente === 0) filtros.EdadFinPaciente = null;
    filtros.EsMasculinoPaciente = ValidacionSelectNull("SexoPaciente");
    filtros.EsUsuarioBloqueadoPaciente = ValidacionSelectNull("BloqueadoPaciente");

    /// Filtros del Promovente
    filtros.NombrePromovente = $("#NombrePromovente").val();
    filtros.Apellido1Promovente = $("#Apellido1Promovente").val();
    filtros.Apellido2Promovente = $("#Apellido2Promovente").val();
    filtros.CURPPromovente = $("#CURPPromovente").val();
    filtros.CorreoPromovente = $("#CorreoPromovente").val();
    filtros.TelefonoPromovente = $("#TelefonoPromovente").val();
    filtros.CelularPromovente = $("#CelularPromovente").val();

    filtros.EdadInicioPromovente = ValidacionEntero("EdadPromovente");
    filtros.EdadFinPromovente = ValidacionEntero("EdadPromovente1");
    if (filtros.EdadFinPromovente === 0) filtros.EdadFinPromovente = null;
    filtros.EsMasculinoPromovente = ValidacionSelectNull("SexoPromovente");

    /// Filtros generales de la cita
    filtros.HoraInicioCita = ValidacionHora("HoraCita");
    filtros.HoraFinCita = ValidacionHora("HoraCita1");
    filtros.HoraInicioRegistro = ValidacionHora("HoraSolicitud");
    filtros.HoraFinRegistro = ValidacionHora("HoraSolicitud1");

    filtros.FechaInicioCita = ValidacionFecha("FechaCita");
    filtros.FechaFinCita = ValidacionFecha("FechaCita1");
    filtros.FechaInicioRegistro = ValidacionFecha("FechaRegistro");
    filtros.FechaFinRegistro = ValidacionFecha("FechaRegistro1");

    filtros.CatTipoEdoCita = ValidacionSelectMultiple("CatTipoEstado");
    filtros.CatTipoTramite = ValidacionSelectMultiple("CatTipoTramite");

    /// Filtros por UADyCS
    filtros.UnidadAtencionId = $("#UnidadAtencionId").val();
    filtros.Unidad_Atencion = $("#UnidadAtencionId option:selected").text();

    var cadena = JSON.stringify(filtros);
    //    var reg = new RegExp(",", "g");
    //    cadena = cadena.replace(reg, ", </br>");
    jsNormalizarModelo(filtros);
    $("#PeticicionJson").html(cadena);
    G_FiltrosQuery = filtros;
    CompendioTable1();

    $("#ClickExcel").css("display", "inline");
    $("#ClickResume").css("display", "inline");
    $("#ClickTools").css("display", "inline");
    $("#ClickConsulta").css("display", "none");
}

function jsLabel(mensaje, clase) {
    return "<label class='" + clase + "'>" + mensaje + "</label>";
}

function jsValor(mensaje) {
    return jsLabel(mensaje, "FiltroValores") + "</br>";
}

function jsEdad(inicio, fin, entidad) {
    if (fin == null && inicio !== 0) {
        return "La " +
            jsLabel("edad del " + entidad, "FiltroPaciente") +
            " sea mayor o igual a" +
            jsValor("\"" + inicio + "\"");
    }
    if (fin != null) {
        if (inicio > fin) {
            var temp = fin;
            fin = inicio;
            inicio = temp;
        }
        return "La " +
            jsLabel("edad del " + entidad, "FiltroPaciente") +
            " sea " +
            jsValor("\" mayor o igual a " + inicio + " y menor o igual a " + fin + "\"");
    }
    return "";
}

function jsHora(inicio, fin, entidad) {
    if (fin == null && inicio != null) {
        return "La " +
            jsLabel("hora " + entidad, "FiltroCita") +
            " sea mayor o igual a" +
            jsValor("\"" + inicio + "\"");
    }
    if (inicio == null && fin != null) {
        return "La " + jsLabel("hora " + entidad, "FiltroCita") + " sea mayor o igual a" + jsValor("\"" + fin + "\"");
    }

    if (fin != null && inicio != null) {

        var fechaUno = new Date();
        var fechaDos = new Date();

        fechaUno.setHours(parseInt(inicio.split(":")[0]));
        fechaUno.setMinutes(parseInt(inicio.split(":")[1]));

        fechaDos.setHours(parseInt(fin.split(":")[0]));
        fechaDos.setMinutes(parseInt(fin.split(":")[1]));

        if (fechaUno > fechaDos) {
            var temp = fin;
            fin = inicio;
            inicio = temp;
        }
        return "La " +
            jsLabel("hora del " + entidad, "FiltroCita") +
            " sea " +
            jsValor("\" mayor o igual a " + inicio + " y menor o igual a " + fin + "\"");
    }
    return "";
}

function jsFecha(inicio, fin, entidad) {
    if (fin == null && inicio != null) {
        return "La " +
            jsLabel("fecha " + entidad, "FiltroCita") +
            " sea mayor o igual a" +
            jsValor("\"" + inicio + "\"");
    }
    if (inicio == null && fin != null) {
        return "La " + jsLabel("fecha " + entidad, "FiltroCita") + " sea mayor o igual a" + jsValor("\"" + fin + "\"");
    }

    if (fin != null && inicio != null) {
        return "La " +
            jsLabel("fecha " + entidad, "FiltroCita") +
            " sea " +
            jsValor("\" mayor o igual a " + inicio + " y menor o igual a " + fin + "\"");
    }
    return "";
}

function jsNormalizarModelo(filtros) {

    var columnas = filtros.Encabezados;
    var reg = new RegExp(",", "g");
    columnas = columnas.replace(reg, ", ");
    var cadena = "Las " + jsLabel("columnas de salida", "FiltroColumnas") + " son: " + jsValor("\"" + columnas + "\"");

    if (filtros.CatTipoTramite !== "") {
        cadena += "Los " +
            jsLabel("tipos de trámite", "FiltroCita") +
            " contengan: " +
            jsValor("\"" + $(".dropdown-toggle[data-id='CatTipoTramite'] ").attr("title") + "\"");
    }
    cadena += jsHora(filtros.HoraInicioCita, filtros.HoraFinCita, "de la cita");
    cadena += jsHora(filtros.HoraInicioRegistro, filtros.HoraFinRegistro, "del registro");
    cadena += jsFecha(filtros.FechaInicioCita, filtros.FechaFinCita, "de la cita");
    cadena += jsFecha(filtros.FechaInicioRegistro, filtros.FechaFinRegistro, "del registro");

    if (filtros.UnidadAtencionId === 0) {
        cadena += "Las " +
            jsLabel("unidades de atención", "FiltroUA") +
            " contengan " +
            jsValor("\"" + "todas" + "\"");
    } else {
        cadena += "Las " +
            jsLabel("unidades de atención", "FiltroUA") +
            " contengan " +
            jsValor("\"" + filtros.Unidad_Atencion + "\"");
    }

    if (filtros.NombrePaciente !== "") {
        cadena += "El " +
            jsLabel("nombre del paciente", "FiltroPaciente") +
            " contenga: " +
            jsValor("\"" + filtros.NombrePaciente + "\"");
    }
    if (filtros.Apellido1Paciente !== "") {
        cadena += "El " +
            jsLabel("apellido paterno del paciente", "FiltroPaciente") +
            " contenga: " +
            jsValor("\"" + filtros.Apellido1Paciente + "\"");
    }
    if (filtros.Apellido2Paciente !== "") {
        cadena += "El " +
            jsLabel("apellido materno del paciente", "FiltroPaciente") +
            " contenga: " +
            jsValor("\"" + filtros.Apellido2Paciente + "\"");
    }
    if (filtros.CURPPaciente !== "") {
        cadena += "La " +
            jsLabel("CURP del paciente", "FiltroPaciente") +
            " contenga: " +
            jsValor("\"" + filtros.CURPPaciente + "\"");
    }
    if (filtros.CorreoPaciente !== "") {
        cadena += "El " +
            jsLabel("correo del paciente", "FiltroPaciente") +
            " contenga: " +
            jsValor("\"" + filtros.CorreoPaciente + "\"");
    }
    if (filtros.TelefonoPaciente !== "") {
        cadena += "El " +
            jsLabel("teléfono del paciente", "FiltroPaciente") +
            " contenga: " +
            jsValor("\"" + filtros.TelefonoPaciente + "\"");
    }
    if (filtros.CelularPaciente !== "") {
        cadena += "El " +
            jsLabel("celular del paciente", "FiltroPaciente") +
            " contenga: " +
            jsValor("\"" + filtros.CelularPaciente + "\"");
    }
    if (filtros.EsMasculinoPaciente) {
        cadena += "El " + jsLabel("sexo del paciente", "FiltroPaciente") + " sea: " + jsValor("\"masculino\"");
    }
    if (filtros.EsMasculinoPaciente === false) {
        cadena += "El " + jsLabel("sexo del paciente", "FiltroPaciente") + " sea: " + jsValor("\"femenino\"");
    }
    if (filtros.EsUsuarioBloqueadoPaciente) {
        cadena += "El " + jsLabel("paciente", "FiltroPaciente") + " este: " + jsValor("\"bloqueado\"");
    }
    if (!filtros.EsUsuarioBloqueadoPaciente === false) {
        cadena += "El " + jsLabel("paciente", "FiltroPaciente") + " no este: " + jsValor("\"bloqueado\"");
    }
    cadena += jsEdad(filtros.EdadInicioPaciente, filtros.EdadFinPaciente, "paciente");

    if (filtros.NombrePromovente !== "") {
        cadena += "El " +
            jsLabel("nombre del promovente", "FiltroPromovente") +
            " contenga: " +
            jsValor("\"" + filtros.NombrePromovente + "\"");
    }
    if (filtros.Apellido1Promovente !== "") {
        cadena += "El " +
            jsLabel("apellido paterno del promovente", "FiltroPromovente") +
            " contenga: " +
            jsValor("\"" + filtros.Apellido1Promovente + "\"");
    }
    if (filtros.Apellido2Promovente !== "") {
        cadena += "El " +
            jsLabel("apellido materno del promovente", "FiltroPromovente") +
            " contenga: " +
            jsValor("\"" + filtros.Apellido2Promovente + "\"");
    }
    if (filtros.CURPPromovente !== "") {
        cadena += "La " +
            jsLabel("CURP del promovente", "FiltroPromovente") +
            " contenga: " +
            jsValor("\"" + filtros.CURPPromovente + "\"");
    }
    if (filtros.CorreoPromovente !== "") {
        cadena += "El " +
            jsLabel("correo del promovente", "FiltroPromovente") +
            " contenga: " +
            jsValor("\"" + filtros.CorreoPromovente + "\"");
    }
    if (filtros.TelefonoPromovente !== "") {
        cadena += "El " +
            jsLabel("teléfono del promovente", "FiltroPromovente") +
            " contenga: " +
            jsValor("\"" + filtros.TelefonoPromovente + "\"");
    }
    if (filtros.CelularPromovente !== "") {
        cadena += "El " +
            jsLabel("celular del promovente", "FiltroPromovente") +
            " contenga: " +
            jsValor("\"" + filtros.CelularPromovente + "\"");
    }
    if (filtros.EsMasculinoPromovente) {
        cadena += "El " + jsLabel("sexo del promovente", "FiltroPromovente") + " sea: " + jsValor("\"masculino\"");
    }
    if (filtros.EsMasculinoPromovente === false) {
        cadena += "El " + jsLabel("sexo del promovente", "FiltroPromovente") + " sea: " + jsValor("\"femenino\"");
    }
    if (filtros.EsUsuarioBloqueadoPromovente) {
        cadena += "El " + jsLabel("promovente", "FiltroPromovente") + " este: " + jsValor("\"bloqueado\"");
    }
    if (!filtros.EsUsuarioBloqueadoPromovente === false) {
        cadena += "El " + jsLabel("promovente", "FiltroPromovente") + " no este: " + jsValor("\"bloqueado\"");
    }
    cadena += jsEdad(filtros.EdadInicioPromovente, filtros.EdadFinPromovente, "promovente");

    $("#PeticionNormalizada").html(cadena);
}

function ValidacionEntero(id) {
    var valor = $("#" + id).val();
    return (isNaN(valor) || valor == null || valor === "") ? 0 : parseInt(valor);
}

function ValidacionHora(id) {

    var valor = $("#" + id).val();
    if (valor == null || valor === "") return null;
    if (IndexOf(valor,":") < 0) return null;
    if (valor.split(":").length !== 2) return null;

    var secciones = valor.split(":");

    if (secciones[0] === "" || isNaN(secciones[0])) return null;
    if (secciones[1] === "" || isNaN(secciones[1])) return null;

    var hora = parseInt(secciones[0]);
    var minuto = parseInt(secciones[0]);
    hora = new String(hora);
    minuto = new String(minuto);

    if (hora.length === 1) hora = "0" + hora;
    if (minuto.length === 1) minuto = "0" + minuto;

    return "" + hora + ":" + minuto;
}

function ValidacionFecha(id) {

    var valor = $("#" + id).val();
    if (valor == null || valor === "") return null;
    if (IndexOf(valor,"/") < 0) return null;
    if (valor.split("/").length !== 3) return null;
    var secciones = valor.split("/");

    if (secciones[0] === "" || isNaN(secciones[0])) return null;
    if (secciones[1] === "" || isNaN(secciones[1])) return null;
    if (secciones[2] === "" || isNaN(secciones[2])) return null;

    var dia = parseInt(secciones[0]);
    var mes = parseInt(secciones[1]);
    var anio = parseInt(secciones[2]);

    if (dia < 0 || dia > 31) {
        return null;
    }
    if (mes < 0 || mes > 12) {
        return null;
    }
    dia = new String(dia);
    if (dia.length === 1) dia = "0" + dia;

    mes = new String(mes);
    if (mes.length === 1) mes = "0" + mes;

    return "" + dia + "/" + mes + "/" + anio;
}

function ValidacionSelectNull(id) {
    var valor = $("#" + id).val();
    var res = false;
    if (valor == -1) res = null;
    else if (valor == 1) res = true;
    return res;
}

function ValidacionSelectMultiple(id) {
    var items = [];
    $("#" + id + " option:selected").each(function() { items.push($(this).val()); });
    return items.join(",");
}