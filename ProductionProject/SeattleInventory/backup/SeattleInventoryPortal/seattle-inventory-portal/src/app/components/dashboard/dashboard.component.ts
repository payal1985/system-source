import { Component, OnInit, OnDestroy ,Output,EventEmitter} from '@angular/core';
import { MediaObserver, MediaChange } from '@angular/flex-layout';
import { Subscription } from 'rxjs';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import {FormControl, Validators} from '@angular/forms';
import { ActivatedRoute,Router } from '@angular/router';
import { ImageCarouselDialogComponent } from './image-carousel-dialog/image-carousel-dialog.component';

import {MatDialog} from '@angular/material/dialog';
import { AddCartDialogComponent } from './add-cart-dialog/add-cart-dialog.component';
import { CartService } from 'src/app/services/cart.service';
import { CartDialogComponent } from './cart-dialog/cart-dialog.component';
import { inventory } from 'src/app/model/inventory';
import { inventoryitem } from 'src/app/model/inventoryitem';

// import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  // device: any;
  mediaSub!: Subscription;
  deviceXs!: boolean;
  topVal = 0;
  showCategory!:string;
  id!: string;
  flr!:string


  inventoryCategorylist!: string[];
  inventoryBuldinglist!: string[];
  inventoryFloorlist!: string[];
  inventoryList: inventory[];
  //inventoryList:inventory[];

  invitemimgdata:any;

  buildingControl = new FormControl('', Validators.required);
  floorControl = new FormControl('', Validators.required);

   public totalItem : number = 0;
   public buildingFilterValue:string;
  // public cartInventoryItem : any = [];
  items = [];

  constructor(private route: ActivatedRoute,private routerNav:Router,public mediaObserver: MediaObserver
    ,private inventoryService: InventoryService
    ,private inventoryItemService: InventoryItemService
    //,private inventoryItemImageService:InventoryitemimageService
    ,public dialog: MatDialog
    ,private cartService : CartService) {

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

    // this.route.queryParams.subscribe(params => {
    //   this.flr = params['flr'];      
    // });

    this.inventoryService.getInventory(this.id,"","").subscribe(data =>{
      this.inventoryList = data;
    });

    //this.inventoryService.castInventory.subscribe(data => this.inventoryList = data);

    this.cartService.loadCart();
    this.items = this.cartService.getItems();
    this.totalItem = this.items.length;
    this.cartService.getProducts()
    .subscribe(res=>{
      debugger;
      //this.cartInventoryItem = res;
      this.totalItem = res.length;
      console.log(this.totalItem);
    })

    // this.cartService.getProducts()
    // .subscribe(res=>{
    //   debugger;
    //   this.cartInventoryItem = res;
    //   this.totalItem = res.length;
    //   console.log(this.totalItem);
    // })

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
    let e = this.deviceXs ? 10 : 0;
    return e - this.topVal;
  }

  applyFloorFilter(filterValue: string){    
    debugger
    console.log(filterValue);
    //this.router.navigate(['../',value]);
    this.routerNav.navigate([], {
      relativeTo: this.route,
      queryParams: {
        floor: filterValue
      },
      queryParamsHandling: 'merge'      
    });
    //this.inventoryService.inv.next(filterValue);  
    //this.inventoryService.castInventory.subscribe(inv => this.getInventoryBulding());

}

applyBuildingFilter(filterValue:string){
  debugger
  console.log(filterValue);
  //this.buildingFilterValue = filterValue;

  //this.router.navigate(['../',value]);
  this.routerNav.navigate([], {
    relativeTo: this.route,
    queryParams: {
      building: filterValue
    },
    queryParamsHandling: 'merge'
    // preserve the existing query params in the route
    //skipLocationChange: true
    // do not trigger navigation
  });

  //this.inventoryService.getInventory(this.id,this.buildingFilterValue,"");
}


openDialog(inv_item_id:number) {
   
      this.dialog.open(ImageCarouselDialogComponent, {
        width: '80%',
        height: '80%',
        data: {id:inv_item_id,invdata:this.invitemimgdata}
      });
   

    // dialogRef.afterClosed().subscribe(result => {
    //   console.log('The dialog was closed');
    //   this.inventoryItemImagesList = result;
    // });
  }

openAddCartDialog(inv_item:Array<inventoryitem>){
  debugger;
  console.log(inv_item);
  this.dialog.open(AddCartDialogComponent,{
    width: '70%',
    height: '50%',
    data: inv_item
  });
}

openCartDialog(){
  debugger;
  this.items = [...this.cartService.getItems()];
  this.dialog.open(CartDialogComponent,{
    width: '50%',
    height: '50%',
    //data: this.cartInventoryItem
    data: this.items
  });
}
  
}



// @Component({
//   selector: 'dialog-elements-example-dialog',
//   templateUrl: 'dialog-elements-example-dialog.html',
// })
// export class DialogElementsExampleDialog implements OnInit{

  // public inventoryItemImagesList:any;
  // public id!:number;

  // constructor(public dialogRef: MatDialogRef<DialogElementsExampleDialog>,
  //   @Inject(MAT_DIALOG_DATA) public data: any,private inventoryItemImageService:InventoryitemimageService) {
  //   debugger;
  //     console.log('data', this.data);
  //   this.id = this.data.id;
  // }

  // ngOnInit(){
  //   debugger;
  //   this.getImages(this.id);
    

  // }

  // getImages(id:number)
  // {
  //   debugger;
  //   this.inventoryItemImageService.getInvItemImages(id).subscribe(data => {
  //     debugger;
  //           this.inventoryItemImagesList = data;
  //           console.log(this.inventoryItemImagesList);
  //           this.data.invdata = this.inventoryItemImagesList;
  //           console.log(this.data.invdata);
  //   // this.inventoryItemImagesList = new inventoryitemimages[];
  //           // data.forEach(element => {
  //           //   var item = new inventoryitemimages();
  //           // });
  //         });


  // }

//}

