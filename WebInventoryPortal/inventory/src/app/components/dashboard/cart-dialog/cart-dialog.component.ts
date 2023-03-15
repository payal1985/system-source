import { Component, OnInit,Inject,ViewChild,AfterViewInit, ChangeDetectorRef  } from '@angular/core';
import { MatDialog,MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';

import { FormControl, Validators} from '@angular/forms';

import { InventoryorderService } from 'src/app/services/inventoryorder.service';
import { addcart } from 'src/app/models/addcart';
import { AddCartService } from 'src/app/services/addcart.service';

import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { DatePipe } from '@angular/common';
import { ImageCarouselDialogComponent } from '../image-carousel-dialog/image-carousel-dialog.component';
import { AddDialogComponent } from './add-dialog/add-dialog.component';
import { ErrorStateMatcher } from '@angular/material/core';
import { map, Observable, startWith } from 'rxjs';
import { inventorybuilding } from 'src/app/models/inventorybuilding';
import { inventoryfloor } from 'src/app/models/inventoryfloor';

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null): boolean {
    //const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched));
  }
}

@Component({
  selector: 'app-cart-dialog',
  templateUrl: './cart-dialog.component.html',
  styleUrls: ['./cart-dialog.component.scss']
})
export class CartDialogComponent implements OnInit,AfterViewInit  {


 // cartDataSource:cart[]; //old flow code support line
 //cartDataSource:ordercart[];
  cartDataSource:addcart[];
 inventoryBuldinglist: inventorybuilding[];
 inventoryFloorlist: inventoryfloor[];
 depcostCenterlist:string[];
 firstError :string;
 masterSelected:boolean;
 addDialogResult: boolean;
//  ordCart: ordercart[];
selectedDepCostCenter:Observable<string[]>;

  //items = [];
 // emptyData = new MatTableDataSource([{ empty: "row" }]);
  displayedColumns: string[] = ['inv_image_name','item_code', 'description', 'pullinfo','cond','pullqty','remove','select','dest_bldg','dest_flr','dest_loc','depcost_cener','req_inst_date','comment'];
  // displayedColumns: string[] = ['inv_image_name','item_code', 'description', 'pullinfo','cond','pullqty','remove','email','requestor','dest_bldg','dest_flr','dest_loc','req_inst_date','comment'];
  // displayedColumns: string[] = ['inv_image_name','item_code', 'description', 'pullinfo','cond','pullqty','remove'];
  //displayedColumns: string[] = ['building', 'floor', 'mploc', 'qty','star'];
  qtyFormControl = new FormControl('', []);
  // @ViewChild(MatTable, {static: false}) table: MatTable<any>;
  clicked = false;
  checkedList:addcart[];

  emailFormControl = new FormControl('', [Validators.required,Validators.email]);
 requestorFormControl = new FormControl('', [Validators.required]);
dest_buildingFormControl = new FormControl('', [Validators.required]);
dest_floorFormControl = new FormControl('', [Validators.required]);
dest_locFormControl = new FormControl('', [Validators.required]);
req_inst_dateFormControl  = new FormControl('', [Validators.required]);
commentFormControl = new FormControl('');
dest_depcostFormControl = new FormControl('',[Validators.required]);
matcher = new MyErrorStateMatcher();


  constructor(public dialogRef: MatDialogRef<CartDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    //,private cartService: OrderCartService
    ,private cartService: AddCartService
    ,public dialog: MatDialog
    ,private cdr: ChangeDetectorRef
    ,private inventoryOrderService: InventoryorderService
    ,private inventoryItemService: InventoryItemService
    ,private datePipe: DatePipe
    
    ) { 
      //debugger;
      console.log('cart data', this.data);
      if(this.data.invitem.length >0){
        this.inventoryItemService.getBuildings(this.data.invitem[0].clientID).subscribe(dataBldg => {
          //debugger;
          this.inventoryBuldinglist = dataBldg;
        });
        this.inventoryItemService.getFloor(this.data.invitem[0].clientID).subscribe(dataFlr => {
          //debugger;
          this.inventoryFloorlist = dataFlr;
        });
      }
   

      this.inventoryItemService.getDepCostCenters().subscribe(dataDepCostCenter => {
        //debugger;
        this.depcostCenterlist = dataDepCostCenter;
        //this.selectedDepCostCenter = this.depcostCenterlist;
        this.selectedDepCostCenter = this.dest_depcostFormControl.valueChanges
        .pipe(
          startWith(''),
          map(value => this.filterDepCostCenter(value))
        );
      });


    }

