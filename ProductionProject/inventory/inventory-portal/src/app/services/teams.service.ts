import { HttpClient, HttpHeaders } from '@angular/common/http';
import {Injectable} from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { throwError } from 'rxjs/internal/observable/throwError';
import { catchError } from 'rxjs/operators';
import {teams} from '../components/scrollable/mock-data';

const httpOptions={  
  headers : new HttpHeaders({  
    'Authorization':'563492ad6f917000010000014060d806c66c47b88b9b4d7f8c487692'  
  })  
}  
  
@Injectable({
  providedIn: 'root'
})
export class TeamsService {

  url = "https://localhost:44318/api/download/";

  constructor(private http:HttpClient) {
  }

  getTeams() {
    return teams;
  }

  getdata(search,perPage):Observable<any>{  
    const url="https://api.pexels.com/v1/search?query="+search+"&per_page="+perPage;  
    return this.http.get<any>(url,httpOptions).pipe(catchError(this.handelError));  
  }  
  handelError(error){  
    return throwError(error.message || "Server Error");  
  }  

  getImages(url:string,path:string){
    debugger;
   // return this.http.get<any>(url,httpOptions).pipe(catchError(this.handelError));  
   return this.http.get<any[]>(this.url + 'getfiles/'+ url +'/'+path);

  }
}