import { Injectable } from '@angular/core';
//import { inventory } from '../model/inventory';
import {BehaviorSubject,Observable} from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GlobalConstants } from '../shared/global-constants';

@Injectable({
  providedIn: 'root'
})
export class InventoryItemService {

  public invitem = new BehaviorSubject<any>([]);
  castInventory = this.invitem.asObservable();
  
  url = GlobalConstants.apiURL;

  //url = 'https://localhost:44325/api/inventoryitem';
  //url = 'https://ssidb-test.systemsource.com/siapi/api/inventoryitem';
 
  constructor(private http: HttpClient) { }

  getBuildings(client_id:number): Observable<string[]> {
    
    return this.http.get<string[]>(this.url + 'inventoryitem/getbuildings?client_id='+client_id);
    
  }

  getFloor(client_id:number) : Observable<string[]>{
    return this.http.get<string[]>(this.url + 'inventoryitem/getfloor?client_id='+client_id);
  }

  getRooms(client_id:number) : Observable<string[]>{
    return this.http.get<string[]>(this.url + 'inventoryitem/getrooms?client_id='+client_id);
  }

}
