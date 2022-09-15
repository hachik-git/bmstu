п»їgoogle.charts.load('current', { 'packages': ['corechart'] });
google.charts.setOnLoadCallback(drawChart);

function drawChart() {
    //var data = google.visualization.arrayToDataTable(benchmark1);


    /*    benchmark);
    [
        ['Р’С‹Р·РѕРІС‹', 'РњР°С‚СЂРёС†Р°', 'Р РµРєСѓСЂСЃРёСЏ', 'Р РµРєСѓСЂСЃРёСЏ СЃ РєРµС€РµРј'],
        ['2004', 1000, 400, 100],
        ['2005', 1170, 460,200],
        ['2006', 660, 1120,300],
        ['2007', 1030, 540,400]
    ]);*/

    var options = {
        title: 'Performance',
        //curveType: 'function',
        legend: { position: 'bottom' }
    };

    /*var chart = new google.visualization.LineChart(document.getElementById('chart_1'));

    chart.draw(data, options);*/

    var data = google.visualization.arrayToDataTable(benchmark2);
    var chart = new google.visualization.LineChart(document.getElementById('chart_2'));

    chart.draw(data, options);
}