import { Component, OnInit,Input } from '@angular/core';
import { InventoryService } from 'src/app/services/inventory.service';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css']
})
export class ContentComponent implements OnInit {
  @Input() deviceXs: boolean = false;
  topVal = 0;
  inventoryCategorylist!: string[];

 constructor(private inventoryService: InventoryService) {
    //this.inventoryService.castInventory.subscribe(inv => this.getInventory(this.inventorylist));
    this.inventoryService.castInventory.subscribe(inv => this.getInventoryCategory());
  }


  ngOnInit(): void {
  }

  onScroll(e:any) {
    let scrollXs = this.deviceXs ? 55 : 73;
    if (e.srcElement.scrollTop < scrollXs) {
      this.topVal = e.srcElement.scrollTop;
    } else {
      this.topVal = scrollXs;
    }
  }
  sideBarScroll() {
    let e = this.deviceXs ? 160 : 130;
    return e - this.topVal;
  }

  onClick(value:string){
    //  this.catEvent.emit(value);
     //  console.log(value);
    //debugger;
    //this.inventoryService.inv.next(value); 
    }
    
  getInventoryCategory(): any{
    this.inventoryService.getInventoryCategory().subscribe(data=>{
    this.inventoryCategorylist = data;
    //console.log(this.inventoryCategorylist);
    // console.log(data);
    });
  }
}
