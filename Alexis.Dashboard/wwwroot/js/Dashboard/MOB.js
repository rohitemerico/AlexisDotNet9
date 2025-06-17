const summaryTitle = "All Transactions";

var isLineChart = false; //alter this to choose DEFAULT - bar or line chart
var arr_Charts = new Array();
var CellNum = 0; //range [-1,0]
var Row = null;
var Cell = null;

//Windows Onload--------------------
window.onload = function () {
	//Create toggle icon based on default chart----------
	var icon = document.getElementById('togglerIcon');
	if (isLineChart)
		icon.classList.add('fa-bar-chart');
	else
		icon.classList.add('fa-line-chart');

	jsonData = jsonData.reverse(); //reverse array because in the upcoming method, every new row will be inserted to the first row

	//create main chart-------------
	var parent_fieldset = document.getElementById('mainFieldset');
	var maincanvas = CreateCanvas('canvas_main');
	maincanvas.style.width = '100%';
	maincanvas.height = 250;
	parent_fieldset.appendChild(maincanvas);

	var arr_labels = new Array();
	var arr_dataTotal = new Array();
	var arr_dataPass = new Array();
	var arr_dataFail = new Array();

	//extract relevant data from raw json
	for (var j = 0; j < summaryData.Device.length; j++) {
		arr_labels[j] = summaryData.Device[j].MachineName;
		arr_dataTotal[j] = summaryData.Device[j].TotalTrans;
		arr_dataPass[j] = summaryData.Device[j].TotalPassed;
		arr_dataFail[j] = summaryData.Device[j].TotalFailed;
	}

	//generate chart for the newly added canvas
	GenerateCanvasChart(isLineChart, "canvas_main", summaryTitle, arr_labels, arr_dataTotal, arr_dataPass, arr_dataFail);

	//create sub charts-------------
	for (var i = 0; i < jsonData.length; i++) {
		//first, generate a row with empty canvas
		GenerateRowContent("canvas_" + i);

		var arr_labels = new Array();
		var arr_dataTotal = new Array();
		var arr_dataPass = new Array();
		var arr_dataFail = new Array();

		//extract relevant data from raw json
		for (var j = 0; j < jsonData[i].Device.length; j++) {
			arr_labels[j] = jsonData[i].Device[j].MachineName;
			arr_dataTotal[j] = jsonData[i].Device[j].TotalTrans;
			arr_dataPass[j] = jsonData[i].Device[j].TotalPassed;
			arr_dataFail[j] = jsonData[i].Device[j].TotalFailed;
		}

		//generate chart for the newly added canvas
		GenerateCanvasChart(isLineChart, "canvas_" + i, jsonData[i].TransDesc, arr_labels, arr_dataTotal, arr_dataPass, arr_dataFail);
	}
}

function GenerateRowContent(canvasId) {
	const table = document.getElementById("mainTable");

	// Every two canvases go into a single new row
	if (CellNum % 2 === 0) {
		// Insert new row at the top
		var newRow = table.insertRow(0);
		var cell = newRow.insertCell(0);
		cell.appendChild(CreateCanvas(canvasId));

		// Store row for next canvas
		GenerateRowContent.currentRow = newRow;
	} else {
		// Add canvas to the second cell of the existing top row
		var cell = GenerateRowContent.currentRow.insertCell(1);
		cell.appendChild(CreateCanvas(canvasId));
	}

	CellNum++;
}



//Insert new first row first col with canvas element
//function GenerateRowContent(canvasId) {
//	var table = document.getElementById("mainTable");

//	//control cell 1 or cell 2
//	if (CellNum == 0) {
//		Row = table.insertRow(0);
//		CellNum--;
//	} else {
//		CellNum++;
//	}
//	Cell = Row.insertCell(CellNum);
//	Cell.appendChild(CreateCanvas(canvasId));
//}

//Create canvas element
function CreateCanvas(canvasId) {
	var canvas = document.createElement("canvas");
	canvas.setAttribute("id", canvasId);
	canvas.style.width = '100%';
	canvas.height = 200;
	return canvas;
}

