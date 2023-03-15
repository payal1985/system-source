import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { addcart } from 'src/app/models/addcart';

import { inventoryitemdetail } from 'src/app/models/inventoryitemdetail';
import { AddCartService } from 'src/app/services/addcart.service';

import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { WarrantyRequestDialogComponent } from '../warranty-request-dialog/warranty-request-dialog.component';


@Component({
  selector: 'app-add-cart-dialog',
  templateUrl: './add-cart-dialog.component.html',
  styleUrls: ['./add-cart-dialog.component.scss']
})
export class AddCartDialogComponent implements OnInit {

  dataSource: addcart[];  
  firstError :string;
  cartDataItem: inventoryitemdetail;
  masterSelected:boolean;
  checkedList:addcart[];

  displayedColumns: string[] = ['image','building', 'floor', 'room','condition', 'qty','pullqty','select'];
  numberFormControl = new FormControl('', [
    Validators.required,Validators.pattern('[0-9]+')
    // ,
    // Validators.max(this.maxQty+1)
  ]
);


//selection = new SelectionModel<inventoryitem>(true, []);

  constructor(public dialogRef: MatDialogRef<AddCartDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Array<addcart>,private cartService: AddCartService
    ,private inventoryItemService: InventoryItemService
    ,private dialog: MatDialog
    ) {
     // debugger;
     // console.log('data', this.data);
     
     }

  ngOnInit(): void {
//debugger;
this.dataSource = this.data;
//this.maxQty = this.data[0].qty;
console.log(this.dataSource);
  }

  QtyChange(index:number,pullqty:string){
    //debugger;
    this.dataSource[index].pullQty = parseInt(pullqty);
  }

  addtocart()  {
    if(this.checkedList)
    {
      for (var i = 0; i < this.checkedList.length; i++) {
        if(this.checkedList[i].pullQty < 0)
        {
          alert("Quantity must be a positive number");
        }
        if(this.checkedList[i].pullQty == 0)
        {
          this.cartService.removeItem(this.checkedList[i],i);
        }
        else if(this.checkedList[i].pullQty > this.checkedList[i].qty)
        {
          alert("Sorry, only " + this.checkedList[i].qty + " are available in row " + (i+1));         
        }    
        else{
          //debugger;
          this.cartService.addtoCart(this.checkedList[i],this.checkedList[i].pullQty);
          this.dialogRef.close();
        }
      }
    }
  
  }
// The master checkbox will check/ uncheck all items
checkUncheckAll() {
  //debugger;
  for (var i = 0; i < this.dataSource.length; i++) {
    this.dataSource[i].isSelected = this.masterSelected;
    (<HTMLInputElement>document.getElementById("numberFormControl"+i)).value = this.dataSource[i].qty.toString();
    
  }
  // if(this.masterSelected)
  // {    
  // }

  this.getCheckedItemList();
}

// Check All Checkbox Checked
isAllSelected() {
  debugger;
  this.masterSelected = this.dataSource.every(function(item:any) {
   // debugger;
      return item.isSelected == true;
    })

    if(!this.masterSelected)
    {
      for (var i = 0; i < this.dataSource.length; i++) {
        if(!this.dataSource[i].isSelected)
        {
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
        this.dataSource[i].pullQty = parseInt((<HTMLInputElement>document.getElementById("numberFormControl"+i)).value);
      }
      else {
        (<HTMLInputElement>document.getElementById("numberFormControl"+i)).value = this.dataSource[i].qty.toString();
        this.dataSource[i].pullQty = this.dataSource[i].qty;
      }
      this.checkedList.push(this.dataSource[i]);
    }      
  }
  //this.checkedList = JSON.stringify(this.checkedList);
  
  //console.log('checked list->',this.checkedList);
}

openWarrentyRequestDialog(){
   //debugger;  
   this.dialogRef.close();
   this.dialog.open(WarrantyRequestDialogComponent,{ 
    panelClass: 'warranty-request-dialog',
    data: this.checkedList
   });
}

}
