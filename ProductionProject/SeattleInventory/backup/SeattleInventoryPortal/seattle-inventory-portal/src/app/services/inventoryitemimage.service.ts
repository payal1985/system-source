import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import { concat } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InventoryitemimageService {
  
  url = 'https://localhost:44325/api/inventoryitemimages';
  constructor(private http: HttpClient) { }

  getInvItemImages(inv_item_id:number): Observable<any[]> {
    debugger;
    var apiurl = this.url + '/getinventoryitemimages?inv_item_id='+ inv_item_id;
    console.log(apiurl);
    return this.http.get<any[]>(apiurl);
    
  }
}
