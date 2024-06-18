/*** LINE CHART START ***/
/* Variables */
const ctx = document.getElementById('myChart');

var addsomeSales = parseInt(ctx.getAttribute('addSome'));
var aheadBizSale = parseInt(ctx.getAttribute('aheadBiz'));

// Initialize chart with initial data
var myChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'], // Dynamic labels for days of the week
        datasets: [{
            label: 'Addsome Business Corporation',
            data: [addsomeSales], // Updated data points for Addsome Business Corporation
            fill: false,
            borderColor: '#00008B',
            backgroundColor: '#00008B',
            borderWidth: 1,
            tension: 0.4 // Adjust the tension for smoother curves if needed
        }, {
            label: 'Ahead Biz Computers',
            data: [aheadBizSale], // Updated data points for Ahead Biz Computers
            fill: false,
            borderColor: '#FFFF00',
            backgroundColor: '#FFFF00',
            borderWidth: 1,
            tension: 0.4 // Adjust the tension for smoother curves if needed
        }]
    },
    options: {
        scales: {
            y: {
                beginAtZero: true
            }
        }
    }
});

// Function to update chart data dynamically
function updateChartData(addsomeSales, aheadSales) {
    myChart.data.datasets[0].data[6] = addsomeSales; // Update the last data point for Addsome Business Corporation
    myChart.data.datasets[1].data[6] = aheadSales; // Update the last data point for Ahead Biz Computers
    myChart.update();
}
/*** LINE CHART END ***/