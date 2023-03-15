import { BreakpointObserver,Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { inventory } from 'src/app/model/inventory';
import { inventoryitem } from 'src/app/model/inventoryitem';
import { CartService } from 'src/app/services/cart.service';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { AddCartDialogComponent } from '../dashboard/add-cart-dialog/add-cart-dialog.component';
import { CartDialogComponent } from '../dashboard/cart-dialog/cart-dialog.component';
import { ImageCarouselDialogComponent } from '../dashboard/image-carousel-dialog/image-carousel-dialog.component';

@Component({
  selector: 'app-main-nav',
  templateUrl: './main-nav.component.html',
  styleUrls: ['./main-nav.component.css']
})
export class MainNavComponent implements OnInit {

  isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset);

  inventoryCategorylist: string[];
  inventoryBuldinglist!: string[];
  inventoryFloorlist!: string[];
  inventoryList: inventory[];
  items = [];

  showCategory:string;
  id: string;
  buildingId:string="";
  floorId:string="";
  totalItem : number = 0;

  buildingControl = new FormControl('');
  floorControl = new FormControl('');

  constructor(private route: ActivatedRoute,private routerNav:Router,private breakpointObserver: BreakpointObserver ,private inventoryService: InventoryService
    ,private inventoryItemService: InventoryItemService
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
    // debugger;
    // this.cartService.loadCart();
    // this.items = this.cartService.getItems();
    // this.totalItem = this.items.length;

  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.id = params['id'];      
    });

    this.showCategory = this.id;

    if(this.id !== "")
    {
      this.inventoryService.getInventory(this.id,"","").subscribe(data =>{
        this.inventoryList = data;
      });
    }

    //this.inventoryService.castInventory.subscribe(invitem => this.getInventories(invitem));

     this.cartService.loadCart();
    // this.items = this.cartService.getItems();
    // this.totalItem = this.items.length;
    debugger;    
    this.cartService.getProducts()
    .subscribe(res=>{
      debugger;
      //this.cartInventoryItem = res;
      this.totalItem = res.length;
      console.log(this.totalItem);
    });
    

  }

  // getInventories(value:string)
  // {
  //   this.inventoryService.getInventory(value).subscribe(data => {
  //     this.inventoryList = data;  
  //     console.log(this.inventoryList);
  //     console.log(data);
  //   });
  // }

  onClick(value:string){ 
    this.showCategory = value;  
    //debugger;
    this.inventoryService.getInventory(value,"","").subscribe(data =>{
      this.inventoryList = data;
    });
  
    console.log(this.inventoryList);      
  }

  applyFloorFilter(filterValue: string){    
    debugger
    console.log(filterValue);
  this.floorId = filterValue;


  this.floorId = (filterValue === undefined) ? "" : filterValue;

    //this.router.navigate(['../',value]);
    this.routerNav.navigate([], {
      relativeTo: this.route,
      queryParams: {
        floor: filterValue
      },
      queryParamsHandling: 'merge'      
    });

    this.inventoryService.getInventory(this.id,this.buildingId,this.floorId).subscribe(data =>{
      this.inventoryList = data;
    });
    
   // this.inventoryService.castInventory.next(filterValue);  

    //this.inventoryList = this.inventoryList.filter(f=>f.inventoryItemModelsDisplay.filter(inn=>inn.floor === filterValue) && f.inventoryItemModelsDisplay.length > 0);
    //this.inventoryService.inv.next(filterValue);  
    //this.inventoryService.castInventory.subscribe(inv => this.getInventoryBulding());


//     this.inventoryList = this.inventoryList.map((i)=>{
//       i.inventoryItemModelsDisplay = i.inventoryItemModelsDisplay.filter((x)=> x.floor === filterValue)  
//       return i;
//     }).filter(f=>f.inventoryItemModelsDisplay.length > 0);
// console.log(this.inventoryList);


}

applyBuildingFilter(filterValue:string){
  debugger
  console.log(filterValue);
  this.buildingId = filterValue;

  this.buildingId = (filterValue === undefined) ? "" : filterValue;

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
  debugger;

  // this.inventoryList = this.inventoryList.map((i)=>{
  //   i.inventoryItemModelsDisplay = i.inventoryItemModelsDisplay.filter((x)=> x.building === filterValue)  
  //   return i;
  // }).filter(f=>f.inventoryItemModelsDisplay.length > 0);

  // console.log(this.inventoryList);


  this.inventoryService.getInventory(this.id,this.buildingId,this.floorId).subscribe(data =>{
    this.inventoryList = data;
  });
}

// openDialog(inv_item_id:number) {
   
//   this.dialog.open(ImageCarouselDialogComponent, {
//     width: '80%',
//     height: '80%',
//     data: {id:inv_item_id,invdata:this.invitemimgdata}
//   });
// }

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
  this.cartService.loadCart();
  this.items = [...this.cartService.getItems()];
  this.dialog.open(CartDialogComponent,{
  width: '50%',
  // height: '50%',
  //data: this.cartInventoryItem
  data: this.items
  });
}

}
