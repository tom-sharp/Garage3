function doughnutgraph(car, mc, truck, bus, boat) {

    var ctxD = document.getElementById("doughnutChart").getContext('2d');
    var myLineChart = new Chart(ctxD, {
        type: 'doughnut',
        data: {
            labels: ["Cars (" + car + ")", "Motorcycles (" + mc + ")", "Trucks (" + truck + ")", "Buses (" + bus + ")", "Boats (" + boat + ")"],
            datasets: [{
                data: [car, mc, truck, bus, boat],
                backgroundColor: ["#F7464A", "#46BFBD", "#FDB45C", "#949FB1", "#4D5360"],
                hoverBackgroundColor: ["#FF5A5E", "#5AD3D1", "#FFC870", "#A8B3C5", "#616774"]
            }]
        },
        options: {
            responsive: true
        }
    });
}