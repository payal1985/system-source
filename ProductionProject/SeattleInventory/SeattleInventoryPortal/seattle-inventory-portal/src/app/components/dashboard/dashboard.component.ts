import { BreakpointObserver,Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TooltipPosition } from '@angular/material/tooltip';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { inventory } from 'src/app/model/inventory';
import { inventoryitem } from 'src/app/model/inventoryitem';
import { users } from 'src/app/model/users';
//import { CartService } from 'src/app/services/cart.service';
import { OrderCartService } from 'src/app/services/ordercart.service';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { LoginService } from 'src/app/services/login.service';
import { AddCartDialogComponent } from '../dashboard/add-cart-dialog/add-cart-dialog.component';
import { CartDialogComponent } from '../dashboard/cart-dialog/cart-dialog.component';
import { ImageCarouselDialogComponent } from '../dashboard/image-carousel-dialog/image-carousel-dialog.component';
import { debounceTime, switchMap, startWith } from "rxjs/operators";
import { of } from 'rxjs';
import { OrderCartDialogComponent } from './order-cart-dialog/order-cart-dialog.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  
  //isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset) || this.breakpointObserver.observe(Breakpoints.Tablet);
  isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset);
  //isTablet:Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Tablet);

  inventoryCategorylist: string[];
  inventoryBuldinglist!: string[];
  inventoryFloorlist!: string[];
  inventoryRoomlist:string[];
  inventoryList: inventory[];
  items = [];
  user = new users();
 // user : users[];

  showCategory:string;
  id: string;
  buildingId:string="";
  floorId:string="";
  roomId:string="";
  condId:string="";
  totalItem : number = 0;
  

  buildingControl = new FormControl('');
  floorControl = new FormControl('');
  roomControl = new FormControl('');
  condControl = new FormControl('');
  search = new FormControl();

  public positionOptions: TooltipPosition[] = ['left']; // Tooltip postion 
  public position = new FormControl(this.positionOptions[0]); 
  public searchText:any;

  constructor(private route: ActivatedRoute,private routerNav:Router
    ,private breakpointObserver: BreakpointObserver 
    ,private inventoryService: InventoryService
    ,private inventoryItemService: InventoryItemService
    ,public dialog: MatDialog
    //,public dialogRef:MatDialogRef<ImageCarouselDialogComponent>
    //,private cartService : CartService //old flow code support this line
    ,private cartService : OrderCartService
    ,private loginService: LoginService) { 
//debugger;
      this.loginService.loadUser().subscribe(usr=>{
      //  debugger;
        this.user = usr;
        //usr.clientId
        // this.user.user_id = usr.user_id;
        // this.user.username = usr.username;
        // this.user.client_id = usr.client_id;
        // this.user.password = usr.password;
        // this.user.isadmin = usr.isadmin;
       //// this.user.push(usr);
        console.log(this.user);
      });

    this.inventoryService.getInventoryCategory(this.user.clientId).subscribe(data => {
      //debugger;
      this.inventoryCategorylist = data;
    });

    this.inventoryItemService.getBuildings(this.user.clientId).subscribe(data => {
      this.inventoryBuldinglist = data;
    });
    this.inventoryItemService.getFloor(this.user.clientId).subscribe(data => {
      this.inventoryFloorlist = data;
    });
    this.inventoryItemService.getRooms(this.user.clientId).subscribe(room => {
      this.inventoryRoomlist = room;
    });
  
    // debugger;
    // this.cartService.loadCart();
    // this.items = this.cartService.getItems();
    // this.totalItem = this.items.length;
  }

  ngOnInit(): void {
//debugger;
    // this.breakpointObserver
    // .observe([Breakpoints.Small, Breakpoints.HandsetPortrait])
    // .subscribe((state: BreakpointState) => {
    //   if (state.matches) {
    //     console.log(
    //       'Matches small viewport or handset in portrait mode'
    //     );
    //   }
    // });

    this.route.queryParams.subscribe(params => {
      this.id = params['id'];      
    });

    this.showCategory = this.id;

    if(this.id !== "")
    {
      this.inventoryService.getInventory(this.id,this.user.clientId,"","","","").subscribe(data =>{
        this.inventoryList = data;
      });
    }

    //this.inventoryService.castInventory.subscribe(invitem => this.getInventories(invitem));

     this.cartService.loadCart(this.user.clientId);
    // this.items = this.cartService.getItems();
    // this.totalItem = this.items.length;
    //debugger;    
    this.cartService.getProducts(this.user.clientId)
    .subscribe(res=>{
     // debugger;
      //this.cartInventoryItem = res;
      this.totalItem = res.length;
     // console.log(this.totalItem);
    });
    

  }

  $search = this.search.valueChanges.pipe(
    startWith(null),
    debounceTime(200),
    switchMap((res: string) => {
      if (!res) return of(this.inventoryList);
      res = res.toLowerCase();
      return of(
        this.inventoryList.filter(x => x.manuf.toLowerCase().indexOf(res) >= 0)
      );
    })
  );

  onClick(value:string){ 

    if(value=="home")
    {
      this.showCategory = "";  
      this.inventoryList = [];

    }
    else
    {
      this.showCategory = value;  
      //debugger;
      this.inventoryService.getInventory(value,this.user.clientId,"","","","").subscribe(data =>{
        this.inventoryList = data;
      });
    }
    
  
    console.log(this.inventoryList);      
  }

  applyFloorFilter(filterValue: string){    
    //debugger
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

    this.inventoryService.getInventory(this.id,this.user.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
      this.inventoryList = data;
    });
}

