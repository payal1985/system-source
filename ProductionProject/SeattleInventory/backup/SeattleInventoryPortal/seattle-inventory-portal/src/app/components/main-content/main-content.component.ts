import { Component, OnInit,Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { InventoryService } from 'src/app/services/inventory.service';

@Component({
  selector: 'app-main-content',
  templateUrl: './main-content.component.html',
  styleUrls: ['./main-content.component.css']
})
export class MainContentComponent implements OnInit {
  id: any;
  @Input() deviceXs!: boolean;
  //@Input() catEvent!:string;
  inventoryList:any;
  // cat!:string;

  // @Input() catValue!: string;
  // public clickedEvent!: string;

  constructor(private route: ActivatedRoute,private inventoryService: InventoryService) {
    debugger; 
    //this.inventoryService.inv.subscribe(inv => this.cat = inv);
    this.inventoryService.castInventory.subscribe(inv => this.getInventory(""));
   // this.inventoryService.castInventory.subscribe(inv => this.getInventory(this.catEvent));
  
  }

  ngOnInit(): void {
    debugger;
    this.route.queryParams.subscribe(params => {
      this.id = params['id'];      
    });  

  //   console.log(this.catValue);
  // // debugger;
     this.inventoryService.castInventory.subscribe(inv => this.getInventory(this.id));

  }

  getInventory(value:string): any{   
    this.inventoryService.getInventory(value,"","").subscribe(data=>{
    this.inventoryList = data;
    console.log(this.inventoryList);
    // console.log(data);
    });
  }

  // childEventClicked(event: string) {
  //   this.clickedEvent = event;
  // }
}
