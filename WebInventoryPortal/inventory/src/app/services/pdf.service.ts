import { Injectable } from '@angular/core';
import  jspdf from 'jspdf';
import 'jspdf-autotable';
import html2canvas from 'html2canvas';

@Injectable({
  providedIn: 'root'
})
export class PdfService {

    constructor() { }

    exportAsPdfFile(tableId:string){
        //debugger;
        var doc = new jspdf('p', 'mm', "a0");

        (doc as any).autoTable({ 
            html: '#'+tableId,
            bodyStyles: { minCellHeight: 20, fontSize:14 },
            columnStyles: { Image: { halign: 'center' } },
            theme: 'grid',
            styles: { valign: 'middle', overflow: 'linebreak', halign: 'center', minCellHeight: 21 },
            headStyles: { fillColor: '#37474f', textColor: 'white', fontStyle: 'bold', lineWidth: 0.5, lineColor: '#ccc',minCellHeight:20,textAlign:'center',fontSize:20 },
            margin: { top: 50, left: 20, right: 20, bottom: 0 },
            didDrawCell: (data) => {
               // debugger;
                //console.log(data.column.index);
                if (data.column.index === 7 && data.cell.section === 'body') {
                   // debugger;
                  const td = data.cell.raw;
                  const img = td.getElementsByTagName('img')[0];
                  //console.log(img);
                  if(img.currentSrc != "")
                  {
                    //debugger;
                   let dim = data.cell.height - data.cell.padding('vertical');
                  // let textPos = data.cell.textPos;
                  doc.addImage(img.src, data.cell.x + 3, data.cell.y + 1, 19, 19);
                  //doc.addImage(img.src, data.cell.x + 1, data.cell.y + 1, dim, dim);
                  }
                }
              }
        });
        doc.setFontSize(30);
        doc.text('SSI Inventory Orders as of '+ new Date().toLocaleString(),270, 20);
        var filename = 'SSI Inventory Orders as of '+ new Date().toLocaleString() + '.pdf';

        doc.save(filename);


        /*currently working code*/
    //     let DATA = document.getElementById(tableId);
        
    // debugger;
    //     html2canvas(DATA).then(canvas => {
            
    //         let fileWidth = 1208;
    //         let fileHeight = canvas.height * fileWidth / canvas.width;
            
    //         const FILEURI = canvas.toDataURL('image/jpeg')
    //         let PDF = new jspdf("l", "mm", "a0");
            
    //         let position = 35;
    //         PDF.text('SSI Inventory Orders as of '+ new Date().toLocaleString(),170, 10);
            
    //         PDF.addImage(FILEURI, 'JPEG', 1, position, fileWidth, fileHeight)
            
    //         var filename = 'SSI Inventory Orders as of '+ new Date().toLocaleString() + '.pdf';
    //         debugger;
    //         PDF.save(filename);
    //     });
    }
}