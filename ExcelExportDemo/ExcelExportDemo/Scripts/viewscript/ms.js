var data = [{
	"Image": "https://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=1",
	"Manufacturer": "A630 - asdfasdfasdfasfConic Tub Chair",
	"Description": "4-6 weeks",
	"Price": "$1185.60",
	"Quantity": "1"
},
{
	"Image": "https://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2",
	"Manufacturer": "A631 - asdfasdfasdfasfConic Tub Chair",
	"Description": "4-6 weeks",
	"Price": "$1185.60",
	"Quantity": "1"
},
{
	"Image": "https://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=3",
	"Manufacturer": "A632 - asdfasdfasdfasfConic Tub Chair",
	"Description": "4-6 weeks",
	"Price": "$1185.60",
	"Quantity": "1",
}];

$(document).ready(function () {
	//var st = sessionStorage.getItem("sessionTimestamp");
	var ct = Date.now();
	

	//if (st && st != "") {
	//	var diff = (ct - st) / 1000;
	//	if (diff > 60 * 30) {
	//		sessionStorage.clear();
	//	}
	//}
	//sessionStorage.setItem("sessionTimestamp", ct);

	//updateArr();
});

$(function () {

	$('#cartButton').on('click', function (e) {		
			showCart();
	});

});

function showDialog(options) {
	options = $.extend({
		id: 'orrsDiag',
		title: null,
		text: null,
		negative: false,
		positive: false,
		generate: false,
		cancelable: true,
		contentStyle: null,
		onLoaded: false
	}, options);
	$('.dialog-container').remove();
	$(document).unbind("keyup.dialog");

	$('<div id="' + options.id + '" class="dialog-container"><div class="mdl-card mdl-shadow--16dp"></div></div>').appendTo("body");
	var dialog = $('#orrsDiag');
	var content = dialog.find('.mdl-card');
	if (options.contentStyle != null) content.css(options.contentStyle);
	if (options.title != null) {
		$('<h5>' + options.title + '</h5>').appendTo(content);
	}
	if (options.text != null) {
		$('<p>' + options.text + '</p>').appendTo(content);
	}
	if (options.negative || options.positive || options.generate) {
		var buttonBar = $('<div class="mdl-card__actions dialog-button-bar"></div>');
		if (options.negative) {
			options.negative = $.extend({
				id: 'negative',
				title: 'Cancel',
				onClick: function () {
					return false;
				}
			}, options.negative);
			var negButton = $('<button class="mdl-button mdl-js-button mdl-js-ripple-effect" id="' + options.negative.id + '">' + options.negative.title + '</button>');
			negButton.click(function (e) {
				e.preventDefault();
				if (!options.negative.onClick(e))
					hideDialog(dialog)
			});
			negButton.appendTo(buttonBar);
		}
		if (options.positive) {
			options.positive = $.extend({
				id: 'positive',
				title: 'OK',
				onClick: function () {
					return false;
				}
			}, options.positive);
			var posButton = $('<button class="mdl-button mdl-button mdl-js-button mdl-js-ripple-effect" id="' + options.positive.id + '">' + options.positive.title + '</button>');
			posButton.click(function (e) {
				e.preventDefault();
				if (!options.positive.onClick(e))
					hideDialog(dialog)
			});
			posButton.appendTo(buttonBar);
		}
		if (options.generate) {
			options.positive = $.extend({
				id: 'generate',
				title: 'Generate XLS',
				onClick: function () {
					return false;
				}
			}, options.positive);
			var posButton = $('<button class="mdl-button mdl-button mdl-js-button mdl-js-ripple-effect" id="' + options.generate.id + '">' + options.generate.title + '</button>');
			posButton.click(function (e) {
				e.preventDefault();
				if (!options.generate.onClick(e))
					hideDialog(dialog)
			});
			posButton.appendTo(buttonBar);
		}
		buttonBar.appendTo(content);
	}
	//componentHandler.upgradeDom();
	if (options.cancelable) {
		dialog.click(function () {
			hideDialog(dialog);
		});
		$(document).bind("keyup.dialog", function (e) {
			if (e.which == 27)
				hideDialog(dialog);
		});
		content.click(function (e) {
			e.stopPropagation();
		});
	}
	setTimeout(function () {
		dialog.css({ opacity: 1 });
		if (options.onLoaded)
			options.onLoaded();
	}, 1);
}

