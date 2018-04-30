using System.Web.Mvc;

namespace ISSSTE.Tramites2016.QuejasMedicas.Helpers
{
    public class Paginador
    {
        public static MvcHtmlString SeccionPaginacion(string IdTabla, bool PaginadorControlador = true)
        {
            var cadena =
                "<section class='paginacion' id='Paginacion_" + IdTabla + "' data-controlador='" +
                PaginadorControlador + "' style='width: 1200px;'> " +
                "<ul>" +
                "<li class='disabled' id='ZonaAnt_" + IdTabla + "' ><a onclick='jsAnterior(\"" + IdTabla +
                "\")' ><i class='fa fa-angle-double-left'></i>&nbsp;Anterior</a></li>" +
                "<li><a class='active disabled Statics'><p>P&aacute;gina</p>&nbsp;<p id='PagAct_" + IdTabla +
                "'>0</p>&nbsp;<p>de</p>&nbsp;<p id='PagTotal_" + IdTabla + "'>0</p></a></li>" +
                "<li>" +
                "<div class='input-group BusquedaPaginador' style='width: 100px;'>" +
                "<span onmouseover='tooltip.show(\"Proporcionar el n&uacute;mero de p&aacute;gina y enter\")' onmouseout='tooltip.hide();' " +
                " class='SearchIcon input-group-addon'><i class='fa fa-search'></i></span> " +
                "<input type='text' style='width:60px;' onkeypress='return KeyBusqueda(event,\"" + IdTabla +
                "\")' id='Captura_" + IdTabla + "' maxlength='3' class='Numero form-control'>" +
                "</div>" +
                "</li>" +
                "<li class='SelectBusqueda'>" +
                "<select id='Select_" + IdTabla + "' class='form-control SelectSearch' onchange='KeyChangeSelect(\"" +
                IdTabla + "\")'>" +
                "<option value='5'>5</option>" +
                "<option value='10'>10</option>" +
                "<option value='20'>20</option>" +
                "<option value='30'>30</option>" +
                "<option value='40'>40</option>" +
                "<option value='50'>50</option>" +
                "<option value='100'>100</option>" +
                "</select>" +
                "</li>" +
                "<li class='disabled' id='ZonaSig_" + IdTabla + "' ><a onclick='jsSiguiente(\"" + IdTabla +
                "\")'>Siguiente&nbsp;<i class='fa fa-angle-double-right'>&nbsp;</i></a></li>" +
                "</ul>" +
                "</section>" +
                "<section class='paginacion'>" +
                "<ul>" +
                "<li class='TotRows'><a class='active disabled ZonaRows'><p>Registros:</p>&nbsp;<p id='Rows_" +
                IdTabla + "'>0</p></a></li>" +
                "<li class='SearchRows'>" +
                "<div class='input-group BusquedaTextoPaginador' style='width: 120px;' >" +
                "<span onmouseover='tooltip.show(\"Buscador de frase por p&aacute;gina, proporcione la frase y enter\")' onmouseout='tooltip.hide();' " +
                " class='SearchBusquedaIcon input-group-addon'><i class='fa fa-paragraph'></i></span> " +
                "<input type='text'style='width:190px;' onkeypress='return KeyBusquedaFrase(event,\"" + IdTabla +
                "\")' id='Busqueda_" + IdTabla + "' maxlength='50' class='form-control BusquedaPredictiva'>" +
                "</div>" +
                "</li>" +
                "</ul>" +
                "</section>";

            return MvcHtmlString.Create(cadena);
        }
    }
}