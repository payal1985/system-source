import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { InventoryitemimageService } from 'src/app/services/inventoryitemimage.service';

@Component({
  selector: 'app-image-carousel-dialog',
  templateUrl: './image-carousel-dialog.component.html',
  styleUrls: ['./image-carousel-dialog.component.css']
})
export class ImageCarouselDialogComponent implements OnInit {


  public inventoryItemImagesList:any;
  public id!:number;

  constructor(public dialogRef: MatDialogRef<ImageCarouselDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,private inventoryItemImageService:InventoryitemimageService) {
    debugger;
      console.log('data', this.data);
    this.id = this.data.id;
  }

  ngOnInit(){
    debugger;
    this.getImages(this.id);
    

  }

  getImages(id:number)
  {
    debugger;
    this.inventoryItemImageService.getInvItemImages(id).subscribe(data => {
      debugger;
            this.inventoryItemImagesList = data;
            console.log(this.inventoryItemImagesList);
            this.data.invdata = this.inventoryItemImagesList;
            console.log(this.data.invdata);
          });
  }

}
