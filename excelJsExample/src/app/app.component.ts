import { Component, OnInit} from '@angular/core';

import { Observable } from 'rxjs';
import { Workbook } from 'exceljs';
import * as fs from 'file-saver';
import { ImageService } from './image.service';
import {encode, decode} from 'node-base64-image';
import { HexBase64BinaryEncoding } from 'crypto';
import { toBase64String } from '@angular/compiler/src/output/source_map';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
 
  title = 'excelJs Example in Angular';
  imageToShow: any;
  //angular:any;
  isImageLoading: boolean = false;;
  rowCount:number=2;
  rowname:string = "";
  //ImageIdNewStorage = Array<number>();

  constructor(private _imageService: ImageService) {

    this.toDataURL('http://dev.systemsource.com/invssi/simg/MU-24.jpg', function (dataUrl:any) {
      console.log('base64 is-',  dataUrl)
      //imageToShow = dataUrl;
          })
   }

  ngOnInit() {
    //this.isImageLoading = false;
    //this.getImageFromService('http://maps.google.com/mapfiles/ms/icons/blue.png');
  }

   data: product[] = [
     {
       "Image": "http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=1",
       "Manufacturer": "A630 - asdfasdfasdfasfConic Tub Chair",
       "Description": "4-6 weeks",
       "Price": "$1185.60",
       "Quantity": "1",
       "imgBase64":"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=="
     },
     {
       "Image": "http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=2",
       "Manufacturer": "A631 - asdfasdfasdfasfConic Tub Chair",
       "Description": "4-6 weeks",
       "Price": "$1185.60",
       "Quantity": "1",
       "imgBase64":"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=="

     },
     {
       "Image": "http://dev.systemsource.com/static/RL/MicrosoftImageTest.pl?p_image_ID=3",
       "Manufacturer": "A632 - asdfasdfasdfasfConic Tub Chair",
       "Description": "4-6 weeks",
       "Price": "$1185.60",
       "Quantity": "1",
       "imgBase64":"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg=="

     }
   ]

//    this.data.forEach(e => {
    
