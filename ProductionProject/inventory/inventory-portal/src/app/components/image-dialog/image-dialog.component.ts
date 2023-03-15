import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
// import { Router } from '@angular/router';
// import { map,switchMap } from 'rxjs/operators';
// import {HttpClient} from '@angular/common/http';
// import { DomSanitizer } from '@angular/platform-browser';
// import { BehaviorSubject } from 'rxjs';
import { Location } from '@angular/common';

@Component({
  selector: 'app-image-dialog',
  templateUrl: './image-dialog.component.html',
  styleUrls: ['./image-dialog.component.scss']
})
export class ImageDialogComponent implements OnInit {

  imagedatasource:any;
  // public src: string="";
  // private src$ = new BehaviorSubject(this.src);
  
  constructor(public dialogRef: MatDialogRef<ImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ,private location: Location
    // ,private router:Router,private httpClient: HttpClient
    // , private domSanitizer: DomSanitizer
    ) { 
      console.log(this.data);
    }

  ngOnInit(): void {

    this.imagedatasource = this.data;
    console.log(this.imagedatasource);

  }

  openImgInNewWindow(imgSrc) {
    // Converts the route into a string that can be used 
    // with the window.open() function
    debugger;
    // const url = this.router.serializeUrl(
    //   //this.router.createUrlTree([`/custompage/${imgSrc}`])
    //   this.router.createUrlTree([`${imgSrc}/123-ffkk`])
    // )
   
    

    
    window.open(imgSrc, '_blank');
  }

  onRightClick(event) {
    debugger;
    event.target.src = this.location.replaceState("/user-reviews/something");
    return true;
}

}
