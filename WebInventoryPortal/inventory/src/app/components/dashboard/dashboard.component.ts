import { BreakpointObserver,Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { TooltipPosition } from '@angular/material/tooltip';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { inventory } from 'src/app/models/inventory';
import { inventoryitem } from 'src/app/models/inventoryitem';
import { users } from 'src/app/models/users';
//import { CartService } from 'src/app/services/cart.service';
//import { OrderCartService } from 'src/app/services/ordercart.service';
import { InventoryService } from 'src/app/services/inventory.service';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { LoginService } from 'src/app/services/login.service';
//import { AddCartDialogComponent } from '../dashboard/add-cart-dialog/add-cart-dialog.component';
import { CartDialogComponent } from '../dashboard/cart-dialog/cart-dialog.component';
import { ImageCarouselDialogComponent } from '../dashboard/image-carousel-dialog/image-carousel-dialog.component';
import { debounceTime, switchMap, startWith } from "rxjs/operators";
import { of } from 'rxjs';
import { OrderCartDialogComponent } from './order-cart-dialog/order-cart-dialog.component';
import { client } from 'src/app/models/client';
import { ClientService } from 'src/app/services/client.service';
import { AddCartDialogComponent } from './add-cart-dialog/add-cart-dialog.component';
import { AddCartService } from 'src/app/services/addcart.service';
import { itemtyeps } from 'src/app/models/itemtypes';
import { inventorybuilding } from 'src/app/models/inventorybuilding';
import { inventoryfloor } from 'src/app/models/inventoryfloor';
import { addcart } from 'src/app/models/addcart';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  
  //isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset) || this.breakpointObserver.observe(Breakpoints.Tablet);
  isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset);
  //isTablet:Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Tablet);

  inventoryCategorylist: itemtyeps[];
  inventoryBuldinglist: inventorybuilding[];
  inventoryFloorlist: inventoryfloor[];
  inventoryRoomlist:string[];
  inventoryConditionlist:string[];
  inventoryClientlist:client[];
  inventoryList: inventory[];
  items: addcart[];
  currentUser: users;
  currentClient: client;
 // user : users[];

  showCategory:string;
  id: string;
  categoryId:number;
  buildingId:string="";
  floorId:string="";
  roomId:string="";
  condId:string="";
  totalItem : number = 0;
  

  clientControl = new FormControl('');
  buildingControl = new FormControl('');
  floorControl = new FormControl('');
  roomControl = new FormControl('');
  condControl = new FormControl('');
  search = new FormControl();

  public positionOptions: TooltipPosition[] = ['left']; // Tooltip postion 
  public position = new FormControl(this.positionOptions[0]); 
  public searchText:any;

  isExpanded = true;
  showSubmenu: boolean = false;
  isShowing = false;
  showSubSubMenu: boolean = false;
  
  constructor(private route: ActivatedRoute,private routerNav:Router
    ,private breakpointObserver: BreakpointObserver 
    ,private inventoryService: InventoryService
    ,private inventoryItemService: InventoryItemService
    ,public dialog: MatDialog
    //,public dialogRef:MatDialogRef<ImageCarouselDialogComponent>
    //,private cartService : CartService //old flow code support this line
    //,private cartService : OrderCartService // second old flow code support this line
    ,private cartService : AddCartService
    ,private loginService: LoginService
    ,private clientService: ClientService) { 
//debugger;
      // this.loginService.loadUser().subscribe(usr=>{
      // //  debugger;
      //   this.user = usr;
      //   console.log(this.user);
      // });
      this.loginService.currentUser.subscribe(x => this.currentUser = x);

      this.clientService.getClients(this.currentUser.userName,this.currentUser.password).subscribe(client=>{
       //debugger;
        this.inventoryClientlist = client;
      });

    //  this.clientService.currentClient.subscribe(x=>this.currentClient = x);
  this.currentClient=new client();
  // this.currentCategoryId = new BehaviorSubject<number>(0);
  // this.currentCatId = this.currentCategoryId.asObservable()
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

  this.categoryId = JSON.parse(localStorage.getItem('categoryId'));
   console.log(this.categoryId);
  }

  $search = this.search.valueChanges.pipe(  
    startWith(null),
    debounceTime(200),
    switchMap((res: string) => {
      //debugger;
      if (!res) return of(this.inventoryList);
      res = res.toLowerCase();
      return of(
        this.inventoryList.filter(x => x.manuf.toLowerCase().indexOf(res) >= 0)
      );
    })
  );

  searchInventory()
  {
  //  debugger;
    this.inventoryService.searchInventory(this.searchText,this.currentClient.clientId,"","","","").subscribe(data =>{
      this.inventoryList = data;
    });

   // this.id = this.inventoryList.filter(x=>x.category).toString();
  }

  onClick(categoryid:number,categoryname:string){ 

    if(categoryname=="home")
    {
      //this.showCategory = "";  
      this.inventoryList = [];

    }
    else
    {
      //this.showCategory = value;  
     // debugger;
      this.inventoryService.getInventory(categoryid,this.currentClient.clientId,"","","","").subscribe(data =>{
        //debugger;
        this.inventoryList = data;
        //console.log(this.inventoryList);


      });
      this.categoryId = categoryid;
      localStorage.setItem('categoryId', JSON.stringify(categoryid));      
    }
    
  
    //console.log(this.inventoryList);      
  }

  applyClientFilter(filterValue: number){

    this.inventoryService.getInventoryCategory(filterValue).subscribe(data => {
      //debugger;
      this.inventoryCategorylist = data;
      //console.log( this.inventoryCategorylist);
    });

    this.inventoryItemService.getBuildings(filterValue).subscribe(data => {
      this.inventoryBuldinglist = data;
    });
    this.inventoryItemService.getFloor(filterValue).subscribe(data => {
      this.inventoryFloorlist = data;
    });
    this.inventoryItemService.getRooms(filterValue).subscribe(room => {
      this.inventoryRoomlist = room;
    });    
    this.inventoryItemService.getConditions().subscribe(cond => {
      this.inventoryConditionlist = cond;
    });

    
    this.clientService.setClient(this.inventoryClientlist.find(x=>x.clientId == filterValue));
    this.clientService.currentClient.subscribe(x => this.currentClient = x);

   //removed from oninit and paste here as per client dropdown implementation

   this.route.queryParams.subscribe(params => {
    this.id = params['id'];      
  });

  //this.showCategory = this.id;

  this.id = (this.id === undefined) ? "" : this.id;


  if(this.id !== "" )
  {
    this.inventoryService.getInventory(this.categoryId,this.currentClient.clientId,"","","","").subscribe(data =>{
      this.inventoryList = data;
     // console.log(this.inventoryList);
    });
  }
  
   this.cartService.loadCart(this.currentClient.clientId);
  // this.items = this.cartService.getItems();
  // this.totalItem = this.items.length;
  //debugger;    
  this.cartService.getProducts(this.currentClient.clientId)
  .subscribe(res=>{
   // debugger;
    //this.cartInventoryItem = res;
    this.totalItem = res.length;
   // console.log(this.totalItem);
  });
  


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

    this.searchText = (this.searchText === undefined) ? "" : this.searchText;

    if(this.searchText)
    {
      this.inventoryService.searchInventory(this.searchText,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
        this.inventoryList = data;
      });
    }
    else{
      this.inventoryService.getInventory(this.categoryId,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
        this.inventoryList = data;
      });
    }
 
}

