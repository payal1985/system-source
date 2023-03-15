import { ChangeDetectorRef, Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { inventoryitemimages } from 'src/app/models/inventoryitemimages';
import { InventoryitemimageService } from 'src/app/services/inventoryitemimage.service';


@Component({
  selector: 'app-image-slider-dialog',
  templateUrl: './image-slider-dialog.component.html',
  styleUrls: ['./image-slider-dialog.component.scss']
})
export class ImageSliderDialogComponent implements OnInit {

  public inventoryItemImagesList:inventoryitemimages[];
  public id!:number;
  
  constructor(public dialogRef: MatDialogRef<ImageSliderDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,private inventoryItemImageService:InventoryitemimageService
    ,private cdr: ChangeDetectorRef) {
    //debugger;
      console.log('data', this.data);
    this.id = this.data.id;

  }

  ngOnInit(){  
    this.getImages(this.id);
  }
  
  ngAfterContentChecked(): void {
    this.cdr.detectChanges();
 }  

  getImages(id:number)
  {
    this.inventoryItemImageService.getInvItemImages(id).subscribe(data => {
            this.inventoryItemImagesList = data;
            console.log(this.inventoryItemImagesList);
          });
  }

  closeDialog(){
    this.dialogRef.close();

  }
}
