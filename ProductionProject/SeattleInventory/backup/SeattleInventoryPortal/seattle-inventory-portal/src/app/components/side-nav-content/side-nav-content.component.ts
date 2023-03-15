import { Component,OnInit,Input  } from '@angular/core';
import { InventoryService } from 'src/app/services/inventory.service';


@Component({
  selector: 'app-side-nav-content',
  templateUrl: './side-nav-content.component.html',
  styleUrls: ['./side-nav-content.component.css']
})


export class SideNavContentComponent implements OnInit {
  
  inventoryList:any;
 
  inventoryCategorylist!: string[];
  //@Output() catEvent = new EventEmitter<string>();
  @Input() deviceXs!: boolean;

  constructor(private inventoryService: InventoryService) {
    //this.inventoryService.castInventory.subscribe(inv => this.getInventory(this.inventorylist));
    this.inventoryService.castInventory.subscribe(inv => this.getInventoryCategory());
  }

  ngOnInit(): void {    
    this.inventoryService.castInventory.subscribe(inv => this.getInventoryCategory());

  }

  onClick(value:string){
  //  this.catEvent.emit(value);
   //  console.log(value);
  debugger;
  //this.inventoryService.inv.next(value); 
  this.inventoryService.castInventory.subscribe(inv=>this.getInventory(value));
  }

  getInventoryCategory(): any{
    this.inventoryService.getInventoryCategory().subscribe(data=>{
    this.inventoryCategorylist = data;
    //console.log(this.inventoryCategorylist);
    // console.log(data);
    });
  }

  getInventory(value:string): any{   
    this.inventoryService.getInventory(value,"","").subscribe(data=>{
    this.inventoryList = data;
    console.log(this.inventoryList);
    // console.log(data);
    });
  }
}
