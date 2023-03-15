import { Component, OnInit } from '@angular/core';
import { TeamsService } from 'src/app/services/teams.service';

@Component({
  selector: 'app-scrollable',
  templateUrl: './scrollable.component.html',
  styleUrls: ['./scrollable.component.scss']
})
export class ScrollableComponent implements OnInit {

  //teams = [];
  array = [];  
  sum = 10;  
  throttle = 300;  
  scrollDistance = 1;  
  scrollUpDistance = 2;  
  direction = "";  
  modalOpen = false;  
  photos: any;  
  start: number = 0;  
  
  constructor(private teamsService: TeamsService) { }

  ngOnInit(): void {
    //this.getTeams();
    this.getPhoto();  
  }

  getPhoto() {  
    this.teamsService.getdata('bikes', this.sum).subscribe((response: any) => {   
      this.photos = response.photos;  
      this.addItems(this.start, this.sum);  
    }, (error) => {  
      console.log(error);  
    })  
  }  
  selector: string = '.main-panel';  
  
  addItems(index, sum) {  
    for (let i = index; i < sum; ++i) {  
      //debugger  
      this.array.push(this.photos[i]);  
      console.log(this.array);  
  
    }  
  }  
  
  onScrollDown(ev) {  
  debugger;
    // add another 20 items  
    this.start = this.sum;  
    this.sum += 20;  
    this.getPhoto();  
    //this.direction = "down";  
  }  

  getImage(){
    debugger;
this.teamsService.getImages("https://tst.assets.systemsource.com","/private/unnamed.jpg");
  }
  // getTeams(){
  //   this.teams = this.teamsService.getTeams();
  // }

}
