import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { cart } from '../models/cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  public cartItemList : any =[]
  public inventoryList = new BehaviorSubject<cart[]>([]);
  //public search = new BehaviorSubject<string>("");
  items:cart[];
  // public invItems = new BehaviorSubject<any>([]);

  constructor() { }
  getProducts(client_id:number){
   // debugger;
   // return this.items;;
   this.getItems();
   this.inventoryList.next(this.items);
    return this.inventoryList.asObservable();
  }

  setProduct(product : any){
    //debugger;
    this.cartItemList.push(...product);
    this.inventoryList.next(product);
  }
  
  addtoCart(addedItem : cart,qty: number){
    //debugger;
    // addedItem.pullqty += qty;
    // this.items.push(addedItem);
    // this.inventoryList.next(this.items);

    const i = this.items.findIndex(_item => _item.inventory_id === addedItem.inventory_id && _item.inv_item_id == addedItem.inv_item_id && _item.client_id == addedItem.client_id);
    if (i > -1) {
      if(this.items[i].pullqty < this.items[i].qty)
      {
        var newqty = this.items[i].pullqty += qty;
        if(newqty > this.items[i].qty){          
          this.items[i].pullqty = this.items[i].qty;
        }
      }
      // else if(this.items.findIndex(i=>i.inv_item_id != addedItem.inv_item_id))
      // {
      //   addedItem.pullqty = qty;
      //   this.items.push(addedItem);
      // }
      //this.items[i].pullqty += qty; // (2)
      console.log(this.items[i]);
    }
    else     {
      addedItem.pullqty = qty;
    this.items.push(addedItem);
    }
     this.inventoryList.next(this.items);
    
    this.saveCart();
    // const i = this.cartItemList.findIndex(_item => _item.inventory_id === product.inventory_id && _item.inv_item_id == product.inv_item_id);
    // if (i > -1) {
    //   this.cartItemList[i].pullqty = this.cartItemList[i].pullqty + qty; // (2)
    //   console.log(this.cartItemList[i]);
    // }
    // else     {
    // product.pullqty = qty;
    // this.cartItemList.push(product);
    // }
    // //console.log('not exists');
    // //array.push(item);

    // // product.pullqty = qty;
    // // this.cartItemList.push(product);

    // // this.cartItemList.array.forEach(element => {
    // //   Object.assign(product,{pullqty:qty})
    // // });
    // this.inventoryList.next(this.cartItemList);
    // this.getTotalPrice();
    // console.log(this.cartItemList)
  }

//  itemInCart(item): boolean {
//     return this.items.findIndex(o => o.id === item.id) > -1;
//   }

  saveCart() {
   // debugger;
    localStorage.setItem('cart_items', JSON.stringify(this.items)); 
  }

  getItems() {   
    //debugger;
    //return this.items.filter(x=>x.client_id == client_id);
    return this.items;
  } 

  loadCart(client_id:number): void {
   // debugger;
    this.items = JSON.parse(localStorage.getItem("cart_items")) ?? [];  
    this.items = this.items.filter(x=>x.client_id == client_id);
  }

  getTotalPrice() : number{
    let grandTotal = 0;
    this.cartItemList.map((a:any)=>{
      grandTotal += a.total;
    })
    return grandTotal;
  }
  // removeCartItem(product: any){
  //   this.cartItemList.map((a:any, index:any)=>{
  //     if(product.inv_item_id === a.inv_item_id && product.inventory_id === a.inventory_id){
  //       this.cartItemList.splice(index,1);
  //     }
  //   })
  //   this.inventoryList.next(this.cartItemList);
  // }

  removeItem(item,i:number) {
   // debugger;
    const index = this.items.findIndex(o => o.inv_item_id === item.inv_item_id 
                                      && o.inventory_id === item.inventory_id);

    if (index > -1) {
      this.items.splice(index, 1);
    
    }
    //this.items.splice(i,1);
    this.inventoryList.next(this.items);
    this.saveCart();
  }

  clearCart() {
    this.items = [];
    localStorage.removeItem("cart_items");
    this.inventoryList.next(this.items);
    //this.saveCart();
  }
  
  // removeAllCart(){
  //   this.cartItemList = []
  //   this.inventoryList.next(this.cartItemList);
  // }
}