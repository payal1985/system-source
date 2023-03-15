import { Component, OnInit, ViewChild } from '@angular/core';
import { Workbook } from 'exceljs';
import { inventoryorderitem } from 'src/app/models/inventoryorderitem';
import { InventoryorderService } from 'src/app/services/inventoryorder.service';
import * as fs from 'file-saver';
import { DatePipe } from '@angular/common';
import { ExcelService } from 'src/app/services/excel.service';
import { PdfService } from 'src/app/services/pdf.service';
import { MatSort ,Sort} from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent implements OnInit {

  inventoryOrderItemsList: inventoryorderitem[];
  displayedColumns: string[] = ['order_id', 'email', 'project', 'instdate','destb','destf','room','image','qty','item_code','description','building','floor','mploc'];
  dataSource: MatTableDataSource<inventoryorderitem>;

  // rowCount:number=2;
  // rowname:string = "";
  //@ViewChild(MatSort) sort: MatSort;
  @ViewChild(MatSort, {static: true}) sort: MatSort;
    
  constructor(private inventoryOrderServices:InventoryorderService
              ,private excelService:ExcelService
              ,private pdfService:PdfService) { 
    this.inventoryOrderServices.getInventoryOrderItems().subscribe(data =>{
     // debugger;
      this.inventoryOrderItemsList = data;
      this.dataSource = new MatTableDataSource<inventoryorderitem>(this.inventoryOrderItemsList);
      this.dataSource.sort = this.sort;
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
