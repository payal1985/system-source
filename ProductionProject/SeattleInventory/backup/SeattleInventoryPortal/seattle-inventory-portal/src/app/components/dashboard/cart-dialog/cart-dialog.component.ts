import { Component, OnInit,Inject,ViewChild,AfterViewInit, ChangeDetectorRef  } from '@angular/core';
import { MatDialog,MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { MatTable, MatTableDataSource, } from '@angular/material/table';
import {FormControl, Validators} from '@angular/forms';
import { CartService } from 'src/app/services/cart.service';
import { SendorderDialogComponent } from '../sendorder-dialog/sendorder-dialog.component';
import { cart } from 'src/app/model/cart';

@Component({
  selector: 'app-cart-dialog',
  templateUrl: './cart-dialog.component.html',
  styleUrls: ['./cart-dialog.component.css']
})
export class CartDialogComponent implements OnInit,AfterViewInit {

  cartDataSource:cart[];
  //items = [];
 // emptyData = new MatTableDataSource([{ empty: "row" }]);
  displayedColumns: string[] = ['inv_image_name','item_code', 'description', 'Pull Info','pullqty','remove'];
  //displayedColumns: string[] = ['building', 'floor', 'mploc', 'qty','star'];
  qtyFormControl = new FormControl('', []);
  @ViewChild(MatTable, {static: false}) table: MatTable<any>;
  
  constructor(public dialogRef: MatDialogRef<CartDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,private cartService: CartService
    ,public dialog: MatDialog
    ,private cdr: ChangeDetectorRef
    ) { 
      debugger;
      console.log('cart data', this.data);
    }

  ngOnInit(): void {
debugger;
this.cartDataSource = this.data;

  // this.cartDataSource = this.cartDataSource
  //     //.filter(i => i !== this.data.)
  //     .map((i, idx) => (i.inv_item_id = (idx + 1), i));
    //  this.cartDataSource.push();

  //console.log(this.cartDataSource);
  //debugger;
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
//   cartdata.pullqty = element.pullqty;
//   this.cartDataSource.push(cartdata);    
// });
   
debugger;
    console.log('cart data Source data', this.cartDataSource);
   

  }

  ngAfterViewInit() {
    debugger;
    this.cdr.detectChanges();
  }

  pullqtychange(item){
    debugger;
    if(item.pullqty > item.qty)
    {
      alert("There are not enough items availalbe");
    }
   // console.log('new item',item);
  }

  //removefromcart()  {
  removeFromCart(item:any,i:any)  {
    debugger;
    //this.cartService.removeCartItem(item);
    this.cartService.removeItem(item,i);
    //this.cartDataSource = this.cartService.getItems();
    this.cartDataSource = this.cartDataSource
      .filter(i => i !== item)
      .map((i, idx) => (i.inv_item_id = (idx + 1), i));
   // this.items = this.cartService.getItems();
debugger;

//this.setInventoryItemList();
    // const product = this.data.find(p => {
    //   return p.inventory_id === item.inventory_id && p.inv_item_id == item.inv_item_id;
    // });
    // if(product){
    //   //this.data.splice(this.data.map(p => p.inv_item_id).indexOf(item.inv_item_id), 1);
    //   this.cartDataSource.data.splice(i,1) 
    //   debugger
    //   console.log(this.cartDataSource);
      //this.table.renderRows();
   // }


  }

  // setInventoryItemList()
  // {
  //   this.cartService.getProducts()
  //   .subscribe(res=>{
  //     debugger;
  //     this.cartDataSource = res;
  //     //this.totalItem = res.length;
  //     console.log(this.cartDataSource);
  //   })
  // }

  openSendOrderDialog(){

    // const dialogConfig = new MatDialogConfig();

    // dialogConfig.disableClose = true;
    // dialogConfig.autoFocus = true;

    // dialogConfig.data = {
    //     cartStorageItem: this.cartDataSource
        
    // };

    debugger;
    //console.log(inv_item);
    this.dialogRef.close();
    this.dialog.open(SendorderDialogComponent,{    
      width: '350px',
      minHeight: 'calc(100vh - 90px)',
      height : 'auto',
      data:this.cartDataSource
    });

   
  }
}
