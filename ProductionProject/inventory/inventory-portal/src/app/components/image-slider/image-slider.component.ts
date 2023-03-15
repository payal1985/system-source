import { UrlResolver } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ImageDialogComponent } from '../image-dialog/image-dialog.component';
import { Location } from '@angular/common';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrls: ['./image-slider.component.scss']
})
export class ImageSliderComponent implements OnInit {

  imgSrc:string;

  constructor(public dialog: MatDialog,private location: Location) {
    this.location.replaceState("/user-reviews/something");
   }


  ngOnInit(): void {
    this.location.replaceState("/user-reviews/something");
   // imgSrc = this.location.replaceState("/");
  }

  imageObject = [{
    image: 'http://ssidb-test.systemsource.com/Project/GoCanvasImages/BKNO76.jpg',
    //thumbImage: 'https://sanjayv.github.io/ng-image-slider/contents/assets/img/slider/5.jpg',
    title: 'Hummingbirds are amazing creatures'
}, {
    image: 'http://ssidb-test.systemsource.com/Project/GoCanvasImages/BKNO76_1.jpg',
    //thumbImage: 'https://sanjayv.github.io/ng-image-slider/contents/assets/img/slider/9.jpg'
}, {
    image: 'http://ssidb-test.systemsource.com/Project/GoCanvasImages/BKnoll86.jpg',
    //thumbImage: 'https://sanjayv.github.io/ng-image-slider/contents/assets/img/slider/4.jpg',
    title: 'Example with title.'
},{
    image: 'http://ssidb-test.systemsource.com/Project/GoCanvasImages/BKnoll86_1.jpg',
    //thumbImage: 'https://sanjayv.github.io/ng-image-slider/contents/assets/img/slider/7.jpg',
    title: 'Hummingbirds are amazing creatures'
}, {
    image: 'http://ssidb-test.systemsource.com/Project/GoCanvasImages/BKnoll86_2.jpg',
   // thumbImage: 'https://sanjayv.github.io/ng-image-slider/contents/assets/img/slider/1.jpg'
}, {
    image: 'http://ssidb-test.systemsource.com/Project/GoCanvasImages/BKnoll86_3.jpg',
    //thumbImage: 'https://sanjayv.github.io/ng-image-slider/contents/assets/img/slider/2.jpg',
    title: 'Example two with title.'
}];


openImageCarouselDialog(){  
  this.dialog.open(ImageDialogComponent,{
  width: '80%',
  height: '50%',
  //data: this.cartInventoryItem
  data: this.imageObject
  });
}

}
