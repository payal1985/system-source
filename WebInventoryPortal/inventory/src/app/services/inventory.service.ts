import { Injectable } from '@angular/core';
import { inventory } from '../models/inventory';
import {BehaviorSubject,Observable} from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GlobalConstants } from '../shared/global-constants';
import { itemtyeps } from '../models/itemtypes';
//import { inventory } from 'src/app/model/inventory';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {

  //invDataList:inventory[];
  

  public inv = new BehaviorSubject<any>([]);
  castInventory = this.inv.asObservable();


  url = GlobalConstants.apiURL;
  
  //url = 'https://localhost:44325/api/inventory';
  //url = 'https://ssidb-test.systemsource.com/siapi/api/inventory';
  constructor(private http: HttpClient) { }

  getInventory(categoryid:number,client_id:number,filterBuildingValue:string,filterFloor:string,filterRoom:string,filterCond:string): Observable<inventory[]> {
   // debugger;
    //this.inv.next(value);
    return this.http.get<inventory[]>
    (this.url + 'inventory/getinventories?itemTypeId='+ categoryid 
    + '&client_id='+client_id +'&building='+filterBuildingValue + '&floor='+filterFloor+ '&room='+filterRoom+ '&cond='+filterCond);
    
    
  }

  getInventoryCategory(client_id:number) : Observable<itemtyeps[]>{
    return this.http.get<itemtyeps[]>(this.url + 'inventory/getcategory?client_id='+client_id);
  }

  searchInventory(value:string,client_id:number,filterBuildingValue:string,filterFloor:string,filterRoom:string,filterCond:string): Observable<inventory[]> {
     //debugger;
     //this.inv.next(value);
     return this.http.get<inventory[]>
     (this.url + 'inventory/searchinventories?search='+ value 
     + '&client_id='+client_id +'&building='+filterBuildingValue + '&floor='+filterFloor+ '&room='+filterRoom+ '&cond='+filterCond);
     
     
   }
}
