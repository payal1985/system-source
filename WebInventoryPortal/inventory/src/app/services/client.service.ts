import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { of } from 'rxjs';
import { map } from "rxjs/operators";
import { client } from '../models/client';
import { GlobalConstants } from '../shared/global-constants';


@Injectable({
  providedIn: 'root'
})
export class ClientService {


  url = GlobalConstants.loginApiURL;

  private currentClientSubject: BehaviorSubject<client>;
  public currentClient: Observable<client>;
  
  constructor(private http: HttpClient) { 
    this.currentClientSubject = new BehaviorSubject<client>(JSON.parse(localStorage.getItem('currentClient')));
    this.currentClient = this.currentClientSubject.asObservable()
  }

  public get currentClientValue(): client {
    return this.currentClientSubject.value;
}


  getClients(user: string, pwd: string) : Observable<client[]>
  {
 
    return this.http.get<any>(this.url + 'login/getclients?userName='+user+'&password='+pwd);
//     return this.http.get<any>(this.url + 'login/getclients?userName='+user+'&password='+pwd).pipe(map(client=>{
//       if (client) {
//         // store client details in local storage to keep client logged in between page refreshes
//         // localStorage.setItem('currentClient', JSON.stringify(client));
//         // this.currentClientSubject.next(client);
//     }
// debugger;
//     return client;
//     }));
  
  }

  setClient(client:client){
      //debugger;
    //var client =  new client();
    //var tmclient = this.currentClient.subscribe(x=>x.clientId == client_id);
    
    localStorage.setItem('currentClient', JSON.stringify(client));
    this.currentClientSubject.next(client);

  }

 
}
