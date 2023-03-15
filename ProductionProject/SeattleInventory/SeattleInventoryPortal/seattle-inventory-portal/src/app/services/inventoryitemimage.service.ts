import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {Observable} from 'rxjs';
import{ GlobalConstants } from '../shared/global-constants';

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
    var apiurl = this.url + 'inventoryitemimages/getinventoryitemimages?inv_id='+ inv_id;
    console.log(apiurl);
    return this.http.get<any[]>(apiurl);
    
  }
}