//Insert chart to canvas
function GenerateCanvasChart(isLineChart, canvasId, title, arr_labels, arr_dataTotal, arr_dataPass, arr_dataFail) {
	//configure
	const data = {
		labels: arr_labels,
		datasets: [{
			label: 'Total Transaction',
			borderColor: '#E83C4F',
			backgroundColor: '#E83C4F',
			fill: false,
			data: arr_dataTotal
		},
		{
			label: 'Success Transaction',
			borderColor: '#ff8c00',
			backgroundColor: '#ff8c00',
			fill: false,
			data: arr_dataPass
		},
		{
			label: 'Fail Transaction',
			borderColor: '#AAAAAA',
			backgroundColor: '#AAAAAA',
			fill: false,
			data: arr_dataFail
		},
		]
	};

	const config_lineChart = {
		type: 'line',
		data: null,
		options: {
			scales: {
				x: {
					title: {
						display: true,
						text: 'Devices',
						family: 'ITC Avant Garde Gothic Std Book'
					}
				},
				y: {
					beginAtZero: true,
					title: {
						display: true,
						text: 'Transaction Count',
						family: 'ITC Avant Garde Gothic Std Book'
					}
				}
			},
			responsive: false,
			plugins: {
				title: {
					display: true,
					align: 'center',
					text: null,
					font: {
						size: 20,
						family: 'ITC Avant Garde Gothic Std Book'
					}
				},
				legend: {
					display: true,
					align: 'center',
					position: 'top',
					labels: {
						padding: 10,
						boxWidth: 15,
						font: {
							size: 12,
							family: 'ITC Avant Garde Gothic Std Book'
						}
					}
				}
			}
		}
	};
	const config_barChart = {
		type: 'bar',
		data: null,
		options: {
			indexAxis: 'y',
			responsive: false,
			plugins: {
				title: {
					display: true,
					align: 'center',
					text: null,
					font: {
						size: 20,
						family: 'ITC Avant Garde Gothic Std Book'
					}
				},
				legend: {
					display: true,
					align: 'center',
					position: 'top',
					labels: {
						padding: 10,
						boxWidth: 15,
						font: {
							size: 12,
							family: 'ITC Avant Garde Gothic Std Book'
						}
					}
				}
			},
			scales: {
				x: {
					title: {
						display: true,
						text: 'Transaction Count',
						family: 'ITC Avant Garde Gothic Std Book'
					},
					grid: {
						offset: false
					},
					stacked: true
				},
				y: {
					title: {
						display: true,
						text: 'Devices',
						family: 'ITC Avant Garde Gothic Std Book'
					},
					stacked: true
				}
			}
		}
	};

	config_lineChart.data = data;
	config_lineChart.options.plugins.title.text = title;
	config_barChart.data = data;
	config_barChart.options.plugins.title.text = title;

	//draw the graph
	var canvasChart = document.getElementById(canvasId);

	if (isLineChart)
		arr_Charts.push(new Chart(canvasChart, config_lineChart));
	else
		arr_Charts.push(new Chart(canvasChart, config_barChart));
}