// e.imgBase64 =  this.toDataURL(e.Image, function (dataUrl:any) {
//      // console.log('base64 is-',  dataUrl)
//           });
//    });

   exportExcel() {

     let workbook = new Workbook();
   let worksheet = workbook.addWorksheet('ProductSheet');

   worksheet.columns = [
    { header: 'Image', key: 'Image', width: 10 },
    { header: 'Manufacturer', key: 'Manufacturer', width: 32 },
     { header: 'Description', key: 'Description', width: 10 },
     { header: 'Price', key: 'Price', width: 10 },
     { header: 'Quantity', key: 'Quantity', width: 10, style: { font: { name: 'Arial Black', size:10} } },
     { header: 'NewImg', key: 'imgBase64', width: 30 },
   ];
 
  // const imageId1 = workbook.addImage({
  //   filename: 'http://maps.google.com/mapfiles/ms/icons/blue.png',
  //   extension: 'png',
  // });

  // const myBase64Image = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABGdBTUEAANbY1E9YMgAABNRJREFUWMO1lwtMm1UUgG8frEKBBdqZFcf7scmjpVAoZRRGH1BglLYIlFcdbxGWieAY6ICVjT19z20OJtM4jY+pmC26LKIZU9To1GQmksxkbibbfCwmRgkbcDy3xDJs6+BOm3z50/b/7/3+c889914CAMQdlq5fSHH3dWLsuESyN5wmacUvEZl+gBcutyn9RJHdHA5niBBylF79RBFbQxNK0+WGvfw0yzBZZ3ufGNt/cDxP2/HUB4XcTqCo4zLRNXxMEjS9cqF/0Ck+32taFCSHyORaWLO2DSIVdSBapQD8fUboLxmNy+pSauvOOMT/EwELEpe55X5826mQOAsYWr4Ba/8sVO0CJ+XbZyF/47cQJrVii2R6TfpDzc7n70SgtHeKJOU9XsPhEFBahqFqN0DFAIDVPgtl26bnsc84fqf/p5e+ClwuF6Ta/hb6PLOAqfMKyao+ocDOb6iKj0L1XsDOZhZ27MIM2PC+DOsbNBKzGdY31abOq2wCuc3nSIBEdioYw169ezGdz1O9BwCTFfzFMWM5TZ/zmQRkObvUXC4P8lq/hoodsOjOKeV4f2HbBPB4XhCfvTWXSSBQIn8sQCJ1NLaUt/97KGhOrAhR0ijYmQTwMxgmLXNk+dI6n4MmZJSiljY0xCowHJVS72iIVeBerBO0HSYBzP5DIfGWO4pAhLwaOIQcZhK4OyyzY7k4GovOjGOeLzUHaO4ESmQguif5USYBxfrnkrDOT+kbzkDlwNKiQBPQ0PIFcDjc2STDvgwmAV3DWYyC+hUEKne6qX6ewGjRYQuKMeDbK0Z0uC4wCZixgukbP4nkeQmuS7X2RRcjeh+WbyzH/N+1dR/Gmjuvsa4Fv5IyrOWq4hd1mMmTyQVP33ZG0P+VliM082+mGA8WlvbdcLTDLOCQwEaUpsFibBTW2U54nBV0mPSNYzjuHBqBmtK+uYWIfTHafMVJSe8kiUlr7RMIxWDpuorL70IJK9a6sr4/wS8wEsJlVfuptLnrZ2Le8pMDJoHChy86MbZfIkWP/MjzF68eD0+sdIkC/R6v6QGBd8D3+RvPLy9sv0jWt11wwiRgePCrBRRsmiCppkENHYq8lnPOBcraDxiVa8Bf5gMJmr5a2mEu3n8rTAKa2lEX9E2f4hKdeDI0YX6NoNfEnJ0g8Amc0NWfFegbxx1buFthrgP/JLf5S5Kc/5SexxdA0ebLc7lgnwbfwHCISmlqp5HS1Y+5wCSQWTHiSuW7eH3Ha5nA70Kq6TDY9mHmN43jus+fVFeOhOqbPnN5e+YIrC173S1ZVSfJygjNs7TSbXgS6IYDfAPCxjQ1o7gdf88tTAJK8wtuUd13jKxWbcrz9l2BOXATNx0qWBVr6lZXvE3SS465hUkAM94tSvMRkpT/hMRLIPxDW/cR+PithLis7oz0kpcJPZS4g0kgQbvNA3Yi1fUTb6H4u9jMTrhLKPpNUfCMKI1GyDTkFjYBTa9HZPodxF8U8YE4OBXHP+R8atHzJMV4CDnoFiaBqJRGj2BZJv7i6NeWCYS46Yw+LdVtJ/HZPR75XwTwzHCAVkW/wPDjuPUieCzzCJNAivGAR2hiBccW9VABSZR+Px7HPM4aCpOApylFUZe/RaJTH7BRATwBtWZUHCcqnAWe+DeBvwCBxinM8zq2PAAAAABJRU5ErkJggg==";
 
  // const imageId2 = workbook.addImage({
  //   base64: myBase64Image,
  //   extension: 'png',
  // });


 
  // const imageId1 = workbook.addImage({
  //   filename: 'http://maps.google.com/mapfiles/ms/icons/blue.png',
  //   extension: 'jpeg',
  // });
  // this.data.map(e => {
  //   const imageId = workbook.addImage({
  //     filename: e.imgBase64,
  //     extension: 'png',
  //   });
  //   debugger;
  //   this.ImageIdNewStorage.push(imageId);

  //   this.rowname = 'F'+this.rowCount +':F'+this.rowCount;

  //   worksheet.addRow({Manufacturer: e.Manufacturer, Description:e.Description, Price:e.Price, Quantity:e.Quantity,NewImg:e.imgBase64},"n");
    
  //   worksheet.addImage(imageId, this.rowname);
  //   this.rowCount++;
  // });

  // this.rowCount=2;
  // workbook.xlsx.writeBuffer().then((data) => {
  //   let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
  //   fs.saveAs(blob, 'ProductData.xlsx');
  // })
  this.data.forEach(e=>{
    const imageId = workbook.addImage({
      base64: e.imgBase64,
      extension: 'png',
    });
  
    debugger;
   // this.ImageIdNewStorage.push(imageId);
    e.imgBase64 = imageId;
  });

   this.data.forEach(e => {   

   //  this.imageToShow = this.getImageFromService('http://maps.google.com/mapfiles/ms/icons/blue.png');
   this.rowname = 'F'+this.rowCount +':F'+this.rowCount;

   debugger;
    // this.toDataURL(e.Image, function (dataUrl:any) {
    //        console.log('base64 is-',  dataUrl)
    //           });

   //new logic to test
    // const imageId1 = workbook.addImage({
    //       filename: 'https://pbs.twimg.com/profile_images/558329813782376448/H2cb-84q_400x400.jpeg',
    //       extension: 'jpeg',
    //     });


   // var imageid1 = this.getImageFromService(e.Image);
     worksheet.addRow({Manufacturer: e.Manufacturer, Description:e.Description, Price:e.Price, Quantity:e.Quantity,NewImg:e.imgBase64 },"n");
     // row.eachCell((cell, number) => {  
      
  //   // });
  
  worksheet.addImage(e.imgBase64, {tl: {col:6,row:6} ,ext: { width: 500, height: 200 }});
 // worksheet.addImage(imageId2, this.rowname);

    this.rowCount++;
   });
   this.rowCount=2;
   workbook.xlsx.writeBuffer().then((data) => {
     let blob = new Blob([data], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
     fs.saveAs(blob, 'ProductData.xlsx');
   })
 
   
   }


   toDataURL(url:string, callback:any) {
     debugger;
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        var reader = new FileReader();
        reader.onloadend = function () {
            callback(reader.result);
        }
        reader.readAsDataURL(xhr.response);
    };
    xhr.open('GET', url);
    xhr.responseType = 'blob';
    xhr.send();
}


createImageFromBlob(image: Blob) {
  let reader = new FileReader();
  reader.addEventListener("load", () => {
     this.imageToShow = reader.result;
  }, false);

  if (image) {
     reader.readAsDataURL(image);
  }
}

getImageFromService(src:string) {
  this.isImageLoading = true;
  this._imageService.getImage(src).subscribe(data => {
    this.createImageFromBlob(data);
    this.isImageLoading = false;
  }, error => {
    this.isImageLoading = false;
    console.log(error);
  });
}

  // compressImage(src:any, newX:number, newY:number) {
  //   return new Promise((res, rej) => {
  //     const img = new Image();
  //     img.src = src;
  //     img.onload = () => {
  //       const elem = document.createElement('canvas');
  //       elem.width = newX;
  //       elem.height = newY;
  //       const ctx = this.angular.elem.getContext('2d');
  //       ctx.drawImage(img, 0, 0, newX, newY);
  //       const data = ctx.canvas.toDataURL();
  //       console.log(data);
  //       res(data);
  //     }
  //     img.onerror = error => rej(error);
  //   })
  // }

  // this.getBase64ImageFromURL(imageUrl).subscribe(base64data => {
  //   console.log(base64data);
  //   this.base64Image = 'data:image/jpg;base64,' + base64data;
  // });

 
}
 
export interface product {
  Image: string
  Manufacturer: string
  Description: string
  Price: string
  Quantity: string
  imgBase64:any
}