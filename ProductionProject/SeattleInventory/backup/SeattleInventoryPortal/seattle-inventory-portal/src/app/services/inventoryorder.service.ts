import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { order } from '../model/order';

@Injectable({
  providedIn: 'root'
})
export class InventoryorderService {
  private headers: HttpHeaders;
  url = 'https://localhost:44325/api/inventoryitemorder';
  constructor(private http: HttpClient) { 
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
  }

  // postMember(Member): Observable<any> {
  //   return this.http.post('http://localhost:8000/api/members/store', JSON.stringify(Member), {headers: this.getCommonHeaders()})
  //       .map(res => res.json())
  //       .catch(err => Observable.throw(err));
  // }
  // getCommonHeaders(){
  //   let headers = new Headers();
  //   headers.append('Content-Type','application/json');
  //   return headers;
  // }

SaveOrderInfo(orderdetail:order): Observable<any>
{
  debugger;
 

  var str="tesr";

//   const httpOptions = {
//     headers: new HttpHeaders(
//     {       
//        'Content-Type': 'application/json'
//     })
// };
return this.http.post(this.url+ '/postinventoryitemorder',JSON.stringify(orderdetail),{ headers: this.headers });

}
}
