import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GlobalConstants } from '../shared/global-constants';
import { Observable } from 'rxjs';
import { donationinventory } from '../model/donationinventory';


@Injectable({
  providedIn: 'root'
})
export class DonationInventoryService {

  url = GlobalConstants.apiURL + 'donationinventory';

 constructor(private http: HttpClient) { }

  getInventory(value:string): Observable<donationinventory[]> {
   // debugger;
    //this.inv.next(value);
    return this.http.get<donationinventory[]>
    (this.url + '/getinventories?category='+ value);   
    
  }

  getInventoryCategory() : Observable<string[]>{
    return this.http.get<string[]>(this.url + '/getcategory');
  }

}