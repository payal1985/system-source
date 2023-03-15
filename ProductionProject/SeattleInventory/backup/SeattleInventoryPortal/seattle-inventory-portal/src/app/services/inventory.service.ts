import { Injectable } from '@angular/core';
import { inventory } from '../model/inventory';
import {BehaviorSubject,Observable} from 'rxjs';
import { HttpClient } from '@angular/common/http';
//import { inventory } from 'src/app/model/inventory';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {

  //invDataList:inventory[];
  

  public inv = new BehaviorSubject<any>([]);
  castInventory = this.inv.asObservable();


  
  url = 'https://localhost:44325/api/inventory';
  constructor(private http: HttpClient) { }

  getInventory(value:string,filterBuildingValue:string,filterFloor:string): Observable<inventory[]> {
   // debugger;
    //this.inv.next(value);
    return this.http.get<inventory[]>
    (this.url + '/getinventories?category='+ value 
    + '&building='+filterBuildingValue + '&floor='+filterFloor);
    
    
  }

  getInventoryCategory() : Observable<string[]>{
    return this.http.get<string[]>(this.url + '/getcategory');
  }

}
