import { Injectable } from '@angular/core';
import { inventory } from '../model/inventory';
import {BehaviorSubject,Observable} from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GlobalConstants } from '../shared/global-constants';
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

  getInventory(value:string,client_id:number,filterBuildingValue:string,filterFloor:string,filterRoom:string,filterCond:string): Observable<inventory[]> {
   // debugger;
    //this.inv.next(value);
    return this.http.get<inventory[]>
    (this.url + 'inventory/getinventories?category='+ value 
    + '&client_id='+client_id +'&building='+filterBuildingValue + '&floor='+filterFloor+ '&room='+filterRoom+ '&cond='+filterCond);
    
    
  }

  getInventoryCategory(client_id:number) : Observable<string[]>{
    return this.http.get<string[]>(this.url + 'inventory/getcategory?client_id='+client_id);
  }

}
