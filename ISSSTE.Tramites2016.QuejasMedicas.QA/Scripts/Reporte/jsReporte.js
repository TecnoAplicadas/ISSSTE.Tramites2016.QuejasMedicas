var G_FiltrosQuery = {};

var G_Modelo = {
    CatTipoTramite: "",
    TramiteUnidadAtencion: "",
    Unidad_Atencion: "",
    HoraInicioCita: "",
    HoraInicioRegistro: "",
    HoraFinCita: "",
    HoraFinRegistro: "",
    FechaInicioCita: "",
    FechaInicioRegistro: "",
    FechaFinCita: "",
    FechaFinRegistro: "",
    NombrePaciente: "",
    Apellido1Paciente: "",
    Apellido2Paciente: "",
    CURPPaciente: "",
    CorreoPaciente: "",
    TelefonoPaciente: "",
    CelularPaciente: "",
    NombrePromovente: "",
    Apellido1Promovente: "",
    Apellido2Promovente: "",
    CURPPromovente: "",
    CorreoPromovente: "",
    TelefonoPromovente: "",
    CelularPromovente: "",
    EsMasculinoPaciente: "",
    EsMasculinoPromovente: "",
    EsUsuarioBloqueadoPaciente: "",
    CatTipoEdoCita: "",
    EdadInicioPaciente: "",
    EdadFinPaciente: "",
    EdadInicioPromovente: "",
    EdadFinPromovente: ""
};

var G_Objetos = {
    FechaRegistroCad: "Fecha de la solicitud",
    HoraSolicitud: "Hora de la solicitud",
    FechaCitaCad: "Fecha de la cita",
    HoraCita: "Hora de la cita",
    EstadoCita: "Estado de la cita",
    EstadoTramite: "Trámite",
    ConceptoUnidad: "Unidad de atención",
    NombrePaciente: "Nombre del paciente",
    Apellido1Paciente: "Apellido paterno del paciente",
    Apellido2Paciente: "Apellido materno del paciente",
    EdadPaciente: "Edad del paciente",
    CorreoPaciente: "Correo electrónico del paciente",
    TelefonoPaciente: "Teléfono del paciente",
    CelularPaciente: "Celular del paciente",
    EsUsuarioBloqueadoPaciente: "El paciente esta bloqueado",
    NombrePromovente: "Nombre del promovente",
    Apellido1Promovente: "Apellido paterno del promovente",
    Apellido2Promovente: "Apellido materno del promovente",
    EdadPromovente: "Edad del promovente",
    CorreoPromovente: "Correo electrónico del promovente",
    TelefonoPromovente: "Teléfono del promovente",
    CelularPromovente: "Celular del promovente",
    CURPPromovente: "CURP del promovente",
    CURPPaciente: "CURP del paciente",
    GeneroPaciente: "Sexo del paciente",
    GeneroPromovente: "Sexo del promovente"
};

function jsCrearCompendioExcel() {

    var url = "../Administrador/jsCrearCompendioExcel";

    $("#formaXLS").remove();
    $("body").append('<form action="' + url + '" method="post" id="formaXLS" name="formaXLS"></form>');

    var auxiliar = $("#PeticionNormalizada").html();
    if (auxiliar != null) {
        auxiliar = auxiliar.replace(/Á/g, "&Aacute;");
        auxiliar = auxiliar.replace(/É/g, "&Eacute;");
        auxiliar = auxiliar.replace(/Í/g, "&Iacute;");
        auxiliar = auxiliar.replace(/Ó/g, "&Oacute;");
        auxiliar = auxiliar.replace(/Ú/g, "&Uacute;");
        auxiliar = auxiliar.replace(/á/g, "&aacute;");
        auxiliar = auxiliar.replace(/é/g, "&eacute;");
        auxiliar = auxiliar.replace(/í/g, "&iacute;");
        auxiliar = auxiliar.replace(/ó/g, "&oacute;");
        auxiliar = auxiliar.replace(/ú/g, "&uacute;");
        auxiliar = auxiliar.replace(/Ü/g, "&Uuml;");
        auxiliar = auxiliar.replace(/ü/g, "&uuml;");
        auxiliar = auxiliar.replace(/Ñ/g, "&Ntilde;");
        auxiliar = auxiliar.replace(/ñ/g, "&ntilde;");
    }

    var form = $("#formaXLS");

    form.append(JsParametroEnvio("JsonFiltros", JSON.stringify(G_FiltrosQuery)));
    form.append(JsParametroEnvio("Encabezados", G_FiltrosQuery.Encabezados));
    form.append(JsParametroEnvio("Caption", escape(auxiliar)));
    form.append(JsParametroEnvio("Llaves", G_FiltrosQuery.Llaves));
    form.attr("action", url);
    form.attr("method", "POST");
    form.submit();
}

function jsImpresionCompendio() {
    var encabezados = ObtenerEncabezados();
    if (G_FiltrosQuery.Encabezados != null && G_FiltrosQuery.Encabezados !== "" && G_FiltrosQuery.Encabezados !== "") {
        var llaves = ObtenerLlaves();
        jsCrearCompendioExcel(encabezados, llaves);
    } else {
        jsResultado("Necesita primero consultar un reporte para poder generar el Excel", "PRECAUCION");
    }
}

function ObtenerLlaves(palabra) {
    if (palabra == null) palabra = "";
    var objetosSeleccion = palabra.split(",");
    var aux = "";
    for (var k in objetosSeleccion) {
        if (objetosSeleccion.hasOwnProperty(k)) {
            if (aux === "") aux += G_Objetos[objetosSeleccion[k]];
            else aux += "," + G_Objetos[objetosSeleccion[k]];
        }
    }
    return aux;
}

function ObtenerEncabezados() {
    var cadena = "";
    $(".ListaB .ListaSeleccion").each(function() {
        var clase = $(this).attr("class");
        clase = clase.replace("active", " ").replace("ListaSeleccion", "").replace("list-group-item", "");
        if (cadena === "") cadena += clase.trim();
        else cadena += "," + clase.trim();
    });
    return cadena;
}

function CompendioTable1() {
    G_Modelo = G_FiltrosQuery;
    jsPeticion("CompendioTable1", G_Modelo, true);
    jsAjaxGenerico("../Administrador/CompendioCitas", G_Modelo, "SeccionReporte", null);
    jsTrHover("CompendioTable1");
}