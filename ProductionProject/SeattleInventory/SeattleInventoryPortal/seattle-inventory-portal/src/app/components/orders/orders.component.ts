import { Component, OnInit } from '@angular/core';
import { Workbook } from 'exceljs';
import { inventoryorderitem } from 'src/app/model/inventoryorderitem';
import { InventoryorderService } from 'src/app/services/inventoryorder.service';
import * as fs from 'file-saver';
import { DatePipe } from '@angular/common';
import { ExcelService } from 'src/app/services/excel.service';
import { PdfService } from 'src/app/services/pdf.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  inventoryOrderItemsList: inventoryorderitem[]
  displayedColumns: string[] = ['order_id', 'email', 'project', 'instdate','destb','destf','room','image','qty','item_code','description','building','floor','mploc'];
  // rowCount:number=2;
  // rowname:string = "";
 
    
  constructor(private inventoryOrderServices:InventoryorderService
              ,private excelService:ExcelService
              ,private pdfService:PdfService) { 
    //debugger;
    this.inventoryOrderServices.getInventoryOrderItems().subscribe(data =>{
      this.inventoryOrderItemsList = data;
    });
  }

  ngOnInit(): void {
  }

  exportExcel(){    
      this.excelService.exportAsExcelFile(this.inventoryOrderItemsList);
    }

  exportPdf(tableId:string){
      this.pdfService.exportAsPdfFile(tableId);
    }
}
