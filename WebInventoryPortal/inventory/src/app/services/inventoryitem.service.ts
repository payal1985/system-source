import { Injectable } from '@angular/core';
//import { inventory } from '../model/inventory';
import {BehaviorSubject,Observable} from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GlobalConstants } from '../shared/global-constants';
import { inventorybuilding } from '../models/inventorybuilding';
import { inventoryfloor } from '../models/inventoryfloor';

@Injectable({
  providedIn: 'root'
})
export class InventoryItemService {

  private headers: HttpHeaders;
  public invitem = new BehaviorSubject<any>([]);
  castInventory = this.invitem.asObservable();
  
  url = GlobalConstants.apiURL + 'inventoryitem';

  //url = 'https://localhost:44325/api/inventoryitem';
  //url = 'https://ssidb-test.systemsource.com/siapi/api/inventoryitem';
 
  constructor(private http: HttpClient) { 
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  getBuildings(client_id:number): Observable<inventorybuilding[]> {
    
    return this.http.get<inventorybuilding[]>(this.url + '/getbuildings?client_id='+client_id);
    
  }

  getFloor(client_id:number) : Observable<inventoryfloor[]>{
    return this.http.get<inventoryfloor[]>(this.url + '/getfloor?client_id='+client_id);
  }

  getRooms(client_id:number) : Observable<string[]>{
    return this.http.get<string[]>(this.url + '/getrooms?client_id='+client_id);
  }

  getConditions() : Observable<string[]>{
    return this.http.get<string[]>(this.url + '/getconditions');
  }

  getDepCostCenters() : Observable<string[]>{
    return this.http.get<string[]>(this.url + '/getdepcostcenters');
  }

  postBuilding(invbldg:inventorybuilding):Observable<inventorybuilding>{
    //debugger;
     return this.http.post<inventorybuilding>(this.url+ '/insertbuilding',JSON.stringify(invbldg),{ headers: this.headers });
    //return this.http.post(this.url+ '/insertbuilding',{client_id,building,username},{ headers: this.headers });
  
  }

  postFloor(invflr:inventoryfloor):Observable<inventoryfloor>{
    //debugger;
     return this.http.post<inventoryfloor>(this.url+ '/insertfloor',JSON.stringify(invflr),{ headers: this.headers });
    //return this.http.post(this.url+ '/insertbuilding',{client_id,building,username},{ headers: this.headers });
  
  }
}
