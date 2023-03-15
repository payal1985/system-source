import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-image',
   templateUrl: './image.component.html',
  // template:`
  //   <h1>Loading secure images</h1>
  //   <input type="text" [attr.value]="jwtToken" (change)="setJwtToken($event.target.value)"/>
  //   <ul>
  //   <li *ngFor="let image of images$|async">
  //     <secured-image [src]="image.images.original.url"></secured-image>
  //   </li>
  //   </ul>
  // `,
  styleUrls: ['./image.component.scss']
})
export class ImageComponent implements OnInit {

  jwtToken = window.localStorage.getItem('jwtToken');
  // images$ = this.httpClient
  //   .get(`https://api.giphy.com/v1/gifs/search?q=dogs&imit=10&api_key=dc6zaTOxFJmzC`)
  //   .pipe(map((resp: any) => resp.data));
  


    images = [{
      image: 'http://maps.google.com/dsdl-nfndkfj-dfkdjf-1323nv'}
    ,{image: 'http://maps.google.com/mapfiles/ms/icons/blue.png'}
    ,{image: 'http://maps.google.com/mapfiles/ms/icons/blue.png'}
    ,{image: 'http://maps.google.com/mapfiles/ms/icons/blue.png'}];


  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
  }

  setJwtToken(token: any): void {
    debugger;
    this.jwtToken = token;
    window.localStorage.setItem('jwtToken', token);
  }

}