function showCart() {
	showDialog({
		text: buildCartContent(),
		negative: {
			title: 'Close'
		},
		positive: {
			title: 'Create',
			onClick: function (e) {
				alert("coming soon");
			}
		},
		generate: {
			title: 'Generate XLS',
			onClick: function (e) {
				generateXls();
				return true;
			}
		}
	});
}

  
    //$("#btnExport").click(function () {
    //    var todaysDate = new Date().getTime();
    //    var blobURL = tableToExcel('account_table', 'test_table');
    //    $(this).attr('download', todaysDate + '.xls')
    //    $(this).attr('href', blobURL);
    //});

function generateXls() {
	var dataa = [];
	var obj = { 'data': dataa };
	for (var i = 0; i < data.length; i++) {
		var key = data[i];
		//if (key == "sessionTimestamp") {
		//	continue;
		//}
		//var sobj = JSON.parse(sessionStorage.getItem(key));
		var robj = {
			"pdesc": key.Description,
			"leadtime": key.Manufacturer,
			"price": key.Price.replace("$", ""),
			"qty": key.Quantity,
			"img": key.Image
		};
		dataa.push(robj);
	}


	var tableBody = '<table id="excelTable"><tr style="font-weight:bold;background:#16A1E7;"><td>Bookid</td><td>Book Name</td><td>Category</td><td>Price</td><td style="height:200px; width:200px; padding:15px;">Image</td></tr>';

	dataa.forEach(function (d) {
		tableBody += '<tr><td>' + d.pdesc + '</td><td>' + d.leadtime + '</td><td>' + d.price + '</td><td>' + d.qty + '</td><td><img src=' + d.img + ' width="80px;" height="80px;"/></td></tr>';
	});
	tableBody += '</table>';

	var divContainer = document.getElementById("dvTable");
	divContainer.innerHTML = tableBody;

	var todaysDate = new Date().getTime();
	var blobURL = tableToExcel(document.getElementById("dvTable"), 'test_table');
	$(this).attr('download', todaysDate + '.xls')
	$(this).attr('href', blobURL);


	//var createXLSLFormatObj = [];
	//var xlsHeader = ["pdesc", "leadtime", "price", "qty", "img"];

	//var xlsRows = dataa;

	//createXLSLFormatObj.push(xlsHeader);
	//$.each(xlsRows, function (index, value) {
	//	var innerRowData = [];
	//	<table class="table">
	//		<thead>
	//			<tr>
	//				<th scope="col">pdesc</th>
	//				<th scope="col">leadtime</th>
	//				<th scope="col">price</th>
	//				<th scope="col">qty</th>
	//				<th scope="col">img</th>
	//			</tr>
	//		</thead>
	//		<tbody>
	//		</tbody>
	//	</table>
	//	$("tbody").append('<tr><td>' + value.pdesc + '</td><td>' + value.leadtime + '</td><td>' + value.price + '</td><td>' + value.qty + '</td><td><img src=' + value.img + ' width="80px;" height="80px;"/></td></tr>');
	//	$.each(value, function (ind, val) {

	//		innerRowData.push(val);
	//	});
	//	createXLSLFormatObj.push(innerRowData);
	//});


	///* File Name */
	//var filename = "FreakyJSON_To_XLS.xlsx";

	///* Sheet Name */
	//var ws_name = "FreakySheet";

	//if (typeof console !== 'undefined') console.log(new Date());
	//var wb = XLSX.utils.book_new(),
	//	ws = XLSX.utils.aoa_to_sheet(createXLSLFormatObj);

	///* Add worksheet to workbook */
	//XLSX.utils.book_append_sheet(wb, ws, ws_name);

	///* Write workbook and Download */
	//if (typeof console !== 'undefined') console.log(new Date());
	//XLSX.writeFile(wb, filename);
	//if (typeof console !== 'undefined') console.log(new Date()); 




	//var todaysDate = new Date().getTime();
	//var blobURL = tableToExcel('exTable', 'test_table');
	//$(this).attr('download', todaysDate + '.xls')
	//$(this).attr('href', blobURL);

	

	//let a = document.createElement('a')
	//let dataType = 'data:application/vnd.ms-excel'; 
	//a.href = `data:application/vnd.ms-excel, ${encodeURIComponent(exTable)}`
	////a.download = 'sudhir600_' + rand() + '.xls'
	//a.download = 'payaltest_' + new Date().getTime() + '.xls'
	//a.click()
	
}
function tableToExcel(table, name) {
	var uri = 'data:application/vnd.ms-excel;base64,'
		, template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head>< !--[if gte mso 9]> <xml><x: <x: <x: <x: {worksheet}</x: Name > <x: WorksheetOptions><x: </x: WorksheetOptions ></x: ExcelWorksheet ></x: ExcelWorksheets ></x: ExcelWorkbook ></xml >< ![endif]-- > <meta http-equiv="content-type" content="text/plain; charset=UTF-8" /></head > <body><table>{table}</table></body></html>'
		//, template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head>< !--[if gte mso 9]> <xml><x: <x: <x: <x: {worksheet}</x: Name > <x: WorksheetOptions><x: </x: WorksheetOptions ></x: ExcelWorksheet ></x: ExcelWorksheets ></x: ExcelWorkbook ></xml >< ![endif]-- > <meta http-equiv="content-type" content="text/plain; charset=UTF-8" /></head > <body><img src="{imgsrc1}" style="float:left;clear:none;margin-right:50px " height=50 width=100/><img src="{imgsrc2}" style="float:right;clear:none;margin-left:50px " height=50 width=100/><h1 style="display:inline-block">{heading}</h1><br><table>{table}</table></body></html>'
		, base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
		, format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
	return function (table, name) {
		if (!table.nodeType) table = document.getElementById('excelTable');
		heading = 'THIS IS HEAD';
		imgsrc1 = 'http://www.w3schools.com/html/pic_mountain.jpg';
		imgsrc2 = 'http://www.w3schools.com/html/pic_html5.gif';

		//var ctx = { worksheet: name || 'Worksheet', imgsrc1: imgsrc1, imgsrc2: imgsrc2, heading: heading, table: table.innerHTML }
		var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
		//var blob = new Blob([format(template, ctx)]);
		var blob = new Blob([format(template, ctx)]);
		var blobURL = window.URL.createObjectURL(blob);
		return blobURL;
	}
}


function buildCartContent() {
	
	var cstr = "<table id='exTable' border=0 cellspacing=0>";
	cstr += "<tr>";
	cstr += "<th>Image</th>";
	cstr += "<th>Manufacturer</th>";
	cstr += "<th>Description</th>";
	cstr += "<th>Price</th>";
	cstr += "<th>Quantity</th>";
	cstr += "<th></th>";
	cstr += "</tr>";
	for (var i = 0; i < data.length; i++) {
		var key = data[i];
		//if (key == "sessionTimestamp") {
		//	continue;
		//}
		//var obj = JSON.parse(data[i]);
		cstr += "<tr>";
		cstr += "<td><img width='100px' height='100px' src ='" + key.Image + "'/></td>";
		cstr += "<td>" + key.Manufacturer + "</td>";
		cstr += "<td>" + key.Description + "</td>";
		cstr += "<td>" + key.Price + "</td>";
		cstr += "<td><input type='text' size=3 value='" + key.Quantity + "'></td>";
		//cstr += "<td><i class='material-icons' onclick='removeCartItem(\"" + key + "\");'>remove_shopping_cart</i></td>";
		cstr += "</tr>";
	}
	cstr += "</table>";
	return cstr;
}