applyBuildingFilter(filterValue:string){
  //debugger
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

  this.inventoryService.getInventory(this.id,this.user.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
  
    this.inventoryList = data;
  });
}

applyRoomFilter(filterValue: string){    
  //debugger
  console.log(filterValue);
this.roomId = filterValue;


this.roomId = (filterValue === undefined) ? "" : filterValue;

  //this.router.navigate(['../',value]);
  this.routerNav.navigate([], {
    relativeTo: this.route,
    queryParams: {
      room: filterValue
    },
    queryParamsHandling: 'merge'      
  });

  this.inventoryService.getInventory(this.id,this.user.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
    this.inventoryList = data;
  });
}

applyConditionFilter(filterValue: string){    
  //debugger
  console.log(filterValue);
this.condId = filterValue;


this.condId = (filterValue === undefined) ? "" : filterValue;

  //this.router.navigate(['../',value]);
  this.routerNav.navigate([], {
    relativeTo: this.route,
    queryParams: {
      cond: filterValue
    },
    queryParamsHandling: 'merge'      
  });

  this.inventoryService.getInventory(this.id,this.user.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
    this.inventoryList = data;
  });
}

openDialog(inv_id:number) {
   
  this.dialog.open(ImageCarouselDialogComponent, {    
    width: '100%',
    height: '100%',   
    //panelClass: 'image-dialog',
    data: {id:inv_id}
  });
}

openDialogNew(inv_item_id:number) {
   
  this.dialog.open(ImageCarouselDialogComponent, {    
    width: '100%',
    height: '100%',   
    //panelClass: 'image-dialog',
    data: {id:inv_item_id}
  });
}

openAddCartDialog(event:Event,inv_item:Array<inventoryitem>){
//debugger;
event.stopPropagation();
console.log(inv_item);
this.dialog.open(AddCartDialogComponent,{
 width: '100%',
//  height: '50%',
panelClass: 'add-cart-dialog',
data: inv_item
});
}

openCartDialog(){
  //debugger;
  this.cartService.loadCart(this.user.clientId);
  this.items = [...this.cartService.getItems()];
  this.dialog.open(CartDialogComponent,{
  width: '40%',
  // height: '50%',
  //data: this.cartInventoryItem
 panelClass: 'cart-dialog',
  data: this.items
  });
}

logout(){
  this.loginService.logout();
  this.routerNav.navigateByUrl('');
}

openAddOrderCartDialog(event:Event,inv_item:Array<inventoryitem>){
  //debugger;
  event.stopPropagation();
  console.log(inv_item);
  this.dialog.open(OrderCartDialogComponent,{
  width: '100%',
  height: '50%',
  panelClass: 'add-cart-dialog',
  data: {invitem: inv_item, username: this.user.firstName + ' ' + this.user.lastName, email: this.user.email}
  });
 }
}
