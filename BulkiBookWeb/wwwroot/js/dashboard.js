
$(document).ready(function () {

    let dashboardChart = null;
    let currentMode = 'revenue';

    const canvas = document.getElementById('MonthlyRevenue');

    if (!canvas) {
        console.error('MonthlyRevenue canvas was not found.');
        return;
    }

    // Get dashboard data from controller
    $.getJSON('/Admin/Dashboard/GetChartData')
        .done(function (data) {

            // Check API response
            if (!data || !data.monthlyRevenue ||
                !data.monthlyOrders ||
                !data.orderStatusBreakdown ||
                !data.productsPerCategory) {

                console.error('Invalid dashboard data:', data);
                return;
            }

            // Shared chart options
            const commonOptions = {
                responsive: true,
                maintainAspectRatio: false,

                plugins: {
                    legend: {
                        display: false
                    },

                    tooltip: {
                        backgroundColor: '#191414',
                        titleColor: '#fff',
                        bodyColor: '#d1d5db',
                        padding: 12,
                        cornerRadius: 8,
                        displayColors: false
                    }
                },

                scales: {
                    x: {
                        grid: {
                            display: false
                        },
                        border: {
                            display: false
                        },
                        ticks: {
                            color: '#9ca3af'
                        }
                    },

                    y: {
                        beginAtZero: true,
                        border: {
                            display: false
                        },
                        grid: {
                            color: 'rgba(156, 163, 175, 0.12)'
                        },
                        ticks: {
                            color: '#9ca3af',
                            precision: 0
                        }
                    }
                }
            };


            // Create chart only once
            dashboardChart = new Chart(canvas, {
                type: 'line',

                data: {
                    labels: [],
                    datasets: [{
                        label: '',
                        data: [],
                        borderColor: '#1DB954',
                        backgroundColor: 'rgba(29, 185, 84, 0.15)',
                        fill: true,
                        tension: 0.45,
                        borderWidth: 3
                    }]
                },

                options: commonOptions
            });


            // Update chart function
            function updateDashboardChart(mode) {

                currentMode = mode;

                let labels = [];
                let values = [];

                let title = '';
                let summaryLabel = '';
                let summaryValue = '';
                let datasetLabel = '';

                let chartType = 'bar';
                let datasetStyle = {};


                // --------------------------------
                // 1. REVENUE - Smooth Line Chart
                // --------------------------------

                if (mode === 'revenue') {

                    labels = data.monthlyRevenue.map(x => x.label);
                    values = data.monthlyRevenue.map(x => x.revenue);

                    title = 'Revenue Overview';
                    summaryLabel = 'Total Revenue';

                    summaryValue = '$' +
                        values.reduce((a, b) => a + b, 0).toLocaleString();

                    datasetLabel = 'Revenue';
                    chartType = 'line';

                    datasetStyle = {
                        borderColor: '#1DB954',
                        backgroundColor: 'rgba(29, 185, 84, 0.15)',
                        borderWidth: 3,
                        fill: true,
                        tension: 0.45,

                        pointRadius: 4,
                        pointHoverRadius: 7,
                        pointBackgroundColor: '#1DB954',
                        pointBorderColor: '#fff',
                        pointBorderWidth: 2
                    };
                }


                // --------------------------------
                // 2. ORDERS - Vertical Bar Chart
                // --------------------------------

                else if (mode === 'orders') {

                    labels = data.monthlyOrders.map(x => x.label);
                    values = data.monthlyOrders.map(x => x.count);

                    title = 'Orders Overview';
                    summaryLabel = 'Total Orders';

                    summaryValue = values
                        .reduce((a, b) => a + b, 0)
                        .toLocaleString();

                    datasetLabel = 'Orders';
                    chartType = 'bar';

                    datasetStyle = {
                        backgroundColor: [
                            'rgba(29, 185, 84, 0.35)',
                            'rgba(29, 185, 84, 0.45)',
                            'rgba(29, 185, 84, 0.55)',
                            'rgba(29, 185, 84, 0.65)',
                            'rgba(29, 185, 84, 0.75)',
                            'rgba(29, 185, 84, 0.95)'
                        ],

                        borderRadius: 8,
                        borderSkipped: false,
                        barPercentage: 0.55,
                        categoryPercentage: 0.7
                    };
                }


                // --------------------------------
                // 3. ORDER STATUS - Doughnut Chart
                // --------------------------------

                else if (mode === 'status') {

                    labels = data.orderStatusBreakdown.map(x => x.status);
                    values = data.orderStatusBreakdown.map(x => x.count);

                    title = 'Order Status Overview';
                    summaryLabel = 'Total Orders';

                    summaryValue = values
                        .reduce((a, b) => a + b, 0)
                        .toLocaleString();

                    datasetLabel = 'Orders';
                    chartType = 'doughnut';

                    datasetStyle = {
                        backgroundColor: [
                            '#1DB954',
                            '#f59e0b',
                            '#3b82f6',
                            '#8b5cf6',
                            '#ef4444',
                            '#06b6d4'
                        ],

                        borderWidth: 3,
                        borderColor: '#fff',
                        hoverOffset: 10,
                        borderRadius: 5
                    };
                }


                // --------------------------------
                // 4. PRODUCTS BY CATEGORY
                // Horizontal Bar Chart
                // --------------------------------

                else if (mode === 'categories') {

                    labels = data.productsPerCategory.map(x => x.category);
                    values = data.productsPerCategory.map(x => x.count);

                    title = 'Products By Category';
                    summaryLabel = 'Total Products';

                    summaryValue = values
                        .reduce((a, b) => a + b, 0)
                        .toLocaleString();

                    datasetLabel = 'Products';
                    chartType = 'bar';

                    datasetStyle = {
                        backgroundColor: 'rgba(29, 185, 84, 0.75)',
                        borderRadius: 7,
                        borderSkipped: false,
                        barPercentage: 0.65
                    };
                }


                // --------------------------------
                // UPDATE CHART TYPE AND DATA
                // --------------------------------

                dashboardChart.config.type = chartType;

                dashboardChart.data.labels = labels;

                dashboardChart.data.datasets = [{
                    label: datasetLabel,
                    data: values,
                    ...datasetStyle
                }];


                // --------------------------------
                // RESET CHART OPTIONS
                // Important when switching types
                // --------------------------------

                dashboardChart.options.indexAxis =
                    mode === 'categories' ? 'y' : 'x';

                dashboardChart.options.cutout =
                    mode === 'status' ? '72%' : undefined;

                // Reset scales every time
                // Doughnut charts do not use Cartesian scales
                if (mode === 'status') {

                    dashboardChart.options.scales = {};

                } else {

                    dashboardChart.options.scales = {
                        x: {
                            grid: {
                                display: mode !== 'categories'
                            },

                            border: {
                                display: false
                            },

                            ticks: {
                                color: '#9ca3af'
                            }
                        },

                        y: {
                            beginAtZero: true,

                            border: {
                                display: false
                            },

                            grid: {
                                display: mode !== 'categories',
                                color: 'rgba(156, 163, 175, 0.12)'
                            },

                            ticks: {
                                color: '#9ca3af',
                                precision: 0
                            }
                        }
                    };

                    // Revenue formatting
                    if (mode === 'revenue') {

                        dashboardChart.options.scales.y.ticks.callback =
                            function (value) {
                                return '$' + value.toLocaleString();
                            };
                    }

                    // Horizontal category chart
                    if (mode === 'categories') {

                        dashboardChart.options.scales.x.beginAtZero = true;

                        dashboardChart.options.scales.x.grid = {
                            color: 'rgba(156, 163, 175, 0.12)'
                        };

                        dashboardChart.options.scales.x.ticks = {
                            color: '#9ca3af',
                            precision: 0
                        };
                    }
                }


                // --------------------------------
                // LEGEND
                // --------------------------------

                dashboardChart.options.plugins.legend =
                    mode === 'status'
                        ? {
                            display: true,
                            position: 'bottom',

                            labels: {
                                color: '#9ca3af',
                                usePointStyle: true,
                                pointStyle: 'circle',
                                padding: 18
                            }
                        }
                        : {
                            display: false
                        };


                // --------------------------------
                // TOOLTIP FORMATTING
                // --------------------------------

                dashboardChart.options.plugins.tooltip.callbacks = {

                    label: function (context) {

                        if (mode === 'status') {
                            return context.label + ': ' +
                                context.parsed;
                        }

                        if (mode === 'revenue') {
                            return '$' +
                                context.parsed.y.toLocaleString();
                        }

                        const value = mode === 'categories'
                            ? context.parsed.x
                            : context.parsed.y;

                        return value.toLocaleString();
                    }
                };


                // --------------------------------
                // UPDATE SUMMARY
                // --------------------------------

                $('#chartTitle').text(title);
                $('#summaryLabel').text(summaryLabel);
                $('#summaryValue').text(summaryValue);


                // --------------------------------
                // ACTIVE QUICK ACTION
                // --------------------------------

                $('.quick-action').removeClass('active');

                $(`.quick-action[data-chart="${mode}"]`)
                    .addClass('active');


                // Update chart
                dashboardChart.update();
            }


            // Quick Actions click events
            $('.quick-action').on('click', function () {

                const selectedMode = $(this).data('chart');

                updateDashboardChart(selectedMode);
            });


            // Initial chart
            updateDashboardChart('revenue');

        })

        .fail(function (xhr, status, error) {

            console.error('Failed to load dashboard chart data:', error);

            console.error(xhr.responseText);

        });

});
