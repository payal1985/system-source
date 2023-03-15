import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InventoryitemService {
  
  url = 'https://localhost:44325/api/inventoryitem';
  constructor(private http: HttpClient) { }

  getBuildings(): Observable<string[]> {
    
    return this.http.get<string[]>(this.url + '/getbuildings');
    
  }

  getFloor() : Observable<string[]>{
    return this.http.get<string[]>(this.url + '/getfloor');
  }

}
