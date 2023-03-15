import { Component, ElementRef, OnInit } from '@angular/core';
import { UploadS3Service } from 'src/app/services/uploads3.service';

@Component({
  selector: 'app-dropzone-dialog',
  templateUrl: './dropzone-dialog.component.html',
  styleUrls: ['./dropzone-dialog.component.scss']
})
export class DropzoneDialogComponent implements OnInit {

	//imgTag:string;
	imgUrl:string = 'https://systemsource.s3.us-west-2.amazonaws.com/inventory/127/';
	imgName:string="";
	htmlToAdd:string;
id:string="idDoc";

  constructor(private elementRef:ElementRef
	,private uploadS3Service: UploadS3Service) { 

  }

  ngOnInit(): void {
	  
  }


  imgfiles: File;
  docfiles: File[] = [];
  renderImages: any = [];

	onSelect(event) {
		debugger;
		console.log(event.addedFiles);
		//event.addedFiles.file.name = "test123.png";
	//	console.log(event.addedFiles.file.name);
		this.imgfiles= event.addedFiles;
	//	this.files[0].name = "123.png";
	// this.imgName = this.imgfiles[0].name;
	this.onImageUpdate(this.imgfiles,"img");

	console.log("file->",this.imgfiles);
	}

	onSelectDoc(event) {
		debugger;
		console.log(event.addedFiles);
		//event.addedFiles.file.name = "test123.png";
	//	console.log(event.addedFiles.file.name);
		this.docfiles.push(...event.addedFiles);
	//	this.files[0].name = "123.png";


	this.onImageUpdate(this.docfiles,"fimg");
	

	// this.htmlToAdd = `<a href="https://systemsource.s3.us-west-2.amazonaws.com/inventory/127/4CSDN06.jpg" target="_blank">
	// 				`+this.docfiles[0].name+`</a>`;

	// var d1 = this.elementRef.nativeElement.querySelector('.docFile');
	// d1.insertAdjacentHTML('beforeend', `<a href="https://systemsource.s3.us-west-2.amazonaws.com/inventory/127/4CSDN06.jpg" target="_blank">
	// `+this.docfiles[0].name+`</a>`);

	//this.htmlToAdd = '<div class="two">two</div>';

	console.log("file->",this.docfiles);
	}


	async onImageUpdate(files,subfolder) {
		debugger;
		console.log(files);
		if (files.length < 1) {
		  console.log('Please Select Drop your Image first');
		  return;
		}
	
		for (let i = 0; i < files.length; i += 1) {
		  let file = files[i];
	
		  let filePath =
			'inventory/12/' + subfolder + '/' +  file.name; // to create unique name for avoiding being replaced
		  try {
			let response = await this.uploadS3Service.uploadFile(file, filePath);
			console.log(response);
	
			console.log(file.name + 'uploaded Successfully :)');
			const url = (response as any).Location;
			this.renderImages.push(url);
		  } catch (error) {
			console.log('Something went wrong! ');
		  }
		}
		files = [];
	  }

	// onFilesDropped(event): void {
	// 	debugger;
	// 	console.log(event);
	//   }

	// onRemove(event) {
	// 	console.log(event);
	// 	this.files.splice(this.files.indexOf(event), 1);
	// }
}