  ngOnInit(): void {
//debugger;
this.cartDataSource = this.data.invitem;
this.cartDataSource.forEach(x =>  {
  x.isSelected = false,
  x.clientName = this.data.clientName,
  x.userId = this.data.userId
  //x.arrayBuildings=this.inventoryBuldinglist;
});


//debugger;
    console.log('cart data Source data', this.cartDataSource);     
  }

  ngAfterViewInit() {
    //debugger;
    this.cdr.detectChanges();
  }

  filterDepCostCenter(value: string): string[] {
    const filterValue = value.toLowerCase();
//debugger;
    return this.depcostCenterlist.filter(option => option.toLowerCase().includes(filterValue));
  }
  // search(query: string){
  //   console.log('query', query)
  //   let result = this.select(query)
  //   this.selectedDepCostCenter = result;
  // }

  // select(query: string):string[]{
  //   let result: string[] = [];
  //   for(let a of this.depcostCenterlist){
  //     if(a.toLowerCase().indexOf(query) > -1){
  //       result.push(a)
  //     }
  //   }
  //   return result
  // }

  pullqtychange($event:any,index:number){
    //debugger;
    if($event.target.value > this.cartDataSource[index].qty)
    {
      alert("There are not enough items availalbe");
      this.cartDataSource[index].pullQty = this.cartDataSource[index].qty;
    }
    else
    {
      this.cartDataSource[index].pullQty = $event.target.value; 
      let i:any;
      i = this.checkedList.findIndex(_item => _item.inventoryID === this.cartDataSource[index].inventoryID && _item.inventoryItemID === this.cartDataSource[index].inventoryItemID && _item.clientID === this.cartDataSource[index].clientID);
          
        if (i > -1) {
            this.checkedList[i].pullQty = $event.target.value;
        }
    }
   // console.log('new item',item);
  }

  //removefromcart()  {
  removeFromCart(item:any,i:any)  {
    //debugger;
    //this.cartService.removeCartItem(item);
    this.cartService.removeItem(item,i);
    //this.cartDataSource = this.cartService.getItems();
    this.cartDataSource = this.cartDataSource
      .filter(i => i !== item)
      .map((i, idx) => (i.inventoryItemID = (idx + 1), i));
   // this.items = this.cartService.getItems();
//debugger;


  }

