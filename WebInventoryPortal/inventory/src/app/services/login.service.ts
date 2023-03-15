import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
// import 'rxjs/add/observable/of';
import { of } from 'rxjs';
import { map, delay } from "rxjs/operators";
import { users } from '../models/users';
import { GlobalConstants } from '../shared/global-constants';
// import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LoginService {


  url = GlobalConstants.loginApiURL;
  isUser=false;
  public response;

  private currentUserSubject: BehaviorSubject<users>;
  public currentUser: Observable<users>;
  
  constructor(private http: HttpClient) { 
    this.currentUserSubject = new BehaviorSubject<users>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable()
  }

  public get currentUserValue(): users {
    return this.currentUserSubject.value;
}

  isLoggedIn() {
    return localStorage.getItem('currentUser') ? true : false;
  }

  validateLogin(user: string, pwd: string) : Observable<users[]>
  {
    debugger;
    //return this.http.get<any>(this.url + 'login/getusers?userName='+user+'&password='+pwd);
    return this.http.get<any>(this.url + 'login/getusers?userName='+user+'&password='+pwd).pipe(map(user=>{
      if (user) {
        debugger;
        // store user details and jwt token in local storage to keep user logged in between page refreshes
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
    }

    return user;
    }));
    //   debugger;
    //   return response;
  }

  setUserFlag(user:users[])
  {
      localStorage.setItem('currentUser', JSON.stringify(user));
      return true;
  }

  // //async login(user: string, password: string): Observable<boolean> {
  // async login(user: string, password: string) {
  //   debugger;
  // // login(user: users): Observable<boolean> {
  // // debugger;

  // // if(user != null)
  // // {
  // //      localStorage.setItem('currentUser', JSON.stringify(user));
  // //     return of(true);
  // // }
  // // else
  // // {
  // //       return of(false);
  // // }

  //  //var usrdetails = this.getUsers(user,password);
  //  this.response = await this.getUsers(user,password);
  //  //await delay(3000);
  //  if(this.response != null)
  //  {
  //    debugger;
  //  localStorage.setItem('currentUser', JSON.stringify(this.response));
  //  this.isUser = true;
  //  }
  //  else
  //  debugger;
  //     this.isUser = false;

  // //  await this.getUsers(user,password).subscribe(data =>
  // //   {
  // //     debugger;
  // //     console.log('login service',data);
      
  // //     localStorage.setItem('currentUser', JSON.stringify(data));
  // //     if(data != null)
  // //       this.isUser = true;
  // //     else
  // //     this.isUser = false;

  // //   });
 
  // // debugger;
  // //// if email is valid (matches regex) and password length > 0
  //   // let user= {
  //   //   'email': email,
  //   //   'password': password
  //   // }
  //   return this.isUser;
 
  // }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  // //getUsers(user:string,pwd:string): Observable<users> {   
  // async getUsers(user:string,pwd:string) {   
  //   debugger;
  //   //var newurl = this.url + 'login/getusers?user='+user+'&pwd='+pwd;
  //   //return await this.http.get<users>(this.url + 'login/getusers?user='+user+'&pwd='+pwd); 
  //   //return await this.http.get<users>(this.url + 'login/getusers?user='+user+'&pwd='+pwd); 
  //   let response = await this.http.get<any>(this.url + 'login/getusers?userName='+user+'&password='+pwd).toPromise();
  //   debugger;
  //   return response;
  // }

  
  loadUser(): Observable<users> {
     return of(JSON.parse(localStorage.getItem("currentUser")) ?? '');
  }
  // // getTest(usr:string): Observable<users>{
  // //   debugger;
  // //   return this.http.get<users>(this.url + 'login/gettests?user='+usr); 

  // // }

  // // register(user) {
  // //   if(user){
  // //     return of(true);
  // //   }
  // //   else
  // //   {
  // //     return of(false);
  // //   }
  // // }
}
