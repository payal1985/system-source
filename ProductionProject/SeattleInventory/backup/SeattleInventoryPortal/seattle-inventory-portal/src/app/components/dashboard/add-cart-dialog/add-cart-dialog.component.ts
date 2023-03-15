import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatTableDataSource, } from '@angular/material/table';
import { CartService } from 'src/app/services/cart.service';
import {FormControl, Validators} from '@angular/forms';
import { inventoryitemdetail } from 'src/app/model/inventoryitemdetail';
//import { cart } from 'src/app/model/cart';
import { inventoryitem } from 'src/app/model/inventoryitem';
//import { MustMatch } from 'src/app/shared/must-match-validator';

@Component({
  selector: 'app-add-cart-dialog',
  templateUrl: './add-cart-dialog.component.html',
  styleUrls: ['./add-cart-dialog.component.css']
})
export class AddCartDialogComponent implements OnInit {
  //dataSource:MatTableDataSource<any>;
 // dataSource: cart[];
  //dataSource: [];
  dataSource: inventoryitem[];
  maxQty:number=0;
  //disabled = false;
  //myArray : inventoryitem[];
  //firstError :string;

  cartDataItem: inventoryitemdetail;

  displayedColumns: string[] = ['building', 'floor', 'mploc', 'qty','star'];
  numberFormControl = new FormControl('', [
    Validators.required,Validators.pattern('[0-9]+')
    // ,
    // Validators.max(this.maxQty+1)
  ]
);

  constructor(public dialogRef: MatDialogRef<AddCartDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Array<inventoryitem>,private cartService: CartService) {
      debugger;
      console.log('data', this.data);
     }

  ngOnInit(): void {
debugger;
this.dataSource = this.data;
this.maxQty = this.data[0].qty;
console.log(this.dataSource);

// this.data.forEach(element => {
// let commentData : inventoryitem = {
//   inv_item_id: element.inv_item_id,
//   building:element.building,
//   floor: element.floor,
//   mploc: element.mploc,
//   cond: element.cond,qty :element.qty,inventory_id:element.inventory_id,
//   inv_image_name: element.inv_image_name,item_code:element.item_code,description:element.description
// }

// debugger;
// console.log(commentData);
// this.myArray.push(commentData);
// this.dataSource.push(commentData);
// });

// this.data.forEach(element => {
//   var cartdata = new cart();
//   cartdata.inv_item_id = element.inv_item_id;
//   cartdata.building = element.building;
//   cartdata.floor = element.floor;
//   cartdata.mploc = element.mploc;
//   cartdata.cond = element.cond;
//   cartdata.qty = element.qty;
//   cartdata.inventory_id = element.inventory_id;
//   cartdata.inv_image_name = element.inv_image_name;    
//   cartdata.item_code = element.item_code;
//   cartdata.description = element.description;
//   console.log(cartdata);
// debugger;

  // this.dataSource.push(cartdata);  
  //this.dataSource = cartdata;  
//});

   //console.log('Pull Details',this.myArray);
    //console.log('Pull Details',this.dataSource);
  }



  addtocart(item:any,qty:string,i:number)  {
    debugger;    
    if(parseInt(qty) < 0)
    {
      alert("Quantity must be a positive number");
    }
    if(parseInt(qty) == 0)
    {
      this.cartService.removeItem(item,i);
    }
    else if(parseInt(qty) > parseInt(item.qty))
    {
      alert("Sorry, only " + parseInt(item.qty) + " are available");
      //event.stopPropagation();
			//obj.value = obj.defaultValue;
    }    
    else{
      this.cartService.addtoCart(item,parseInt(qty));

    }
  }
}