  checkUncheckAll() {
    debugger;
    for (var i = 0; i < this.cartDataSource.length; i++) {
      this.cartDataSource[i].isSelected = this.masterSelected;
      // (<HTMLInputElement>document.getElementById("numberFormControl"+i)).value = this.cartDataSource[i].qty.toString();
      //this.cartDataSource[i].pullqty = this.cartDataSource[i].qty;
      // this.dataSource[i].destBuilding = (this.showOtherBldg && this.otherBldgFormControl.value != "") ? this.otherBldgFormControl.value : this.dest_buildingFormControl.value;
      // this.dataSource[i].destFloor = (this.showOtherFlr && this.otherFlrFormControl.value != null) ?this.otherFlrFormControl.value : this.dest_floorFormControl.value;
      this.cartDataSource[i].destBuilding = this.dest_buildingFormControl.value;
      this.cartDataSource[i].destFloor =  this.dest_floorFormControl.value;
      this.cartDataSource[i].destRoom = this.dest_locFormControl.value;
      this.cartDataSource[i].destDepCostCenter = this.dest_depcostFormControl.value;
      this.cartDataSource[i].instDate = this.datePipe.transform(this.req_inst_dateFormControl.value, "yyyy-MM-dd HH:mm:ss");
      //this.cartDataSource[i].inst_date = new Date(this.req_inst_dateFormControl.value).toDateString();
      this.cartDataSource[i].comment = this.commentFormControl.value;
      
      // this.cartDataSource[i].inventoryBuildingID= ;
      // this.cartDataSource[i].inventoryFloorID=
      this.cartDataSource[i].destInventoryBuildingID= (this.dest_buildingFormControl.value) ? this.inventoryBuldinglist.find(bldg=>bldg.inventoryBuildingName == this.dest_buildingFormControl.value).inventoryBuildingId : 0;
      this.cartDataSource[i].destInventoryFloorID= (this.dest_floorFormControl.value) ? this.inventoryFloorlist.find(flr=>flr.inventoryFloorName == this.dest_floorFormControl.value).inventoryFloorId : 0;
   
    }
    if(this.masterSelected)
    {
      //this.dest_locFormControl.setValue(this.dest_locFormControl.value);
      this.req_inst_dateFormControl.setValue(this.req_inst_dateFormControl.value);
      // this.commentFormControl.setValue(this.commentFormControl.value);
      // this.dest_buildingFormControl.setValue(this.dest_buildingFormControl.value);
      // this.dest_floorFormControl.setValue(this.dest_floorFormControl.value);
      // this.dest_depcostFormControl.setValue(this.dest_depcostFormControl.value);
    }
    else{
      this.dest_locFormControl.setValue("");
      this.req_inst_dateFormControl.setValue("");
      this.commentFormControl.setValue("");
      this.dest_buildingFormControl.setValue("");
      this.dest_floorFormControl.setValue("");
      this.dest_depcostFormControl.setValue("");
    }

    this.getCheckedItemList();
  }
  isAllSelected() {
    //debugger;
    this.masterSelected = this.cartDataSource.every(function(item:any) {
     // debugger;
        return item.isSelected == true;
      })

      if(!this.masterSelected)
      {
        for (var i = 0; i < this.cartDataSource.length; i++) {
          if(!this.cartDataSource[i].isSelected)
          {
            //debugger;
            this.cartDataSource[i].destBuilding = "";
            this.cartDataSource[i].destFloor = "";
            this.cartDataSource[i].destRoom = "";
            this.cartDataSource[i].destDepCostCenter = "";
            this.cartDataSource[i].instDate = "";
            this.cartDataSource[i].comment = "";
            this.cartDataSource[i].pullQty = 0;
            this.cartDataSource[i].destInventoryBuildingID = 0;
            this.cartDataSource[i].destInventoryFloorID = 0;
            

            // (<HTMLInputElement>document.getElementById("dest_locFormControl"+i)).value = "";
            // (<HTMLInputElement>document.getElementById("dest_depcostFormControl"+i)).value = "";
             (<HTMLInputElement>document.getElementById("req_inst_date"+i)).value = "";
            // (<HTMLInputElement>document.getElementById("commentFormControl"+i)).value = "";
            // (<HTMLInputElement>document.getElementById("numberFormControl"+i)).value = "";
          }
          else
          {
            this.cartDataSource[i].destBuilding = this.dest_buildingFormControl.value;
            this.cartDataSource[i].destFloor =  this.dest_floorFormControl.value;
            this.cartDataSource[i].destRoom = this.dest_locFormControl.value;
            this.cartDataSource[i].destDepCostCenter = this.dest_depcostFormControl.value;
            this.cartDataSource[i].instDate = this.datePipe.transform(this.req_inst_dateFormControl.value, "yyyy-MM-dd HH:mm:ss");
            //this.cartDataSource[i].instDate = new Date(this.req_inst_dateFormControl.value).toDateString();
            this.cartDataSource[i].comment = this.commentFormControl.value;
             this.cartDataSource[i].destInventoryBuildingID= (this.dest_buildingFormControl.value) ? this.inventoryBuldinglist.find(bldg=>bldg.inventoryBuildingName == this.dest_buildingFormControl.value).inventoryBuildingId : 0;
            this.cartDataSource[i].destInventoryFloorID= (this.dest_floorFormControl.value) ? this.inventoryFloorlist.find(flr=>flr.inventoryFloorName == this.dest_floorFormControl.value).inventoryFloorId : 0;
         
            // this.cartDataSource[i].destBuilding = this.cartDataSource[i].destBuilding;
            // this.cartDataSource[i].destFloor =  this.cartDataSource[i].destFloor;
            // this.cartDataSource[i].destRoom = this.cartDataSource[i].destRoom;
            // this.cartDataSource[i].destDepCostCenter = this.cartDataSource[i].destDepCostCenter;
            // this.cartDataSource[i].inst_date = this.datePipe.transform(this.cartDataSource[i].inst_date, "yyyy-MM-dd HH:mm:ss");
            // //this.cartDataSource[i].inst_date = new Date(this.req_inst_dateFormControl.value).toDateString();
            // this.cartDataSource[i].comment = this.cartDataSource[i].comment;
          }
        }
      }

    this.getCheckedItemList();
  }
    // Get List of Checked Items
    getCheckedItemList(){
      //debugger;
      this.checkedList = [];
      for (var i = 0; i < this.cartDataSource.length; i++) {
        if(this.cartDataSource[i].isSelected)
        {    
          if(this.cartDataSource[i].pullQty == 0){
            this.cartDataSource[i].pullQty = this.cartDataSource[i].qty;
          }
          // if((<HTMLInputElement>document.getElementById("numberFormControl"+i)).value != "")
          // {
          //   //this.cartDataSource[i].pullqty = parseInt((<HTMLInputElement>document.getElementById("numberFormControl"+i)).value);
          //   this.cartDataSource[i].pullqty = this.cartDataSource[i].pullqty;
          // }
          this.checkedList.push(this.cartDataSource[i]);
        }      
      }
      //this.checkedList = JSON.stringify(this.checkedList);
      
      console.log('checked list->',this.checkedList);
    }
  
