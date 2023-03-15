import { Component, OnInit } from '@angular/core';
// import { CdkVirtualScrollViewport } from '@angular/cdk/scrolling'
import * as FileSaver from 'file-saver';


@Component({
  selector: 'app-excelexport',
  templateUrl: './excelexport.component.html',
  styleUrls: ['./excelexport.component.scss']
})
export class ExcelexportComponent implements OnInit {
  sales = [];
  constructor() { }

  ngOnInit(): void {

    this.sales = [
      { brand: 'Apple', lastYearSale: '51%', thisYearSale: '40%', lastYearProfit: '$54,406.00', thisYearProfit: '$43,342',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'Samsung', lastYearSale: '83%', thisYearSale: '96%', lastYearProfit: '$423,132', thisYearProfit: '$312,122',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'Microsoft', lastYearSale: '38%', thisYearSale: '5%', lastYearProfit: '$12,321', thisYearProfit: '$8,500',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'Philips', lastYearSale: '49%', thisYearSale: '22%', lastYearProfit: '$745,232', thisYearProfit: '$650,323,',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'Song', lastYearSale: '17%', thisYearSale: '79%', lastYearProfit: '$643,242', thisYearProfit: '500,332',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'LG', lastYearSale: '52%', thisYearSale: ' 65%', lastYearProfit: '$421,132', thisYearProfit: '$150,005',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'Sharp', lastYearSale: '82%', thisYearSale: '12%', lastYearProfit: '$131,211', thisYearProfit: '$100,214',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'Panasonic', lastYearSale: '44%', thisYearSale: '45%', lastYearProfit: '$66,442', thisYearProfit: '$53,322',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'HTC', lastYearSale: '90%', thisYearSale: '56%', lastYearProfit: '$765,442', thisYearProfit: '$296,232',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' },
      { brand: 'Toshiba', lastYearSale: '75%', thisYearSale: '54%', lastYearProfit: '$21,212', thisYearProfit: '$12,533',imageurl:'	http://ssidb-test.systemsource.com/Project/GoCanvasImages/SCWatson139.jpg' }
  ];
  }

  exportExcel() {
    import("xlsx").then(xlsx => {
        const worksheet = xlsx.utils.json_to_sheet(this.sales); // Sale Data
        const workbook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };
        const excelBuffer: any = xlsx.write(workbook, { bookType: 'xlsx', type: 'array' });
        this.saveAsExcelFile(excelBuffer, "sales");
    });
  }
  saveAsExcelFile(buffer: any, fileName: string): void {
    import("file-saver").then(FileSaver => {
      let EXCEL_TYPE =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8";
      let EXCEL_EXTENSION = ".xlsx";
      const data: Blob = new Blob([buffer], {
        type: EXCEL_TYPE
      });
      FileSaver.saveAs(
        data,
        fileName + "_export_" + new Date().getTime() + EXCEL_EXTENSION
      );
    });
  }

}
