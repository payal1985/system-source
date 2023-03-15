import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import{ GlobalConstants } from '../shared/global-constants';
import { inventoryitemimages } from '../models/inventoryitemimages';

@Injectable({
  providedIn: 'root'
})
export class InventoryitemimageService {
  
  url = GlobalConstants.apiURL;
  
  //url = 'https://localhost:44325/api/inventoryitemimages';
  //url = 'https://ssidb-test.systemsource.com/siapi/api/inventoryitemimages';
  constructor(private http: HttpClient) { }

  getInvItemImages(inv_id:number): Observable<any[]> {
    //debugger;
    // var apiurl = this.url + 'inventoryitemimages/getinventoryitemimages?inv_id='+ inv_id;
    // console.log(apiurl);
    return this.http.get<any[]>(this.url + 'inventoryitemimages/getinventoryitemimages?inv_id='+ inv_id);
    
  }

  getInvItemImagesForCondition(inv_item_id:number,condition:string):Observable<inventoryitemimages[]>{
    return this.http.get<inventoryitemimages[]>(this.url + 'inventoryitemimages/getinvitemimagesforcond?inv_item_id='+ inv_item_id + '&condition='+condition);

  }
}