//Toggle between bar chart and line chart
function fn_ToggleChart() {
	// toggle the type attribute
	var icon = document.getElementById('togglerIcon');

	if (!isLineChart) {
		//currently show bar chart
		icon.classList.remove('fa-line-chart');
		icon.classList.add('fa-bar-chart');

		//switch to line chart--------------

		//destroy old charts but retain the data
		var arr_dataToKeep = new Array();

		for (var i = 0; i < arr_Charts.length; i++) {
			var dataToKeep = arr_Charts[i].data;
			arr_dataToKeep.push(dataToKeep)
			arr_Charts[i].destroy();
		}
		arr_Charts = new Array();

		//recreate new charts
		//main chart
		const newConfig = {
			type: 'line',
			data: null,
			options: {
				scales: {
					x: {
						title: {
							display: true,
							text: 'Devices',
							family: 'ITC Avant Garde Gothic Std Book'
						}
					},
					y: {
						beginAtZero: true,
						title: {
							display: true,
							text: 'Transaction Count',
							family: 'ITC Avant Garde Gothic Std Book'
						}
					}
				},
				responsive: false,
				plugins: {
					title: {
						display: true,
						align: 'center',
						text: null,
						font: {
							size: 20,
							family: 'ITC Avant Garde Gothic Std Book'
						}
					},
					legend: {
						display: true,
						align: 'center',
						position: 'top',
						labels: {
							padding: 10,
							boxWidth: 15,
							font: {
								size: 12,
								family: 'ITC Avant Garde Gothic Std Book'
							}
						}
					}
				}
			}
		};

		newConfig.data = arr_dataToKeep[0];
		newConfig.options.plugins.title.text = summaryTitle;

		arr_Charts.push(new Chart(document.getElementById('canvas_main'), newConfig));

		//sub charts
		for (var i = 0; i < jsonData.length; i++) {
			const newConfig = {
				type: 'line',
				data: null,
				options: {
					scales: {
						x: {
							title: {
								display: true,
								text: 'Devices',
								family: 'ITC Avant Garde Gothic Std Book'
							}
						},
						y: {
							beginAtZero: true,
							title: {
								display: true,
								text: 'Transaction Count',
								family: 'ITC Avant Garde Gothic Std Book'
							}
						}
					},
					responsive: false,
					plugins: {
						title: {
							display: true,
							align: 'center',
							text: null,
							font: {
								size: 20,
								family: 'ITC Avant Garde Gothic Std Book'
							}
						},
						legend: {
							display: true,
							align: 'center',
							position: 'top',
							labels: {
								padding: 10,
								boxWidth: 15,
								font: {
									size: 12,
									family: 'ITC Avant Garde Gothic Std Book'
								}
							}
						}
					}
				}
			};

			newConfig.data = arr_dataToKeep[i + 1];
			newConfig.options.plugins.title.text = jsonData[i].TransDesc;

			arr_Charts.push(new Chart(document.getElementById('canvas_' + i), newConfig));
		}


		isLineChart = true;

	} else {
		//currently show line chart
		icon.classList.remove('fa-bar-chart');
		icon.classList.add('fa-line-chart');

		//switch to bar chart--------------
		//destroy old charts but retain the data
		var arr_dataToKeep = new Array();

		for (var i = 0; i < arr_Charts.length; i++) {
			var dataToKeep = arr_Charts[i].data;
			arr_dataToKeep.push(dataToKeep)
			arr_Charts[i].destroy();
		}
		arr_Charts = new Array();

		//recreate new charts
		//main chart
		const newConfig = {
			type: 'bar',
			data: null,
			options: {
				indexAxis: 'y',
				responsive: false,
				plugins: {
					title: {
						display: true,
						align: 'center',
						text: null,
						font: {
							size: 20,
							family: 'ITC Avant Garde Gothic Std Book'
						}
					},
					legend: {
						display: true,
						align: 'center',
						position: 'top',
						labels: {
							padding: 10,
							boxWidth: 15,
							font: {
								size: 12,
								family: 'ITC Avant Garde Gothic Std Book'
							}
						}
					}
				},
				scales: {
					x: {
						title: {
							display: true,
							text: 'Transaction Count',
							family: 'ITC Avant Garde Gothic Std Book'
						},
						grid: {
							offset: false
						},
						stacked: true
					},
					y: {
						title: {
							display: true,
							text: 'Devices',
							family: 'ITC Avant Garde Gothic Std Book'
						},
						stacked: true
					}
				}
			}
		};

		newConfig.data = arr_dataToKeep[0];
		newConfig.options.plugins.title.text = summaryTitle;

		arr_Charts.push(new Chart(document.getElementById('canvas_main'), newConfig));

		//sub charts
		for (var i = 0; i < jsonData.length; i++) {
			const newConfig = {
				type: 'bar',
				data: null,
				options: {
					indexAxis: 'y',
					responsive: false,
					plugins: {
						title: {
							display: true,
							align: 'center',
							text: null,
							font: {
								size: 20,
								family: 'ITC Avant Garde Gothic Std Book'
							}
						},
						legend: {
							display: true,
							align: 'center',
							position: 'top',
							labels: {
								padding: 10,
								boxWidth: 15,
								font: {
									size: 12,
									family: 'ITC Avant Garde Gothic Std Book'
								}
							}
						}
					},
					scales: {
						x: {
							title: {
								display: true,
								text: 'Transaction Count',
								family: 'ITC Avant Garde Gothic Std Book'
							},
							grid: {
								offset: false
							},
							stacked: true
						},
						y: {
							title: {
								display: true,
								text: 'Devices',
								family: 'ITC Avant Garde Gothic Std Book'
							},
							stacked: true
						}
					}
				}
			};

			newConfig.data = arr_dataToKeep[i + 1];
			newConfig.options.plugins.title.text = jsonData[i].TransDesc;

			arr_Charts.push(new Chart(document.getElementById('canvas_' + i), newConfig));
		}

		isLineChart = false;
	}
}