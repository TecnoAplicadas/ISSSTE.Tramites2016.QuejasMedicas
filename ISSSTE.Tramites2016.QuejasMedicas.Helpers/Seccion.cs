using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using ISSSTE.Tramites2016.QuejasMedicas.Model.InformacionDTO;

namespace ISSSTE.Tramites2016.QuejasMedicas.Helpers
{
    public class Seccion
    {
        public static MvcHtmlString ConsultaCita(string Valor1, string Valor2 = "")
        {
            var cuerpo = !string.IsNullOrEmpty(Valor1) && !string.IsNullOrEmpty(Valor2)
                ? "<div class='" + Tag.Col(12) + "'><div class='" + Tag.Col(12) + "'><b>" + Valor1 + ": </b>" + Valor2 +
                  "</div></div>"
                : "<div class='" + Tag.Col(12) + "'><div class='" + Tag.Col(12) + "'>" + Valor1 + "</div></div>";
            return MvcHtmlString.Create(cuerpo);
        }

        public static MvcHtmlString MenuDivs(PromoventeDTO Informacion, int Secccion)
        {
            var i = 0;
            var tabs = new StringBuilder();
            var cuerpoTabs = new StringBuilder();
            var PanelTabs = "<div style='width:100%'>MenuBotones</div>" +
                            "<div class='tab-content col-md-8' >ContenidoInformacion</div>";

            foreach (var seccionDto in Informacion.SeccionPrincipal)
            {
                tabs.Append(BotonImplementacion(seccionDto.Titulo, i));
                cuerpoTabs.Append(CuerpoImplementacion(seccionDto.Detalle, i));
                i++;
            }

            var requisitosTabla = RequisitosPorTramite(Informacion);

            tabs.Append(BotonImplementacion("Requisitos del trámite",i));
            cuerpoTabs.Append(CuerpoImplementacion(requisitosTabla, i));
            
            PanelTabs = PanelTabs.Replace("MenuBotones", tabs.ToString());
            PanelTabs = PanelTabs.Replace("ContenidoInformacion", cuerpoTabs.ToString());

            return MvcHtmlString.Create(PanelTabs);
        }

        private static string CuerpoImplementacion(string Valor, int indice)
        {
            string estilo = (indice == 0) ? "display:inline" : "display:none";
            return "<div class='GeneralCuerpo' id='panel_tab_" + indice + "' style='"+estilo+"'>" + Valor + " </div>";
        }

        private static string BotonImplementacion(string Valor, int indice)
        {
            string estilo = (indice == 0)
                ? "background-color:#4D92DF;color:white"
                : "background-color:#F6F6F6;color:black";
            Valor = (indice == 0) ? "Descripción" : Valor;
            return "<button id='tab_" + indice + "' type='button' style='font-size:16px;text-decoration:none;border:1px solid #DDDDDD;" + estilo+"' class='HeaderBtn btn btn-sm' " +
                         " ondblclick='MuestraDetallePanel(\"tab_" + indice + "\");' " +
                         " onclick='MuestraDetallePanel(\"tab_" + indice + "\");' >" +  
                         Valor + "</button>";
        }


        private static string RequisitosPorTramite(PromoventeDTO Informacion)
        {
            var cuerpoTabla = ConstruyeCuerpoTabla(Informacion.Requisitos);

            return "<table class='table' width='800px'>" +
                                  "<thead>" +
                                  "<tr>" +
                                  "<td><strong>Documento requerido</strong></td>" +
                                  "<td width='150px'><strong>Presentaci&oacute;n</strong></td>" +
                                  "</tr>" +
                                  "</thead>" +
                                  "<tbody>" + cuerpoTabla + "</tbody>" +
                                  "</table>";
        }

        private static StringBuilder ConstruyeCuerpoTabla(List<RequisitoDTO> Requisitos)
        {
            var cuerpoTabla = new StringBuilder();

            foreach (var requisito in Requisitos)
                cuerpoTabla.Append(
                    "<tr>" +
                        "<td style='padding-right: 24px;' ><p>" + requisito.NombreDocumento + "</p ></td >" +
                        "<td >" + requisito.Descripcion + "</ td >" +
                    "</tr >"
                );

            return cuerpoTabla;
        }
    }
}