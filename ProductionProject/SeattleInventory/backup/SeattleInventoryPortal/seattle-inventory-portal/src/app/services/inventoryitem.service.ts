import { Injectable } from '@angular/core';
//import { inventory } from '../model/inventory';
import {BehaviorSubject,Observable} from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class InventoryItemService {

  public invitem = new BehaviorSubject<any>([]);
  castInventory = this.invitem.asObservable();
  
  url = 'https://localhost:44325/api/inventoryitem';
  constructor(private http: HttpClient) { }

  getBuildings(): Observable<string[]> {
    
    return this.http.get<string[]>(this.url + '/getbuildings');
    
  }

  getFloor() : Observable<string[]>{
    return this.http.get<string[]>(this.url + '/getfloor');
  }

}
