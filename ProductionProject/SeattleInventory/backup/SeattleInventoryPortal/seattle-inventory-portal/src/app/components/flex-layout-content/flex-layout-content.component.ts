import { Component, OnInit,Input } from '@angular/core';
import { MediaObserver, MediaChange } from '@angular/flex-layout';
import { Subscription } from 'rxjs';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { InventoryitemimageService } from 'src/app/services/inventoryitemimage.service';
import {FormControl, Validators} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-flex-layout-content',
  templateUrl: './flex-layout-content.component.html',
  styleUrls: ['./flex-layout-content.component.css']
})
export class FlexLayoutContentComponent implements OnInit {
//   @Input() deviceXs!: boolean;
//   topVal = 0;

//   constructor() { }

//   ngOnInit(): void {
//   }
  
//   onScroll(e:any) {
//     let scrollXs = this.deviceXs ? 55 : 73;
//     if (e.srcElement.scrollTop < scrollXs) {
//       this.topVal = e.srcElement.scrollTop;
//     } else {
//       this.topVal = scrollXs;
//     }
//   }
//   sideBarScroll() {
//     let e = this.deviceXs ? 160 : 130;
//     return e - this.topVal;
//   }
// }
mediaSub!: Subscription;
@Input() deviceXs!: boolean;
//deviceXs!: boolean;
topVal = 0;
showCategory!:string;
id!: string;

// @Output() childEvent = new EventEmitter<string>();
// catValue !: string;

inventoryCategorylist!: string[];
inventoryBuldinglist!: string[];
inventoryFloorlist!: string[];
inventoryList:any;
// inventoryItemImagesList:any;
observableData: any[] = [];
inventoryItemImagesList !: Observable<any[]>;

buildingControl = new FormControl('', Validators.required);
floorControl = new FormControl('', Validators.required);

constructor(private route: ActivatedRoute,public mediaObserver: MediaObserver
  ,private inventoryService: InventoryService
  ,private inventoryItemService: InventoryItemService
  ,private inventoryItemImageService:InventoryitemimageService
  ,public dialog: MatDialog) {
  this.inventoryService.getInventoryCategory().subscribe(data => {
    this.inventoryCategorylist = data;
  });
  this.inventoryItemService.getBuildings().subscribe(data => {
    this.inventoryBuldinglist = data;
  });
  this.inventoryItemService.getFloor().subscribe(data => {
    this.inventoryFloorlist = data;
  });
  
}
ngOnInit() {
  //this.device = 'side'; 
  this.mediaSub = this.mediaObserver.media$.subscribe((res: MediaChange) => {
    console.log(res.mqAlias);
    this.deviceXs = res.mqAlias === "xs" ? true : false;
  });

  this.route.queryParams.subscribe(params => {
    this.id = params['id'];      
  });

  this.inventoryService.getInventory(this.id,"","").subscribe(data =>{
    this.inventoryList = data;
  });

  // debugger;
  // const members =  this.inventoryItemImageService.getInvItemImages(0).subscribe(
  //   member => console.log(member)
  // )
}


onClick(value:string){ 
this.showCategory = value;  
//debugger;
this.inventoryService.getInventory(value,"","").subscribe(data =>{
  this.inventoryList = data;
});

console.log(this.inventoryList);
  
}



ngOnDestroy() {
  this.mediaSub.unsubscribe();
}


onScroll(e:any) {
  let scrollXs = this.deviceXs ? 55 : 73;
  if (e.srcElement.scrollTop < scrollXs) {
    this.topVal = e.srcElement.scrollTop;
  } else {
    this.topVal = scrollXs;
  }
}
sideBarScroll() {
  let e = this.deviceXs ? 160 : 87;
  return e - this.topVal;
}



}