  // openSendOrderDialog(){

  //   //debugger;
  //   //console.log(inv_item);
  //   this.dialogRef.close();
  //   this.dialog.open(SendorderDialogComponent,{    
  //     // width: '20%',
  //     // height: '65%',    
  //     width:'300px',
  //     panelClass:'send-ord-dialog',
  //     //width: '350px',
  //     //minHeight: 'calc(100vh - 90px)',
  //    // height : '250px',
  //     data:this.cartDataSource
  //   });

   
  // }

  SendOrder(){
debugger;
    if(!this.emailFormControl.value)
     this.emailFormControl.setValue(this.data.email);

     if(!this.requestorFormControl.value)
     this.requestorFormControl.setValue(this.data.username);

    if(this.emailFormControl.value && this.requestorFormControl.value)
    {
      if(this.checkedList)
      {
          this.checkedList.forEach(x =>  {
              x.email = this.emailFormControl.value,
              x.reqName = this.requestorFormControl.value
          });
          this.inventoryOrderService.SaveOrder(this.checkedList).subscribe(
            response => { 
              if(response)
              {
                //debugger;
                alert("Thank you for your order.")
                this.clearCart();
                this.dialogRef.close();
                window.location.reload();
                //this.router.navigate([this.router.url]);
              }
              console.log(response); 
            },
            error => {      
              //debugger;     
              alert(error.message + "\n Unable to create order");
              this.clicked=false;
              console.log(error); 
            });

      }
    }
  

    // var orderitem = new order();
    // orderitem.requestoremail = '',
    // orderitem.request_individual_project = '',
    // orderitem.destination_building = '',
    // orderitem.destination_floor = '',
    // orderitem.destination_location = '',
    // //orderitem.requested_inst_date = this.myDatepipe.transform(this.createOrderForm.value.req_inst_date, 'yyyy-MM-dd'),
    // orderitem.requested_inst_date = '',
    // //this.datePipe.transform(this.myFormControl.value,'yyyy-MM-dd');
    // //orderitem.requested_inst_date = this.createOrderForm.value.req_inst_date,
    // orderitem.comments = 'abc',

    // orderitem.requestoremail = this.createOrderForm.value.emailFormControl,
    // orderitem.request_individual_project = this.createOrderForm.value.requestorFormControl,
    // orderitem.destination_building = this.createOrderForm.value.dest_buildingFormControl,
    // orderitem.destination_floor = this.createOrderForm.value.dest_floorFormControl,
    // orderitem.destination_location = this.createOrderForm.value.dest_locFormControl,
    // //orderitem.requested_inst_date = this.myDatepipe.transform(this.createOrderForm.value.req_inst_date, 'yyyy-MM-dd'),
    // orderitem.requested_inst_date = this.datePipe.transform(this.createOrderForm.value.req_inst_date, "yyyy-MM-dd HH:mm:ss"),
    // //this.datePipe.transform(this.myFormControl.value,'yyyy-MM-dd');
    // //orderitem.requested_inst_date = this.createOrderForm.value.req_inst_date,
    // orderitem.comments = this.createOrderForm.value.commentFormControl,
    //orderitem.cart_item = this.cartDataSource

   // this.inventoryOrderService.SaveOrder(this.cartDataSource).subscribe(
    // this.inventoryOrderService.SaveOrder(orderitem).subscribe(
    //   response => { 
    //     if(response)
    //     {
    //       alert("Thank you for your order.")
    //       this.clearCart();
    //       this.dialogRef.close();
    //       window.location.reload();
    //       //this.router.navigate([this.router.url]);
    //     }
    //     console.log(response); 
    //   },
    //   error => {      
    //     debugger;     
    //     alert(error.message + "\n Unable to create order");
    //     this.clicked=false;
    //     console.log(error); 
    //   });
  }

  clearCart() {
    // this.items.forEach((item, index) => this.cartService.removeItem(index));
    this.cartService.clearCart();
    //this.items = [...this.cartService.getItems()];
  }

  openDialog(inv_item_id:number,condition:string) {
   
    this.dialog.open(ImageCarouselDialogComponent, {    
      width: '100%',
      height: '100%',   
      //panelClass: 'image-dialog',
      data: {id:inv_item_id,cond:condition}
    });
  }

