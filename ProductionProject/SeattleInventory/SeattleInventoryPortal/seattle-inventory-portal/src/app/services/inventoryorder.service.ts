import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { inventoryorderitem } from '../model/inventoryorderitem';
import { order } from '../model/order';
import { ordercart } from '../model/ordercart';
import { GlobalConstants } from '../shared/global-constants';

@Injectable({
  providedIn: 'root'
})
export class InventoryorderService {
  private headers: HttpHeaders;
  url = GlobalConstants.apiURL + 'inventoryitemorder';

  //url = 'https://localhost:44325/api/inventoryitemorder';
 // url = 'https://ssidb-test.systemsource.com/siapi/api/inventoryitemorder';
 
  constructor(private http: HttpClient) { 
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

SaveOrderInfo(orderdetail:order): Observable<any>
{
  //debugger;

return this.http.post(this.url+ '/postinventoryitemorder',JSON.stringify(orderdetail),{ headers: this.headers });

}

getInventoryOrderItems(): Observable<inventoryorderitem[]> {
    //debugger;
  return this.http.get<inventoryorderitem[]>(this.url + '/getinventoryorderitem');
  
}

SaveOrder(ordercartdetail:ordercart[]){
  return this.http.post(this.url+ '/saveinventoryitemorder',JSON.stringify(ordercartdetail),{ headers: this.headers });

}

}
