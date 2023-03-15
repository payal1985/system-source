import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatTableDataSource, } from '@angular/material/table';
import { CartService } from 'src/app/services/cart.service';
import {FormControl, Validators} from '@angular/forms';
import { inventoryitemdetail } from 'src/app/model/inventoryitemdetail';
//import { cart } from 'src/app/model/cart';
import { inventoryitem } from 'src/app/model/inventoryitem';
//import { MustMatch } from 'src/app/shared/must-match-validator';
import {SelectionModel} from '@angular/cdk/collections';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';

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
  //maxQty:number=0;
  //disabled = false;
  //myArray : inventoryitem[];
  //firstError :string;

  inventoryBuldinglist!: string[];
  inventoryFloorlist!: string[];
  firstError :string;

  cartDataItem: inventoryitemdetail;

  displayedColumns: string[] = ['building', 'floor', 'mploc', 'qty','star'];
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

selection = new SelectionModel<inventoryitem>(true, []);

  constructor(public dialogRef: MatDialogRef<AddCartDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Array<inventoryitem>,private cartService: CartService
    ,private inventoryItemService: InventoryItemService
    ) {
     // debugger;
      console.log('data', this.data);
      this.inventoryItemService.getBuildings(this.data[0].client_id).subscribe(data => {
        this.inventoryBuldinglist = data;
      });
      this.inventoryItemService.getFloor(this.data[0].client_id).subscribe(data => {
        this.inventoryFloorlist = data;
      });
     }

  ngOnInit(): void {
//debugger;
this.dataSource = this.data;
//this.maxQty = this.data[0].qty;
console.log(this.dataSource);
  }



  addtocart(item:any,qty:string,i:number)  {
   // debugger;    
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

    /** Whether the number of selected elements matches the total number of rows. */
    isAllSelected() {
      //debugger;
      const numSelected = this.selection.selected.length;
      const numRows = this.dataSource.length;
      return numSelected === numRows;
    }
  
    /** Selects all rows if they are not all selected; otherwise clear selection. */
    masterToggle() {
      //debugger;
      this.isAllSelected() ?
          this.selection.clear() :
          this.dataSource.forEach(row => 
            {
              this.selection.select(row)
              console.log(row)
            });
    }
}

