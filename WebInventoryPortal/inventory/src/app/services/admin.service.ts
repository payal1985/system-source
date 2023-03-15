import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GlobalConstants } from '../shared/global-constants';
import { client } from '../models/client';
//import { inventory } from 'src/app/model/inventory';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  url = GlobalConstants.apiURL + 'clientinventory';
  private headers: HttpHeaders;

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    
  }

  getClients(): Observable<client[]> {
   // debugger;
    //this.inv.next(value);
    return this.http.get<client[]>(this.url + '/getclients');
    
  }
 
  updateClientHasInventory(item:client): Observable<any>{
    //debugger;
    return this.http.put<any>(this.url + '/updateclienthasinventory',JSON.stringify(item),{ headers: this.headers });
  }

  SaveClientInfo(item:client):Observable<any>
  {
    //debugger;  
    return this.http.post(this.url+ '/postclient',JSON.stringify(item),{ headers: this.headers });
  
  }

}
