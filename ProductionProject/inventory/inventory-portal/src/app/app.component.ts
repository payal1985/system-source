import { Component } from '@angular/core';
import { ImageService } from './services/image.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers:[ImageService]
})
export class AppComponent {
  title = 'inventory-portal';
  imageToShow: any;
  isImageLoading: boolean;
  //imgUrl: string = 'https://picsum.photos/200/300/?random';

  constructor(private imageService: ImageService){
      
  } 
  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      debugger;
      reader.result;
       //this.imageToShow = reader.result;
       //this.imgUrl = 'https://picsum.photos/200/300/?random'
    }, false);
 
    if (image) {
      debugger;
       reader.readAsDataURL(image);
       //reader.readAsArrayBuffer(image);
    }
   }
   
  getImageFromService(imgUrl:string) {
    //this.isImageLoading = true;
    this.imageService.getImage(imgUrl).subscribe(data => {
      this.createImageFromBlob(data);
      this.isImageLoading = false;
    }, error => {
      this.isImageLoading = false;
      console.log(error);
    });
}
}
