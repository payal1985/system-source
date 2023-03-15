import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { addcart } from '../models/addcart';

@Injectable({
  providedIn: 'root'
})
export class AddCartService {

  public cartItemList : any =[]
  public inventoryList = new BehaviorSubject<addcart[]>([]);
  //public search = new BehaviorSubject<string>("");
  items:addcart[];
  // public invItems = new BehaviorSubject<any>([]);

  constructor() { this.items = [];}
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
  
  addtoCart(addedItem : addcart,qty: number){
    //debugger;
    
    let i:any;
    if(this.items)
        i = this.items.findIndex(_item => _item.inventoryID === addedItem.inventoryID && _item.inventoryItemID === addedItem.inventoryItemID && _item.clientID === addedItem.clientID);
        
    if (i > -1) {
      if(this.items[i].pullQty < this.items[i].qty)
      {
        var newqty = this.items[i].pullQty += qty;
        if(newqty > this.items[i].qty){          
          this.items[i].pullQty = this.items[i].qty;
        }
      }
      
      console.log(this.items[i]);
    }
    else     {
      addedItem.pullQty = qty;
    this.items.push(addedItem);
    }
     this.inventoryList.next(this.items);
    
    this.saveCart();
    
  }



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
    this.items = this.items.filter(x=>x.clientID == client_id);
  }

  getTotalPrice() : number{
    let grandTotal = 0;
    this.cartItemList.map((a:any)=>{
      grandTotal += a.total;
    })
    return grandTotal;
  }
  

  removeItem(item,i:number) {
   // debugger;
    const index = this.items.findIndex(o => o.inventoryItemID === item.inv_item_id 
                                      && o.inventoryID === item.inventory_id);

    if (index > -1) {
      this.items.splice(index, 1);
    
    }
   
    this.inventoryList.next(this.items);
    this.saveCart();
  }

  clearCart() {
    //debugger;
    //this.items = [];
    
    var isSelectedFlag;
    this.items.forEach(x=>{
      // debugger;
      if (x.isSelected === false) {
         isSelectedFlag = true;
      }
      
       });

    if(isSelectedFlag){
      for (var i = 0; i < this.items.length; i++)
      {
        this.removeItem(this.items[i],i);
      }
    }
    else{
      this.items = [];
      localStorage.removeItem("cart_items");
    }

  
    this.inventoryList.next(this.items);
  
  }

}