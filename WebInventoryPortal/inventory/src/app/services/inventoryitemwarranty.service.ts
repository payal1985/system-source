import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { addcart } from '../models/addcart';
import { inventoryorderitem } from '../models/inventoryorderitem';
import { order } from '../models/order';
//import { order } from '../models/order';
import { ordercart } from '../models/ordercart';
import { GlobalConstants } from '../shared/global-constants';

@Injectable({
  providedIn: 'root'
})
export class InventoryItemWarrantyService {
  private headers: HttpHeaders;
  url = GlobalConstants.apiURL + 'inventoryitemwarranty';

  //url = 'https://localhost:44325/api/inventoryitemorder';
 // url = 'https://ssidb-test.systemsource.com/siapi/api/inventoryitemorder';
 
  constructor(private http: HttpClient) { 
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

SaveWarrantyRequest(ordercartdetail:addcart[]){
  return this.http.post(this.url+ '/savewarrantyrequest',JSON.stringify(ordercartdetail),{ headers: this.headers });

}

}
