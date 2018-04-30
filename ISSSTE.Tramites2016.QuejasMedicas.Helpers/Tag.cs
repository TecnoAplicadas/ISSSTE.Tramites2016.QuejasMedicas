using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Helpers
{
    public class Tag
    {
        /// <summary>
        ///     Col
        /// </summary>
        /// <param name="Espacios"></param>
        /// <returns></returns>
        public static string Col(int Espacios)
        {
            return " col-md-FACTOR col-sm-FACTOR col-lg-FACTOR col-xs-FACTOR ".Replace("FACTOR", "" + Espacios);
        }

        /// <summary>
        ///     Offset
        /// </summary>
        /// <param name="Espacios"></param>
        /// <returns></returns>
        public static string Offset(int Espacios)
        {
            return " col-md-FACTOR col-sm-FACTOR col-lg-FACTOR col-xs-FACTOR ".Replace("FACTOR", "offset-" + Espacios);
        }

        public static MvcHtmlString Height(int Alto)
        {
            return MvcHtmlString.Create("<div class='row' style='height: " + Alto + "px'></div>");
        }

        public static MvcHtmlString LineaInstitucional()
        {
            return MvcHtmlString.Create("<hr class='red' style='margin-top:-20px; margin-bottom:20px;'>");
        }

        public static MvcHtmlString LineaDeColor(int Alto, string Color)
        {
            return MvcHtmlString.Create("<div class='row' style='height: " + Alto + "px; background-color:" + Color +
                                        "'>&nbsp;</div>");
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private static string ConvertAsciiToHex(string Hexstring)
        {
            string[] escapados =
            {
                "%25", "%21", "%23", "%24", "%26", "%28", "%29", "%3D", "%3F", "%27", "%A1", "%BF", "%A8", "%B4",
                "%7E", "%5B", "%7B", "%5E", "%5D", "%7D", "%60", "%2C", "%3B", "%3A", "%22"
            };

            string[] convertidos =
            {
                "%", "!", "#", "$", "&", "(", ")", "=", "?", "'", "¡", "¿", "¨", "´", "~", "[",
                "{", "^", "]", "}", "`", ",", ";", ":", "\""
            };

            var indice = 0;
            foreach (var elem in convertidos)
            {
                Hexstring = Hexstring.Replace(elem, escapados[indice]);
                indice++;
            }
            return Hexstring;
        }

        private static void ElementosGenerales(ref string Id, object Valor, int MaxLength, bool EsObligatorio,
            string MensajeCampoNoValido,ref string Clase, string ToolTip, ref string Atributos, 
            out string SpanToolTip, out string SpanObligatorio,out string Value, out string LeyendaObligatorio)
        {
            if (string.IsNullOrEmpty(Clase)) Clase = "";

            Clase += " ns_";
            SpanToolTip = !string.IsNullOrEmpty(ToolTip)
                ? "<span class='glyphicon glyphicon-question-sign' onmouseover='tooltip.show(\"" + ToolTip +
                  "\")' onmouseout='tooltip.hide();'></span>"
                : "";
            SpanObligatorio = EsObligatorio ? "<span id='S_div_" + Id + "' class='CampoRequerido'>* </span>" : "";
            Id = string.IsNullOrEmpty(Id) ? "Id_" + DateTime.Now.Ticks : Id;
            Value = Valor == null ? "" : "" + Valor;
            if (MaxLength > 0) Atributos += " maxlength='" + MaxLength + "' ";

            LeyendaObligatorio = EsObligatorio
                ? "<div class='CampoRequerido CampoAux hidden' id='div_" + Id + "'>Este campo es obligatorio</div>"
                : "<div class='CampoAux'>&nbsp;&nbsp;</div>";
            LeyendaObligatorio = LeyendaObligatorio+"<div class='CampoRequerido CampoAux hidden' id='div_Novalido" + Id + "'>"+ MensajeCampoNoValido + "</div>";
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        ///     Botón sin reglas de negocio
        /// </summary>
        /// <param name="Clase"></param>
        /// <param name="Tooltip"></param>
        /// <param name="OnClick"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static MvcHtmlString InputTooltip(string Clase, string Tooltip, string Id = "", string OnClick = "")
        {
            if (string.IsNullOrEmpty(Id)) Id = "Id_" + DateTime.Now.Ticks;

            if (string.IsNullOrEmpty(OnClick) && !string.IsNullOrEmpty(Id)) OnClick = Id + "();";

            return MvcHtmlString.Create("<button type='button' class='btn btn-default btn-sm' " +
                                        " ondblclick ='tooltip.hide();" + OnClick + "' " +
                                        " onclick    ='tooltip.hide();" + OnClick + "' value='" + Tooltip +
                                        "'  onmouseover='tooltip.show(\"" + Tooltip +
                                        "\")' onmouseout='tooltip.hide();' id='" + Id + "'>" +
                                        "<span class='" + Clase + "' id='span_" + Id +
                                        "' style='left: 0px; position: relative;top:2px;' aria-hidden='true'></span>" +
                                        "</button>");
        }

        public static MvcHtmlString InputBoton(string Id, string Evento, int Prioridad, int TamanO = 3,
            string OnClick = "", string Icono = "", string ToolTip = "", string UidCall = "", string Clase = "")
        {
            var input = BotonImpl(Id, Evento, Prioridad, TamanO, OnClick, Icono, ToolTip, UidCall, Clase);
            return MvcHtmlString.Create(input);
        }

        private static string BotonImpl(string Id, string Evento, int Prioridad, int TamanO = 3, string OnClick = "",
            string Icono = "", string ToolTip = "", string UidCall = "", string Clase = "")
        {
            if (string.IsNullOrEmpty(Id)) Id = "Id_" + DateTime.Now.Ticks;
            var claseBtn = string.IsNullOrEmpty(Clase) ? " btn " : "btn " + Clase + " ";

            switch (Prioridad)
            {
                case 1:
                    claseBtn += "btn-primary ";
                    break; // Principal
                case 2:
                    claseBtn += "btn-danger ";
                    break; // Precaución
                case 3:
                    claseBtn += "btn-default ";
                    break; // Inactivo
                case 4:
                    claseBtn += "btn-blank ";
                    break; // Blanco
            }

            switch (TamanO)
            {
                case 1:
                    claseBtn += "btn-xs ";
                    break; // Extra chico
                case 2:
                    claseBtn += "btn-sm ";
                    break; // Chico
                case 4:
                    claseBtn += "btn-lg ";
                    break; // Grande
            }

            var uidCallEvent = !string.IsNullOrEmpty(UidCall) ? @"uid_call(" + UidCall + ")" : string.Empty;


            if (string.IsNullOrEmpty(Id)) Id = "Id_" + DateTime.Now.Ticks;
            OnClick = string.IsNullOrEmpty(OnClick) ? Id + "();" : OnClick + "();";

            var toolTipHide = !string.IsNullOrEmpty(ToolTip) ? "tooltip.hide();" : "";
            var toolTipShow = !string.IsNullOrEmpty(ToolTip)
                ? " onmouseover='tooltip.show(\"" + ToolTip + "\")' onmouseout='" + toolTipHide + "' "
                : "";

            if (Id.ToUpper().Contains("ELIM"))
            {
                OnClick = ConvertAsciiToHex(OnClick);
                OnClick = "jsConfirmar('¿Estas seguro de " + Evento + "?','Cancelación de tu cita',' " + OnClick + "', true);" + uidCallEvent;
            }
            else
            {
                OnClick += uidCallEvent;
            }

            if (string.IsNullOrEmpty(toolTipShow))
                toolTipShow = " onmouseover='' onmouseout='' ";

            return "<button type='button' style='text-decoration:none;' class='" + claseBtn + "' " + toolTipShow +
                   " ondblclick = \"" + toolTipHide + OnClick + " \" " +
                   " onclick    = \"" + toolTipHide + OnClick + " \" " + " id=\"" + Id + "\">" +
                   "<span id='Span_" + Id + "' class=' " + Icono + "' aria-hidden='true'></span>&nbsp;&nbsp;" +
                   Evento +
                   "</button>";
        }

        public static MvcHtmlString InputDiv(int Columnas, string PlaceHolder, string Id, object Valor, int MaxLength,
        string MensajeCampoNoValido,bool EsObligatorio = false,string Clase = "", string ToolTip = "", string Atributos = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, MaxLength, EsObligatorio, MensajeCampoNoValido, ref Clase, ToolTip, ref Atributos,
                out spanToolTip, out spanObligatorio, out value, out leyendaObligatorio);

            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var cadena = "<div id='Div_"+Id+ "' class='col-if-12 " + Col(Columnas) + "'>" +
                         "<div class='form-group ' id='FG_" + Id + "'>" +
                         "<label class='control-label'>" + PlaceHolder + spanObligatorio + ": " + spanToolTip +
                         "</label>" +
                         "<div " + Atributos + " id='" + Id + "' class='" + campoObligatorio + " form-control " + Clase +
                         "' placeholder='" + PlaceHolder + "'>" + value + "</div>" +
                         "</div>" +
                         "</div>";
            return MvcHtmlString.Create(cadena);
        }

        public static MvcHtmlString InputTexto(int Columnas,string  NombreCampo, string PlaceHolder, string Id, object Valor, int MaxLength,
            string MensajeCampoNoValido, bool EsObligatorio = false, string Clase = "", string ToolTip = "", string Atributos = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, MaxLength, EsObligatorio, MensajeCampoNoValido, ref Clase, ToolTip, ref Atributos,
                out spanToolTip, out spanObligatorio, out value, out leyendaObligatorio);

            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var cadena = "<div class='" + Col(Columnas) + "'>" +
                         "<div class='form-group' id='FG_" + Id + "'>" +
                         "<label class='control-label'>" + NombreCampo + spanObligatorio + ": " + spanToolTip +
                         "</label>" +
                         "<input " + Atributos + " id='" + Id + "' value='" + value + "' class='" + campoObligatorio +
                         " form-control " + Clase + "' placeholder='" + PlaceHolder + "' type='text'>" +
                         leyendaObligatorio +
                         "</div>" +
                         "</div>";
            return MvcHtmlString.Create(cadena);
        }



        public static MvcHtmlString InputTextoArea(int Columnas, string PlaceHolder, string Id, object Valor,
            int MaxLength, string MensajeCampoNoValido, bool EsObligatorio, int Rows, int Cols, string Clase = "", string ToolTip = "",
            string Atributos = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, MaxLength, EsObligatorio, MensajeCampoNoValido, ref Clase, ToolTip, ref Atributos,
                out spanToolTip, out spanObligatorio, out value, out leyendaObligatorio);

            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var cadena = "<div class='" + Col(Columnas) + "'>" +
                         "<div class='form-group'id='FG_" + Id + "'>" +
                         "<label class='control-label'>" + PlaceHolder + spanObligatorio + ": " + spanToolTip +
                         "</label>" +
                         "<textarea " + Atributos + " id='" + Id + "' value='" + value + "' class='" +
                         campoObligatorio + "form-control " + Clase + "' placeholder='" + PlaceHolder + "' rows='" +
                         Rows + "' cols='" + Cols + "'>" + Valor + "</textarea>" + leyendaObligatorio +
                         "</div>" +
                         "</div>";
            return MvcHtmlString.Create(cadena);
        }

        public static MvcHtmlString InputTextoCheck(int Columnas, string PlaceHolder, string Id, object Valor,
            string Clase = "", string ToolTip = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio, atributos = "";
            ElementosGenerales(ref Id, Valor, 0, false, string.Empty, ref Clase, ToolTip, ref atributos, out spanToolTip,
                out spanObligatorio, out value, out leyendaObligatorio);

            bool objeto;
            var result = bool.TryParse("" + Valor, out objeto);
            if (result && objeto)
                atributos = " checked='checked' ";

            var cadena = "<div class='" + Col(Columnas) + "'>" +
                         "<div class='form-group' id='FG_" + Id + "'>" +
                         "<label class='control-label'>" + PlaceHolder + ": " + spanToolTip + "</label>" +
                         "<center><input id='" + Id + "' " + atributos + " class='form-control " + Clase +
                         "' style='width: 20px;' type='checkbox'></input></center>" +
                         "</div>" +
                         "</div>";
            return MvcHtmlString.Create(cadena);
        }

        public static MvcHtmlString InputSelect(int Columnas, string PlaceHolder, string Id, string Clase,
            List<DuplaValoresDTO> Combo, bool EsObligatorio = true, string Complemento = "", string ToolTip = "")
        {
            if (string.IsNullOrEmpty(Complemento)) Complemento = "";
            if (string.IsNullOrEmpty(Clase)) Clase = "";
            Clase += " form-control ";

            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var spanToolTip = !string.IsNullOrEmpty(ToolTip)
                ? "<span class='glyphicon glyphicon-question-sign' onmouseover='tooltip.show(\"" + ToolTip +
                  "\")' onmouseout='tooltip.hide();'></span>"
                : "";
            var spanObligatorio = EsObligatorio ? "<span class='CampoRequerido' id='S_div_" + Id + "'>* </span>" : "";
            Id = string.IsNullOrEmpty(Id) ? "Id_" + DateTime.Now.Ticks : Id;
            if (string.IsNullOrEmpty(Clase)) Clase = "";

            var leyendaObligatorio = EsObligatorio
                ? "<div class='CampoRequerido CampoAux hidden' id='div_" + Id + "'>Este campo es obligatorio</div>"
                : "<div class='CampoAux'>&nbsp;&nbsp;</div>";

            var html = GeneracionCuerpoSelect(false, Combo);

            var select = "<center><select  " + Complemento + " id='" + Id + "' class='" + Clase + campoObligatorio +
                         "'>" + html + "</select></center>";

            select = "<div class='" + Col(Columnas) + "'>" +
                     "<div class='form-group' id='FG_" + Id + "'>" +
                     "<label class='control-label'>" + PlaceHolder + spanObligatorio + ": " + spanToolTip + "</label>" +
                     select + leyendaObligatorio +
                     "</div>" +
                     "</div>";

            return MvcHtmlString.Create(select);
        }

        public static MvcHtmlString InputTextoDate(int Columnas, string PlaceHolder, string Id, object Valor,
            bool EsObligatorio, string MensajeCampoNoValido, bool ChangeYear = false, string Clase = "", string ToolTip = "", string Atributos = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, 0, EsObligatorio, MensajeCampoNoValido, ref Clase, ToolTip, ref Atributos, out spanToolTip,
                out spanObligatorio, out value, out leyendaObligatorio);
            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var cadena = "<div class='" + Col(Columnas) + "'>" +
                         "<div class='form-group datepicker-group'>" +
                         "<label class='control-label'>" + PlaceHolder + spanObligatorio + ": " + spanToolTip +
                         "</label>" +
                         "<input " + Atributos + " id='" + Id + "' value='" + value + "' class='" + campoObligatorio +
                         "form-control " + Clase + "' placeholder='" + PlaceHolder + "' type='text'>" +
                         "<span class='glyphicon glyphicon-calendar' aria-hidden='true'></span>" + leyendaObligatorio +
                         "</div>" +
                         "</div>" + "<script type='text/javascript'>$gmx(document).ready(function() {$('#" + Id +
                         "').datepicker({changeYear: " + ("" + ChangeYear).ToLower() + " });});</script>";
            return MvcHtmlString.Create(cadena);
        }


        /// <summary>
        ///     Este se utiliza para devolver las opciones del select construido....
        /// </summary>
        /// <param name="SinTodos"></param>
        /// <param name="Valores"></param>
        /// <returns></returns>
        public static string GeneracionCuerpoSelect(bool SinTodos, List<DuplaValoresDTO> Valores)
        {
            var html = new StringBuilder();
            if (!SinTodos)
                html.Append("<option value = '0' data-auxid='0' >Selecciona</option>");

            if (Valores != null)
            {
                Valores = Valores.OrderBy(S => S.Valor).ToList();
                foreach (var attr in Valores)
                    if (!string.IsNullOrEmpty(attr?.Valor))
                    {
                        html.Append("<option value = '" + attr.Id + "' "+ (attr.IdAux!=0? "data-auxid='"+ attr.IdAux + "' ":string.Empty) +"");
                        if (attr.Selected)
                            html.Append(" selected='selected' ");
                        html.Append(">" + attr.Valor.ToUpper());
                        html.Append("</option>");
                    }
            }

            return html.ToString();
        }
    }
}