import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatTableDataSource, } from '@angular/material/table';
import { OrderCartService } from 'src/app/services/ordercart.service';
import {FormControl, Validators} from '@angular/forms';
import { inventoryitemdetail } from 'src/app/model/inventoryitemdetail';
//import { cart } from 'src/app/model/cart';
import { inventoryitem } from 'src/app/model/inventoryitem';
//import { MustMatch } from 'src/app/shared/must-match-validator';
import {SelectionModel} from '@angular/cdk/collections';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { ordercart } from 'src/app/model/ordercart';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-order-cart-dialog',
  templateUrl: './order-cart-dialog.component.html',
  styleUrls: ['./order-cart-dialog.component.css']
})
export class OrderCartDialogComponent implements OnInit {
  dataSource: ordercart[];

  inventoryBuldinglist!: string[];
  inventoryFloorlist!: string[];
  firstError :string;

  cartDataItem: inventoryitemdetail;
  orderCartItemList = new ordercart();
  ordCart: ordercart[];
  
  bldgSelected:string="";
  flrSelected:string;
  commentEntered:string;
  reqDateEntered:Date;
  locEntered:string;


  masterSelected:boolean;
 // checklist:any;
  checkedList:ordercart[];
  
  // showOtherBldg = false;
  // showOtherFlr = false;



  //displayedColumns: string[] = ['building', 'floor', 'mploc', 'qty','star','email','requestor'];
  displayedColumns: string[] = ['building', 'floor', 'mploc','condition', 'qty','pullqty','select','dest_bldg','dest_flr','dest_loc','req_inst_date','comment'];
  numberFormControl = new FormControl('', [
    Validators.required,Validators.pattern('[0-9]+')
    // ,
    // Validators.max(this.maxQty+1)
  ]
);

emailFormControl = new FormControl('', [Validators.required,Validators.email]);
requestorFormControl = new FormControl('', [Validators.required]);
dest_buildingFormControl = new FormControl('', [Validators.required]);
dest_floorFormControl = new FormControl('', [Validators.required]);
dest_locFormControl = new FormControl('', [Validators.required]);
req_inst_date  = new FormControl('', [Validators.required]);
commentFormControl = new FormControl('')
//otherBldgFormControl = new FormControl('');
//otherFlrFormControl = new FormControl('');


//selection = new SelectionModel<inventoryitem>(true, []);
selection = new SelectionModel<ordercart>(true, []);

  constructor(public dialogRef: MatDialogRef<OrderCartDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,private cartService: OrderCartService
    ,private inventoryItemService: InventoryItemService
    ,private datePipe: DatePipe
    ) {
     // debugger;
      console.log('data', this.data);

      this.inventoryItemService.getBuildings(this.data.invitem[0].client_id).subscribe(data => {
        this.inventoryBuldinglist = data;
      });
      this.inventoryItemService.getFloor(this.data.invitem[0].client_id).subscribe(data => {
        this.inventoryFloorlist = data;
      });
     }

  ngOnInit(): void {
//debugger;
// this.data.forEach(a=>{
//   debugger;
// var ordcart = new ordercart();
// ordcart.inv_item_id = a.inv_item_id;
// this.dataSource.push(ordcart);
// })
this.dataSource = [];
this.data.invitem.forEach(arr=>{
  var ordlist = new ordercart();
  
    ordlist.inv_item_id = arr.inv_item_id;
    ordlist.building = arr.building;
    ordlist.floor = arr.floor;
    ordlist.mploc = arr.mploc;
    ordlist.cond = arr.cond;
    ordlist.qty = arr.qty;
    ordlist.inventory_id = arr.inventory_id;
    ordlist.inv_image_name = arr.inv_image_name;    
    ordlist.inv_image_url = arr.inv_image_url;    
    ordlist.item_code = arr.item_code;
    ordlist.description = arr.description;  
    ordlist.client_id = arr.client_id; 
    ordlist.username = this.data.username;
    ordlist.email = this.data.email;

    this.dataSource.push(ordlist);
});
this.getCheckedItemList();

//this.dataSource = this.data;
//this.maxQty = this.data[0].qty;
console.log(this.dataSource);
this.ordCart = [];
  }

