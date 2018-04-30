function RecolectaLlaves(element) {
    var cadenaIndicadores = "";
    var indices = element.Valores.length;
    for (var k = 0; k < indices; k++) {
        var elemento = element.Valores[k];
        cadenaIndicadores +=
            "<td style='width:20px;'><div style='width:15px;height:15px;border-radius:15px;background-color:" +
            elemento.Item3 +
            ";'></div></td>" +
            "<td style='width:2px;'></td>" +
            "<td style='width:100px;'>" +
            elemento.Item1 +
            "</td>";
    }
    $("#RowIndicadores").html(cadenaIndicadores);
}

function jsReporte2() {

    var arreglo = new Array();
    var indices = 0;
    for (var i = 0; i < G_Objetos.length; i++) {
        var mensaje = G_Objetos[i].Descripcion;
        var auxiliar = G_Objetos[i];
        indices = auxiliar.Valores.length;
        for (var j = 0; j < indices; j++) {
            if (i === 0 && j === 0) {
                element = auxiliar;
                RecolectaLlaves(element);
            }
            var elemento = new Object();
            elemento["Delegacion"] = mensaje;
            elemento["Total"] = auxiliar.Valores[j].Item2;
            elemento["EstadoCita"] = auxiliar.Valores[j].Item1;
            elemento["Color"] = auxiliar.Valores[j].Item3;
            arreglo.push(elemento);
        }
    }
    var indiceMedio = Math.floor(indices / 2);
    renderChart(arreglo, indices, indiceMedio);
}

function renderChart(arreglo, indices, indiceMedio) {

    var data = arreglo;

    var widthBar = $("#divBarras").css("width");
    widthBar = widthBar.replace("px", "");
    widthBar = parseInt(widthBar);

    var valueLabelWidth = 10; // espacio reservado para etiquetas (derecha)
    var barHeight = (450) / 32; // altura de una barra
    var barLabelWidth = 300; ///(widthBar * 0.1); // espacio reservado para etiquetas de la barra
    var barLabelPadding = 2; //  espacio entre las barras y las etiquetas de las barras (izquierda)
    var gridLabelHeight = 18; // espacio reservado para mostrar los rangos
    var gridChartOffset = 33; // espacio entre el inicio del grid y la primera barra

    // accessor functions
    var barLabel = function(d) { return d["Delegacion"] };
    var barValue = function(d) { return parseInt(d["Total"]); };

    // sorting
    /*var sortedData = data.sort(function (a, b) {
        return d3.descending(barValue(a), barValue(b));
    });*/

    var sortedData = data;

    // scales
    var yScale = d3.scale.ordinal().domain(d3.range(0, sortedData.length))
        .rangeBands([0, sortedData.length * barHeight]);
    var ejeY = function(d, i) { return yScale(i); };
    var yText = function(d, i) { return ejeY(d, i) + yScale.rangeBand() / 2; };
    var ejeX = d3.scale.linear().domain([0, d3.max(sortedData, barValue)]).range([0, (widthBar - 400)]);

    // svg container element
    var chart = d3.select("#divBarras").append("svg")
        .attr("width", widthBar + barLabelWidth + valueLabelWidth)
        .attr("height", ((arreglo.length + 10) * barHeight));

    //grid line labels
    var gridContainer = chart.append("g")
        .attr("transform", "translate(" + barLabelWidth + "," + gridLabelHeight + ")");

    gridContainer.selectAll("text").data(ejeX.ticks(10)).enter().append("text")
        .attr("x", ejeX).attr("dy", 30).text(function(d) { return d; }).attr("class", "LabelsSuperior")
        .style("font-size", "10px");

    // vertical grid lines
    gridContainer.selectAll("line").data(ejeX.ticks(10)).enter().append("line")
        .attr("x1", ejeX).attr("x2", ejeX).attr("y1", 40)
        .attr("y2", yScale.rangeExtent()[1] + gridChartOffset).attr("class", "Linea")
        .style("stroke", "#ccc");

    // bar labels
    var labelsContainer = chart.append("g").attr("transform", "translate(0 , 0 )");

    labelsContainer.selectAll("text").data(sortedData).enter().append("text")
        .attr("y", 0)
        .attr("dy", ".35em") // vertical-align: middle
        .attr("text-anchor", "end")
        .text(barLabel)
        .style("opacity", function(d, i) { return ((i % indices) === indiceMedio) ? 1 : 0; })
        .transition()
        //.delay(function(d, i) { return i * 50; })
        //.duration(500)
        .attr("y", yText)
        .attr("class", "LabelsIzquierda")
        .style("font-size", "10px")
        .attr("transform",
            "translate(" + (barLabelWidth - barLabelPadding) + "," + (gridLabelHeight + gridChartOffset) + ")");

    // bars
    var barsContainer = chart.append("g")
        .attr("transform", "translate(" + barLabelWidth + "," + (gridLabelHeight + gridChartOffset) + ")");

    //Categoria
    /*barsContainer.append("text")
        .attr("x", 200)
        .attr("y", -20)
        .text("Reporte de citas por UADyCS")
        .attr("id", "TituloCentral")
        .style("font-size","32px")
        .style("text-align", "center")
        .attr('height', "100px")
        .attr('width',"100%")*/

    barsContainer.selectAll("rect").data(sortedData).enter().append("rect")
        .attr("y", ejeY)
        .attr("height", yScale.rangeBand())
        .attr("width", 0)
        .attr("id", function(d, i) { return "B_" + i; })
        .on("mouseover",
            function(d) {
                d3.select(this).style("fill", "maroon");
                d3.select(this).attr("cursor", "pointer");
                tooltip.show("Unidad: " +
                    d["Delegacion"] +
                    "</br>" +
                    "Estado: " +
                    d["EstadoCita"] +
                    "</br>" +
                    "Cantidad: " +
                    d["Total"]);
            })
        .on("mouseout",
            function() {
                tooltip.hide();
                d3.select(this).style("fill", d3.select(this).attr("color_Fill"));
            })
        .transition()
        //.duration(1000)
        //.delay(function(d, i) { return i * 50; })
        .attr("y", ejeY)
        .attr("height", yScale.rangeBand())
        .attr("width", function(d) { return ejeX(barValue(d)); })
        .style("fill", function(d) { return d.Color; })
        .attr("color_Fill", function(d) { return d.Color; })
        .attr("rx", "5px")
        .attr("ry", "5px")
        .attr("class", "Barras")
        .style("opacity", ".7")
        .style("stroke", "black");

    // bar value labels
    barsContainer.selectAll("text").data(sortedData).enter().append("text")
        .text(function(d) { return barValue(d).toFixed(0); })
        .style("font-size", "0px")
        .transition()
        //.delay(function(d, i) { return i * 50; })
        //.duration(1000)
        .attr("x", function(d) { return ejeX(barValue(d)); })
        .attr("y", yText)
        .attr("dx", 3) // padding-left
        .attr("dy", ".35em")
        .style("font-size", "10px");

    // start line
    barsContainer.append("line")
        .attr("y1", -gridChartOffset + 30)
        .attr("y2", yScale.rangeExtent()[1] + gridChartOffset - 30)
        .attr("id", "LineaPrincipal")
        .style("stroke", "#000");

    $("#SeccionReporte").css("margin-left", "-130px");
    $("#divBarras").css("height", ((arreglo.length + 11) * barHeight));
}