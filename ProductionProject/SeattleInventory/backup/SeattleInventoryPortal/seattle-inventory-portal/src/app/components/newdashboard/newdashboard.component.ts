import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { InventoryService } from 'src/app/services/inventory.service';

@Component({
  selector: 'app-newdashboard',
  templateUrl: './newdashboard.component.html',
  styleUrls: ['./newdashboard.component.css']
})
export class NewdashboardComponent implements OnInit {
  showFiller = false;

  inventoryCategorylist!: string[];

  constructor(private inventoryService: InventoryService) { 
    this.inventoryService.getInventoryCategory().subscribe(data => {
      this.inventoryCategorylist = data;
    });
  }

  ngOnInit(): void {
  }

  onClick(value:string){
    //  this.catEvent.emit(value);
     //  console.log(value);
    debugger;
    //this.inventoryService.inv.next(value); 
   // this.inventoryService.castInventory.subscribe(inv=>this.getInventory(value));
    }

    
  // getInventoryCategory(): any{
  //   this.inventoryService.getInventoryCategory().subscribe(data=>{
  //   this.inventoryCategorylist = data;
  //   //console.log(this.inventoryCategorylist);
  //   // console.log(data);
  //   });
  // }
}