  addtocart()
  {
    //debugger;
  
    if(this.checkedList)
    {
      for (var i = 0; i < this.checkedList.length; i++) {
        if(this.checkedList[i].pullqty < 0)
        {
          alert("Quantity must be a positive number");
        }
        if(this.checkedList[i].pullqty == 0)
        {
          this.cartService.removeItem(this.checkedList[i],i);
        }
        else if(this.checkedList[i].pullqty > this.checkedList[i].qty)
        {
          alert("Sorry, only " + this.checkedList[i].qty + " are available in row " + (i+1));         
        }    
        else{
          this.cartService.addtoCart(this.checkedList[i],this.checkedList[i].pullqty);
        }
      }
    }
  }

//Old add-to cart method to add rowise into cart
  // addtocart(item:any,qty:string,i:number)  {    
  //  debugger;    
  //   if(parseInt(qty) < 0)
  //   {
  //     alert("Quantity must be a positive number");
  //   }
  //   if(parseInt(qty) == 0)
  //   {
  //     this.cartService.removeItem(item,i);
  //   }
  //   else if(parseInt(qty) > parseInt(item.qty))
  //   {
  //     alert("Sorry, only " + parseInt(item.qty) + " are available");
  //     //event.stopPropagation();
	// 		//obj.value = obj.defaultValue;
  //   }    
  //   else{
  //     this.cartService.addtoCart(item,parseInt(qty));

  //   }
  // }

  // The master checkbox will check/ uncheck all items
  checkUncheckAll() {
    //debugger;
    for (var i = 0; i < this.dataSource.length; i++) {
      this.dataSource[i].isSelected = this.masterSelected;
      (<HTMLInputElement>document.getElementById("numberFormControl"+i)).value = this.dataSource[i].qty.toString();
      // this.dataSource[i].destbuilding = (this.showOtherBldg && this.otherBldgFormControl.value != "") ? this.otherBldgFormControl.value : this.dest_buildingFormControl.value;
      // this.dataSource[i].destfloor = (this.showOtherFlr && this.otherFlrFormControl.value != null) ?this.otherFlrFormControl.value : this.dest_floorFormControl.value;
      this.dataSource[i].destbuilding = this.dest_buildingFormControl.value;
      this.dataSource[i].destfloor =  this.dest_floorFormControl.value;
      this.dataSource[i].destroom = this.dest_locFormControl.value;
      this.dataSource[i].inst_date = this.datePipe.transform(this.req_inst_date.value, "yyyy-MM-dd HH:mm:ss");
      this.dataSource[i].comment = this.commentFormControl.value;
      
    }
    if(this.masterSelected)
    {
      this.dest_locFormControl.setValue(this.dest_locFormControl.value);
      this.req_inst_date.setValue(this.req_inst_date.value);
      this.commentFormControl.setValue(this.commentFormControl.value);
      // if(this.showOtherBldg && this.otherBldgFormControl.value != "")
      // {
      //   this.dest_buildingFormControl.setValue('');
      //   this.otherBldgFormControl.setValue(this.otherBldgFormControl.value);
      // }
      // else
      // {
        this.dest_buildingFormControl.setValue(this.dest_buildingFormControl.value);
      // }
      // if(this.showOtherFlr && this.otherFlrFormControl.value != null)
      // {
      //   this.dest_floorFormControl.setValue('');
      //   this.otherFlrFormControl.setValue(this.otherFlrFormControl.value);
      // }
      // else
      // {
        this.dest_floorFormControl.setValue(this.dest_floorFormControl.value);
      //}
    }

    this.getCheckedItemList();
  }

  // Check All Checkbox Checked
  isAllSelected() {
    //debugger;
    this.masterSelected = this.dataSource.every(function(item:any) {
     // debugger;
        return item.isSelected == true;
      })

      if(!this.masterSelected)
      {
        for (var i = 0; i < this.dataSource.length; i++) {
          if(!this.dataSource[i].isSelected)
          {
            //let val = (<HTMLInputElement>document.getElementsById("dest_buildingFormControl"+i)).t;           
            //this.productForm.controls['category'].setValue(this.product.category.id);         
           // this.dest_buildingFormControl.setValue("Dest Building");
            
           // (<HTMLInputElement>document.getElementById("d_bldg"+i)).value = "Dest Floor";
            (<HTMLInputElement>document.getElementById("dest_floorFormControl"+i)).value = "Dest Floor";
           // var loc = (<HTMLInputElement>document.getElementById("dest_locFormControl"+i)).value;
            (<HTMLInputElement>document.getElementById("dest_locFormControl"+i)).value = "";
            (<HTMLInputElement>document.getElementById("req_inst_date"+i)).value = "";
            (<HTMLInputElement>document.getElementById("commentFormControl"+i)).value = "";
            (<HTMLInputElement>document.getElementById("numberFormControl"+i)).value = "";
          }
        }
      }

    this.getCheckedItemList();
  }

