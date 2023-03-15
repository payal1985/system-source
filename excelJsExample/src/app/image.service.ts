import { Injectable,ReflectiveInjector } from '@angular/core';
import { HttpClient,HttpClientModule  } from '@angular/common/http';
//import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';

import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ImageService {

  constructor(private httpClient: HttpClient) { }

  // private httpClient;
  // constructor() { 
  //   let injector = ReflectiveInjector.resolveAndCreate([
  //     (<any>HttpClientModule).decorators[0].args[0].providers
  // ]);
  // this.httpClient = injector.get(HttpClient);
  // console.log(this.httpClient)
  // }
  //imageToShow: any;

  getImage(imageUrl: string): Observable<Blob> {
    return this.httpClient.get(imageUrl, { responseType: 'blob' });
  }

//   createImageFromBlob(image: Blob) {
//     let reader = new FileReader();
//     reader.addEventListener("load", () => {
//        this.imageToShow = reader.result;
//     }, false);
 
//     if (image) {
//        reader.readAsDataURL(image);
//     }
//  }
}
