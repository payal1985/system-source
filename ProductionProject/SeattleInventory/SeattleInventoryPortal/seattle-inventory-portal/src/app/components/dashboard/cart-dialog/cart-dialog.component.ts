import { Component, OnInit,Inject,ViewChild,AfterViewInit, ChangeDetectorRef  } from '@angular/core';
import { MatDialog,MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatTable, MatTableDataSource, } from '@angular/material/table';
import {FormControl, Validators} from '@angular/forms';
//import { CartService } from 'src/app/services/cart.service';
import { OrderCartService } from 'src/app/services/ordercart.service';
import { SendorderDialogComponent } from '../sendorder-dialog/sendorder-dialog.component';
import { cart } from 'src/app/model/cart';
import { ordercart } from 'src/app/model/ordercart';
import { InventoryorderService } from 'src/app/services/inventoryorder.service';

@Component({
  selector: 'app-cart-dialog',
  templateUrl: './cart-dialog.component.html',
  styleUrls: ['./cart-dialog.component.css']
})
export class CartDialogComponent implements OnInit,AfterViewInit {

 // cartDataSource:cart[]; //old flow code support line
  cartDataSource:ordercart[];
  //items = [];
 // emptyData = new MatTableDataSource([{ empty: "row" }]);
  displayedColumns: string[] = ['inv_image_name','item_code', 'description', 'Pull Info','cond','pullqty','remove'];
  //displayedColumns: string[] = ['building', 'floor', 'mploc', 'qty','star'];
  qtyFormControl = new FormControl('', []);
  @ViewChild(MatTable, {static: false}) table: MatTable<any>;
  clicked = false;
  
  constructor(public dialogRef: MatDialogRef<CartDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,private cartService: OrderCartService
    ,public dialog: MatDialog
    ,private cdr: ChangeDetectorRef
    ,private inventoryOrderService: InventoryorderService
    ) { 
     // debugger;
      console.log('cart data', this.data);
    }

  ngOnInit(): void {
//debugger;
this.cartDataSource = this.data;
   
//debugger;
    console.log('cart data Source data', this.cartDataSource);  

  }

  ngAfterViewInit() {
    //debugger;
    this.cdr.detectChanges();
  }

  pullqtychange(item){
    //debugger;
    if(item.pullqty > item.qty)
    {
      alert("There are not enough items availalbe");
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
      .map((i, idx) => (i.inv_item_id = (idx + 1), i));
   // this.items = this.cartService.getItems();
//debugger;


  }



  openSendOrderDialog(){

    //debugger;
    //console.log(inv_item);
    this.dialogRef.close();
    this.dialog.open(SendorderDialogComponent,{    
      // width: '20%',
      // height: '65%',    
      width:'300px',
      panelClass:'send-ord-dialog',
      //width: '350px',
      //minHeight: 'calc(100vh - 90px)',
     // height : '250px',
      data:this.cartDataSource
    });

   
  }

  SendOrder(){

    this.inventoryOrderService.SaveOrder(this.cartDataSource).subscribe(
      response => { 
        if(response)
        {
          alert("Thank you for your order.")
          this.clearCart();
          this.dialogRef.close();
          window.location.reload();
          //this.router.navigate([this.router.url]);
        }
        console.log(response); 
      },
      error => {      
        debugger;     
        alert(error.message + "\n Unable to create order");
        this.clicked=false;
        console.log(error); 
      });
  }

  clearCart() {
    // this.items.forEach((item, index) => this.cartService.removeItem(index));
    this.cartService.clearCart();
    //this.items = [...this.cartService.getItems()];
  }

}