  // Get List of Checked Items
  getCheckedItemList(){
    //debugger;
    this.checkedList = [];
    for (var i = 0; i < this.dataSource.length; i++) {
      if(this.dataSource[i].isSelected)
      {    
        if((<HTMLInputElement>document.getElementById("numberFormControl"+i)).value != "")
        {
          this.dataSource[i].pullqty = parseInt((<HTMLInputElement>document.getElementById("numberFormControl"+i)).value);
        }
        this.checkedList.push(this.dataSource[i]);
      }      
    }
    //this.checkedList = JSON.stringify(this.checkedList);
    
    console.log('checked list->',this.checkedList);
  }

  // addbldg(index:number)
  // {
  //   //alert('hello');
  //   //this.showOtherBldg = true;
  // }

  // addFlr(index:number)
  // {
  //   //alert('hello');
  // }
    // /** Whether the number of selected elements matches the total number of rows. */
    // isAllSelected() {
    //   debugger;
    //   const numSelected = this.selection.selected.length;
    //   const numRows = this.dataSource.length;
    //   return numSelected === numRows;
    // }
  
    // /** Selects all rows if they are not all selected; otherwise clear selection. */
    // masterToggle() {
    //   debugger;
    //   if (this.isSomeSelected()) {
    //     debugger;
    //     this.selection.clear();
    //   } else {
    //       if(this.isAllSelected())
    //       {
    //         debugger;
    //         this.selection.clear();
    //       } 
    //       else
    //       {
    //         this.dataSource.forEach(row => 
    //           {
    //             debugger;
    //             this.selection.select(row)
    //             // this.orderCartItemList.inv_item_id = row.inv_item_id;
    //             // this.orderCartItemList.building = row.building;
    //             // this.orderCartItemList.floor = row.floor;
    //             // this.orderCartItemList.mploc = row.mploc;
    //             // this.orderCartItemList.cond = row.cond;
    //             // this.orderCartItemList.qty = row.qty;
    //             // this.orderCartItemList.inventory_id = row.inventory_id;
    //             // this.orderCartItemList.inv_image_name = row.inv_image_name;
    //             // this.orderCartItemList.inv_image_url = row.inv_image_url;
    //             // this.orderCartItemList.item_code = row.item_code;
    //             // this.orderCartItemList.description = row.description;
    //             // this.orderCartItemList.client_id = row.client_id;
    //             // this.orderCartItemList.pullqty = row.qty;
    //             // this.orderCartItemList.isSelected = true;
    //             // this.orderCartItemList.destbuilding = this.dest_buildingFormControl.value;
    //             // this.orderCartItemList.destfloor = this.dest_floorFormControl.value;
    // // row.destbuilding = this.dest_buildingFormControl.value;
    // // row.destfloor = this.dest_floorFormControl.value;
    // this.dest_buildingFormControl.setValue(this.dest_buildingFormControl.value);
    // this.dest_floorFormControl.setValue(this.dest_floorFormControl.value);
    // this.dest_locFormControl.setValue(this.dest_locFormControl.value);
    // this.req_inst_date.setValue(this.req_inst_date.value);
    // this.commentFormControl.setValue(this.commentFormControl.value);
    //         debugger;
    //             // this.ordCart.push(this.orderCartItemList);
    //             // // console.log(this.dest_buildingFormControl.value)
    //             // // console.log(this.dest_floorFormControl.value)
    //             // // console.log(this.dest_locFormControl.value)
    //             // // console.log(this.req_inst_date.value)
    //             // // console.log(this.commentFormControl.value)
    //             //  console.log(row)
                
    //           });
    //         // console.log(this.ordCart);
    //           //this.dataSource = this.ordCart;
    //       }
        
    //       }
    // }

    // isSomeSelected() {
    //   debugger;
    //   console.log(this.selection.selected);
    //   return this.selection.selected.length > 0;
    // }
    // edit(event, element) {
    //   debugger;
    //   this.selection.clear();
    //   this.selection.changed.subscribe(row => 
    //     { 
    //       this.dest_buildingFormControl.setValue("");
    //       //this.isAddDaysEnabled = this.selection.selected.length == 0; 
    //     });
    // }
}
