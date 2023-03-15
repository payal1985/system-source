import { Injectable } from '@angular/core';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { DatePipe } from '@angular/common';
import { inventoryorderitem } from '../model/inventoryorderitem';


@Injectable({
  providedIn: 'root'
})
export class ExcelService {

  //inventoryOrderItemsList: inventoryorderitem[]
  rowCount:number=2;
  rowname:string = "";
  
    constructor(private datePipe: DatePipe) { }

    public exportAsExcelFile(inventoryOrderItemsList:inventoryorderitem[]){
        //debugger;
        let workbook = new Workbook();
        let worksheet = workbook.addWorksheet('OrderSheet');
     
        var font = { name: 'Arial', size: 10, bold:true };
        worksheet.getCell('A1').font = font;
        worksheet.getCell('B1').font = font;
        worksheet.getCell('C1').font = font;
        worksheet.getCell('D1').font = font;
        worksheet.getCell('E1').font = font;
        worksheet.getCell('F1').font = font;
        worksheet.getCell('G1').font = font;
        worksheet.getCell('H1').font = font;
        worksheet.getCell('I1').font = font;
        worksheet.getCell('J1').font = font;
        worksheet.getCell('K1').font = font;
        worksheet.getCell('L1').font = font;
        worksheet.getCell('M1').font = font;
        worksheet.getCell('N1').font = font;
    
        worksheet.columns = [
         { header: 'Order Id', key: 'order_id', width: 20},
         { header: 'Email', key: 'email', width: 32 },
         { header: 'Project', key: 'project', width: 20  },
         { header: 'Inst Date', key: 'instdate', width: 20},
         { header: 'Dest Bldg', key: 'destb', width: 20 },
         { header: 'Dest Floor', key: 'destf', width: 20 },
         { header: 'Dest Room', key: 'room', width: 20 },
         { header: 'Image', key: 'imgBase64', width: 10 },
         { header: 'Quantity', key: 'qty', width: 10},
         { header: 'Item Code', key: 'item_code', width: 15 },
         { header: 'Description', key: 'description', width: 30 },
         { header: 'Pull Bldg', key: 'building', width: 20 },
         { header: 'Pull Floor', key: 'floor', width: 20  },
         { header: 'Pull Loc', key: 'mploc', width: 20 },
        ];
        
        inventoryOrderItemsList.forEach(e=>{
          if(e.imgBase64 != null)
          {
            const imageId = workbook.addImage({
              base64: e.imgBase64,
              extension: 'jpeg',
            });
          
            //debugger;
            e.imgBase64Num = imageId;
          }
          else
            e.imgBase64Num = -1;
        //orderitem.requested_inst_date = this.datePipe.transform(this.createOrderForm.value.req_inst_date, "yyyy-MM-dd HH:mm:ss"),
           e.instdatestr = this.datePipe.transform(e.instdate,"MM-dd-yyyy");
        });
    
      
        inventoryOrderItemsList.forEach((e,index) => {   
    
          
          this.rowname = 'H'+this.rowCount +':H'+this.rowCount;
           
          worksheet.addRow({
            order_id: e.order_id + ' ' + e.inv_id + ' ' + e.inv_item_id,
            email:e.email,
            project:e.project,
            instdate:e.instdatestr,
            destf:e.destf,
            room:e.room,
            Image:e.imgBase64Num,
            qty:e.qty,
            item_code:e.item_code,
            description:e.description,
            building:e.building,
            floor:e.floor,
            mploc:e.mploc,
            destb:e.destb
          },"n");

          if(e.imgBase64Num != -1)         
              worksheet.addImage(e.imgBase64Num, this.rowname);
    
          // worksheet.addImage(e.imgBase64Num,{
          //   tl: { col: this.rowCount, row: this.rowCount },
          //   ext: { width: 500, height: 200 }
          // });
    
        
        
           this.rowCount++;
          });
          this.rowCount=2;
    
          let rowIndex = 1;
          for (rowIndex; rowIndex <= worksheet.rowCount; rowIndex++) {
            worksheet.getRow(rowIndex).alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };
            worksheet.getRow(rowIndex).height = 70;
    
            // const row = worksheet.getRow(rowIndex);
            // var rowname = 'D'+rowIndex;
            // debugger;
            // row.getCell(4).numFmt = 'm/d/yyyy';
            // row.getCell(3).numFmt = 'm/d/yyyy';
        
            // row.commit();
            //worksheet.getCell('D'+rowIndex).numFmt = 'mm/dd/yyyy';
          }
          
          workbook.xlsx.writeBuffer().then((data) => {
            let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            fs.saveAs(blob, 'SSI Order Admin.xlsx');
          });
    }
}