  addDialog(type:string): void {
    //const message = `Are you sure you want to do this?`;

    //const dialogData = type;

    const dialogRef = this.dialog.open(AddDialogComponent, {
      width: '25%',
      height:'25%',
      data: {type:type,clientId:this.data.invitem[0].clientID,userId:this.data.userId}
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      //debugger;
      if(dialogResult){
        if(type=='Building')
        this.inventoryBuldinglist.push(dialogResult);
      else if(type=='Floor')
        this.inventoryFloorlist.push(dialogResult);
      }     
      this.addDialogResult = dialogResult;
    });
  }

  selectionChangedBldg($event:any,i:number){
    debugger;
    if(this.cartDataSource[i].isSelected){
      this.cartDataSource[i].destBuilding = $event.value;
      let index:any;
      index = this.checkedList.findIndex(_item => _item.inventoryID === this.cartDataSource[i].inventoryID && _item.inventoryItemID === this.cartDataSource[i].inventoryItemID && _item.clientID === this.cartDataSource[i].clientID);
          
        if (index > -1) {
            this.checkedList[index].destBuilding = $event.value;
            this.checkedList[index].destInventoryBuildingID = this.inventoryBuldinglist.find(bldg => bldg.inventoryBuildingName == $event.value).inventoryBuildingId ;
            
        }
    }    
  }

  selectionChangedFlr($event:any,i:number){
    debugger;
    if(this.cartDataSource[i].isSelected){
      this.cartDataSource[i].destFloor = $event.value;
      let index:any;
      index = this.checkedList.findIndex(_item => _item.inventoryID === this.cartDataSource[i].inventoryID && _item.inventoryItemID === this.cartDataSource[i].inventoryItemID && _item.clientID === this.cartDataSource[i].clientID);
          
        if (index > -1) {
            this.checkedList[index].destFloor = $event.value;
            this.checkedList[index].destInventoryFloorID = this.inventoryFloorlist.find(bldg => bldg.inventoryFloorName == $event.value).inventoryFloorId ;

        }
    }
  }

  changedRoom($event:any,i:number){
    //debugger;
    if(this.cartDataSource[i].isSelected){
      this.cartDataSource[i].destRoom = $event.target.value;
      let index:any;
      index = this.checkedList.findIndex(_item => _item.inventoryID === this.cartDataSource[i].inventoryID && _item.inventoryItemID === this.cartDataSource[i].inventoryItemID && _item.clientID === this.cartDataSource[i].clientID);
          
        if (index > -1) {
            this.checkedList[index].destRoom = $event.target.value;
        }
    }
  }

  changedDeptCost($event:any,i:number){
    //debugger;
    if(this.cartDataSource[i].isSelected){
      this.cartDataSource[i].destDepCostCenter = $event.target.value;
      let index:any;
      index = this.checkedList.findIndex(_item => _item.inventoryID === this.cartDataSource[i].inventoryID && _item.inventoryItemID === this.cartDataSource[i].inventoryItemID && _item.clientID === this.cartDataSource[i].clientID);
          
        if (index > -1) {
            this.checkedList[index].destDepCostCenter = $event.target.value;
        }
    }
  }

  changedInstDate($event:any,i:number){
    //debugger;
    if(this.cartDataSource[i].isSelected){
      this.cartDataSource[i].instDate = this.datePipe.transform($event.value,"yyyy-MM-dd HH:mm:ss");
      let index:any;
      index = this.checkedList.findIndex(_item => _item.inventoryID === this.cartDataSource[i].inventoryID && _item.inventoryItemID === this.cartDataSource[i].inventoryItemID && _item.clientID === this.cartDataSource[i].clientID);
          
        if (index > -1) {
            this.checkedList[index].instDate = this.datePipe.transform($event.value,"yyyy-MM-dd HH:mm:ss");
        }
    }document.getElementsByClassName
  }

  changedComment($event:any,i:number){
    //debugger;
    if(this.cartDataSource[i].isSelected){
      //this.cartDataSource[i].comment = (<HTMLInputElement>document.getElementById("commentFormControl"+i)).value;
    this.cartDataSource[i].comment = $event.target.value;
    let index:any;
    index = this.checkedList.findIndex(_item => _item.inventoryID === this.cartDataSource[i].inventoryID && _item.inventoryItemID === this.cartDataSource[i].inventoryItemID && _item.clientID === this.cartDataSource[i].clientID);
        
      if (index > -1) {
          this.checkedList[index].comment = $event.target.value;
      }
      }
  }
}
