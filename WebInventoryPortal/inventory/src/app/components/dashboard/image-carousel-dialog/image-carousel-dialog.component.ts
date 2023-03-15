import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';

// import { NgbActiveModal, NgbCarousel } from '@ng-bootstrap/ng-bootstrap';
// import { NgbCarouselConfig } from '@ng-bootstrap/ng-bootstrap';
import { inventoryitemimages } from 'src/app/models/inventoryitemimages';
import { InventoryitemimageService } from 'src/app/services/inventoryitemimage.service';

@Component({
  selector: 'app-image-carousel-dialog',
  templateUrl: './image-carousel-dialog.component.html',  
  styleUrls: ['./image-carousel-dialog.component.scss']
  // providers:[NgbCarouselConfig]
})
export class ImageCarouselDialogComponent implements OnInit {


  public inventoryItemImagesList:inventoryitemimages[];
  public id:number;

  constructor(public dialogRef: MatDialogRef<ImageCarouselDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ,private inventoryItemImageService:InventoryitemimageService
  
    ) {
    //debugger;
      console.log('data', this.data);
    this.id = this.data.id;

    }

  ngOnInit(){
    debugger;
   if(this.data.cond)
      this.getImagesForCondition(this.data.id,this.data.cond);
   else
      this.getImages(this.id);
    

  }

  // ngAfterViewInit() {
  //   //debugger;
  //   this.cdr.detectChanges();
  // }
//   ngAfterContentChecked(): void {
//     this.cdr.detectChanges();
//  }  

getImagesForCondition(id:number,condition:string)
  {
   // debugger;
    this.inventoryItemImageService.getInvItemImagesForCondition(id,condition).subscribe(imgdata => {
     // debugger;
            this.inventoryItemImagesList = imgdata;
            //console.log(this.inventoryItemImagesList);
            // this.data.invdata = this.inventoryItemImagesList;
            // console.log(this.data.invdata);
          });
  }

  getImages(id:number)
  {
    debugger;
    this.inventoryItemImageService.getInvItemImages(id).subscribe(data => {
     // debugger;
            this.inventoryItemImagesList = data;
            console.log(this.inventoryItemImagesList);
            // this.data.invdata = this.inventoryItemImagesList;
            // console.log(this.data.invdata);
          });
  }

 
    closeDialog(){
      this.dialogRef.close();
    }



}
