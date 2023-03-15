import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { inventory } from '../models/inventory.model';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {


  url = 'https://localhost:44331/api/inventory';
  constructor(private http: HttpClient) { }

  // getInventory(value:string,filterBuildingValue:string,filterFloor:string): Observable<inventory[]> {
  //  // debugger;
  //   //this.inv.next(value);
  //   return this.http.get<inventory[]>
  //   (this.url + '/getinventories?category='+ value 
  //   + '&building='+filterBuildingValue + '&floor='+filterFloor);
    
    
  // }

  getInventory(categoryid:number,client_id:number,filterBuildingValue:number,filterFloor:number,filterRoom:string,filterCond:number,startIndex:number): Observable<inventory[]> {
   
    //this.inv.next(value);
    return this.http.get<inventory[]>
    (this.url + '/getinventories?itemTypeId='+ categoryid 
    + '&client_id='+client_id +'&building='+filterBuildingValue + '&floor='+filterFloor+ '&room='+filterRoom+ '&cond='+filterCond+ '&startindex='+startIndex);
        
  }

  getInventoryCategory() : Observable<string[]>{
    return this.http.get<string[]>(this.url + '/getcategory');
  }

}
