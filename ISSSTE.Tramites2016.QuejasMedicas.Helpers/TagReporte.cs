using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using ISSSTE.Tramites2016.QuejasMedicas.Model.UtilDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Helpers
{
    public class TagReporte
    {
        private static readonly string ClaseCol = " col-md-FACTOR col-sm-FACTOR col-lg-FACTOR col-xs-FACTOR ";

        /// <summary>
        ///     Col
        /// </summary>
        /// <param name="Espacios"></param>
        /// <returns></returns>
        public static string Col(int Espacios)
        {
            return ClaseCol.Replace("FACTOR", "" + Espacios);
        }

        /// <summary>
        ///     Offset
        /// </summary>
        /// <param name="Espacios"></param>
        /// <returns></returns>
        public static string Offset(int Espacios)
        {
            return ClaseCol.Replace("FACTOR", "offset-" + Espacios);
        }

        public static MvcHtmlString Height(int Alto)
        {
            return MvcHtmlString.Create("<div class='row' style='height: " + Alto + "px'></div>");
        }

        public static MvcHtmlString LineaDeColor(int Alto, string Color)
        {
            return MvcHtmlString.Create("<div class='row' style='height: " + Alto + "px; background-color:" + Color +
                                        "'>&nbsp;</div>");
        }

        private static void ElementosGenerales(ref string Id, object Valor, int MaxLength, bool EsObligatorio,
            ref string Clase, string ToolTip, ref string Atributos, out string SpanToolTip, out string SpanObligatorio,
            out string Value, out string LeyendaObligatorio)
        {
            SpanToolTip = !string.IsNullOrEmpty(ToolTip)
                ? "<span class='glyphicon glyphicon-question-sign' onmouseover='tooltip.show(\"" + ToolTip +
                  "\")' onmouseout='tooltip.hide();'></span>"
                : "";
            SpanObligatorio = EsObligatorio ? "<span class='CampoRequerido'>* </span>" : "";
            Id = string.IsNullOrEmpty(Id) ? "Id_" + DateTime.Now.Ticks : Id;
            if (string.IsNullOrEmpty(Clase)) Clase = "";
            Value = Valor == null ? "" : "" + Valor;
            if (MaxLength > 0) Atributos += " maxlength='" + MaxLength + "' ";

            LeyendaObligatorio = EsObligatorio
                ? "<div class='CampoRequerido CampoAux hidden' id='div_" + Id + "'>Este campo es obligatorio</div>"
                : "<div class='CampoAux'>&nbsp;&nbsp;</div>";
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public static MvcHtmlString InputTexto(int Columnas, string PlaceHolder, string Id, object Valor, int MaxLength,
            bool EsObligatorio = false, string Clase = "", string ToolTip = "", string Atributos = "",
            string Display = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, MaxLength, EsObligatorio, ref Clase, ToolTip, ref Atributos,
                out spanToolTip, out spanObligatorio, out value, out leyendaObligatorio);

            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var cadena = "<div class='DivGrupo " + Col(Columnas) + "' id='DivGrupo_" + Id + "' style='display:" +
                         Display + "'>" +
                         "<div class='form-group' id='FG_" + Id + "'>" +
                         "<i class='glyphicon glyphicon-remove-circle pull-right closeMaker' aria-hidden='true' id='Close_" +
                         Id + "' ></i>" +
                         "<label class='control-label label_Titulo' Id='Label_" + Id + "'>" + PlaceHolder +
                         spanObligatorio + ": " + spanToolTip + "</label>" +
                         "<input " + Atributos + " id='" + Id + "' value='" + value + "' class='" + campoObligatorio +
                         "form-control label_better " + Clase + "' placeholder='" + PlaceHolder + "' type='text'>" +
                         leyendaObligatorio +
                         "</div>" +
                         "</div>";

            return MvcHtmlString.Create(cadena);
        }

        public static MvcHtmlString InputTextoArea(int Columnas, string PlaceHolder, string Id, object Valor,
            int MaxLength, bool EsObligatorio, int Rows, int Cols, string Clase = "", string ToolTip = "",
            string Atributos = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, MaxLength, EsObligatorio, ref Clase, ToolTip, ref Atributos,
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
            ElementosGenerales(ref Id, Valor, 0, false, ref Clase, ToolTip, ref atributos, out spanToolTip,
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
            bool EsObligatorio = true, string Complemento = "", string ToolTip = "", string Display = "",
            bool ConSelectPicker = true)
        {
            if (string.IsNullOrEmpty(Complemento)) Complemento = "";
            if (string.IsNullOrEmpty(Clase)) Clase = "";
            Clase += " form-control label_better ";

            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var spanToolTip = !string.IsNullOrEmpty(ToolTip)
                ? "<span class='glyphicon glyphicon-question-sign' onmouseover='tooltip.show(\"" + ToolTip +
                  "\")' onmouseout='tooltip.hide();'></span>"
                : "";
            var spanObligatorio = EsObligatorio ? "<span class='CampoRequerido'>* </span>" : "";
            Id = string.IsNullOrEmpty(Id) ? "Id_" + DateTime.Now.Ticks : Id;

            var leyendaObligatorio = EsObligatorio
                ? "<div class='CampoRequerido CampoAux hidden' id='div_" + Id + "'>Este campo es obligatorio</div>"
                : "<div class='CampoAux'>&nbsp;&nbsp;</div>";

            var select = "<center><select  " + Complemento + " id='" + Id + "' class=' " +
                         (ConSelectPicker ? "selectpicker" : "") + "  " + campoObligatorio + Clase +
                         "'></select></center>";

            select = "<div class='DivGrupo " + Col(Columnas) + "' id='DivGrupo_" + Id + "' style='diplay:" + Display +
                     "'>" +
                     "<div class='form-group' id='FG_" + Id + "'>" +
                     "<i class='glyphicon glyphicon-remove-circle pull-right closeMaker' aria-hidden='true' id='Close_" +
                     Id + "' ></i>" +
                     "<label class='control-label label_Titulo' Id='Label_" + Id + "'>" + PlaceHolder +
                     spanObligatorio + ": " + spanToolTip + "</label>" +
                     select + leyendaObligatorio +
                     "</div>" +
                     "</div>";

            return MvcHtmlString.Create(select);
        }

        public static MvcHtmlString InputTextoDate(int Columnas, string PlaceHolder, string Id, object Valor,
            bool EsObligatorio, bool ChangeYear = false, string Clase = "", string ToolTip = "", string Atributos = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, 0, EsObligatorio, ref Clase, ToolTip, ref Atributos, out spanToolTip,
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

        public static MvcHtmlString InputRango(int Columnas, string PlaceHolder1,string PlaceHolder2, string Id, object Valor, int MaxLength,
            bool EsObligatorio = false, string Clase = "", string ToolTip = "", string Atributos = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, MaxLength, EsObligatorio, ref Clase, ToolTip, ref Atributos,
                out spanToolTip, out spanObligatorio, out value, out leyendaObligatorio);

            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var cadena = "<div class='DivGrupo"  + Col(Columnas) +"' id='DivGrupo_" + Id + "' style='display:inline'>" +
                             "<div id='FG_" + Id +"' class='" + Col(12) + "'><i class='glyphicon glyphicon-remove-circle pull-right closeMaker' " + "aria-hidden='true' + id='Close_" + Id+"' ></i>"+
                            //"<label class='control-label label_Titulo' id='Label_" + Id + "' " +
                            // //"style='color: rgb(46, 154, 254);
                            // "'>" +
                            // PlaceHolder + spanObligatorio + ": " + spanToolTip + "</label>" +
                            "</div>"+
                             "<div class='TopGrupo " + Col(6) + "' style='padding-left:0px'>" +
                                 "<div class='form-group'>"+
                                     "<label id='Label_" + Id + "1' " +">"+ PlaceHolder1 + spanObligatorio + ": " + spanToolTip +"</label>" +
                                     "<input " + Atributos + " id='" + Id + "' value='" + value + "' class='" + campoObligatorio +"form-control label_better " + Clase + "' placeholder='" + PlaceHolder1 + "' type='text'>" +
                                  "</div>" +
                             "</div>" +
                             "<div class='TopGrupo " + Col(6) + "' style='padding-right:0px'>" +
                                 "<div class='form-group'>" +
                                     "<label id='Label_" + Id + "2' " + ">"+ PlaceHolder2 + spanObligatorio + ": " + spanToolTip +"</label>" +
                                     "<input " + Atributos + " id='" + Id + "1' value='" + value + "' class='" + campoObligatorio +"form-control label_better " + Clase + "' placeholder='" + PlaceHolder2 + "' type='text'>" +
                                  "</div>" +
                             "</div>" +

                         "</div>";

            return MvcHtmlString.Create(cadena);
        }

        public static MvcHtmlString InputRangoFecha(int Columnas, string PlaceHolder1, string PlaceHolder2, string Id, object Valor,
            int MaxLength, bool EsObligatorio = false, string Clase = "", string ToolTip = "", string Atributos = "")
        {
            string spanToolTip, spanObligatorio, value, leyendaObligatorio;
            ElementosGenerales(ref Id, Valor, MaxLength, EsObligatorio, ref Clase, ToolTip, ref Atributos,
                out spanToolTip, out spanObligatorio, out value, out leyendaObligatorio);

            var campoObligatorio = EsObligatorio ? " CampoObligatorio " : "";

            var cadena = "<div class='DivGrupo"  + Col(Columnas) +"' id='DivGrupo_" + Id + "' style='display:inline'>" +
                            "<div  id='FG_" + Id + "' class='" + Col(12) + "' >" + "<i class='glyphicon glyphicon-remove-circle pull-right closeMaker'" + "aria-hidden='true' id='Close_" + Id +"' ></i>"+
                            //"<label class='control-label label_Titulo' id='Label_" + Id + "' " +
                            ////"style='color: rgb(46, 154, 254)';
                            //">" + 
                            //spanObligatorio + ": " + spanToolTip +
                            //"</label>" +
                            "</div>" +
                            "<div class='TopGrupo " + Col(6) + "' style='padding-left:0px'>" +
                                "<div class='form-group'>" +
                                "<label id='Label_" + Id + "1' " +">"+ PlaceHolder1 + spanObligatorio + ": " + spanToolTip + "</label>" +
                                "<div class='input-group'><span class='input-group-addon glyphicon glyphicon-calendar' id='Calendar_" +
                                Id + "'></span><input " + Atributos + " id='" + Id + "' value='" + value + "' class='" +
                                campoObligatorio + "form-control label_better " + Clase + "' placeholder='" + PlaceHolder1 +
                                "' type='text' ></div>" +
                                "</div>" +
                            "</div>" +

                            "<div class='TopGrupo " + Col(6) + "' style='padding-right:0px'>" +
                                "<div class='form-group'>" +
                                "<label id='Label_" + Id + "2'" + ">" + PlaceHolder2 + spanObligatorio + ": " + spanToolTip + "</label>" +
                                "<div class='input-group'><span class='input-group-addon glyphicon glyphicon-calendar' id='Calendar_" +
                                Id + "1'></span><input " + Atributos + " id='" + Id + "1' value='" + value + "' class='" +
                                campoObligatorio + "form-control label_better " + Clase + "' placeholder='" + PlaceHolder2 +
                                "' type='text'></div>" +
                                "</div>" +
                            "</div>" +

                        "</div>";

            return MvcHtmlString.Create(cadena);
        }

        /// <summary>
        ///     Este se utiliza para devolver las opciones del select construido....
        /// </summary>
        /// <param name="Valores"></param>
        /// <returns></returns>
        public static string GeneracionCuerpoSelect(List<DuplaValoresDTO> Valores)
        {
            var html = new StringBuilder();
            html.Append("<option value = '0'>");
            html.Append("Selecciona");
            html.Append("</option>");

            if (Valores != null)
            {
                Valores = Valores.OrderBy(S => S.Valor).ToList();
                foreach (var attr in Valores)
                {
                    html.Append("<option value = '" + attr.Id + "' ");
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