applyBuildingFilter(filterValue:string){
 // debugger
  console.log(filterValue);
  //this.buildingId = filterValue;

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

  this.searchText = (this.searchText === undefined) ? "" : this.searchText;

    if(this.searchText)
    {
      this.inventoryService.searchInventory(this.searchText,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
        this.inventoryList = data;
      });
    }
    else{
    this.inventoryService.getInventory(this.categoryId,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
      this.inventoryList = data;
    });
  }
}

applyRoomFilter(filterValue: string){    
 // debugger
  console.log(filterValue);
//this.roomId = filterValue;


this.roomId = (filterValue === undefined) ? "" : filterValue;

  //this.router.navigate(['../',value]);
  this.routerNav.navigate([], {
    relativeTo: this.route,
    queryParams: {
      room: filterValue
    },
    queryParamsHandling: 'merge'      
  });

  this.searchText = (this.searchText === undefined) ? "" : this.searchText;

  if(this.searchText)
  {
    this.inventoryService.searchInventory(this.searchText,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
      this.inventoryList = data;
    });
  }
  else{
  this.inventoryService.getInventory(this.categoryId,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
    this.inventoryList = data;
    });
  }
}

applyConditionFilter(filterValue: string){    
  //debugger
  console.log(filterValue);
//this.condId = filterValue;


this.condId = (filterValue === undefined) ? "" : filterValue;

  //this.router.navigate(['../',value]);
  this.routerNav.navigate([], {
    relativeTo: this.route,
    queryParams: {
      cond: filterValue
    },
    queryParamsHandling: 'merge'      
  });

  this.searchText = (this.searchText === undefined) ? "" : this.searchText;

  if(this.searchText)
  {
    this.inventoryService.searchInventory(this.searchText,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
      this.inventoryList = data;
    });
  }
  else{
  this.inventoryService.getInventory(this.categoryId,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId).subscribe(data =>{
    this.inventoryList = data;
  });
}
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
//  width: '100%',
//  height: '40%',
 panelClass: 'add-cart-dialog',
 data: inv_item
});
}

openCartDialog(){
  //debugger;
  this.cartService.loadCart(this.currentClient.clientId);
  this.items = [...this.cartService.getItems()];
  this.dialog.open(CartDialogComponent,{
  // width: '100%',
  //  height: '50%',
  //data: this.cartInventoryItem
 panelClass: 'cart-dialog',
  data: {invitem:this.items, username: this.currentUser.firstName + ' ' + this.currentUser.lastName, email: this.currentUser.email,clientName:this.currentClient.clientName,userId:this.currentUser.userId}
  // position:{
  //   left:'150px',top:'70px'
  // }
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
  data: {invitem: inv_item, username: this.currentUser.firstName + ' ' + this.currentUser.lastName, email: this.currentUser.email}
  });
 }
}
