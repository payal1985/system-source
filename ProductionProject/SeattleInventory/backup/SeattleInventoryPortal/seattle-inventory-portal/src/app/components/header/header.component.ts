import { Component,OnInit,Input  } from '@angular/core';
// import { BreakpointObserver } from '@angular/cdk/layout';
// import { MatSidenav } from '@angular/material/sidenav';
// import { delay } from 'rxjs/operators';
import {FormControl, Validators} from '@angular/forms';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';
import { InventoryService } from 'src/app/services/inventory.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  // @ViewChild(MatSidenav)
  // sidenav!: MatSidenav;
  buildingControl = new FormControl('', Validators.required);
  floorControl = new FormControl('', Validators.required);
  // selectFormControl = new FormControl('', Validators.required);
  @Input() catEvent!:string;
  @Input() deviceXs!: boolean;
  building!:string

  inventoryBuldinglist!: string[];
  inventoryFloorlist!: string[];
  
  // @Input() event!: string;


  constructor(private route: ActivatedRoute,private inventoryItemService: InventoryItemService,private inventoryService: InventoryService) {
    //this.inventoryService.castInventory.subscribe(inv => this.getInventory(this.inventorylist));
    this.inventoryItemService.castInventory.subscribe(inv => this.getInventoryBulding());
  }

  ngOnInit(): void {
      

   // console.log(this.catItem);
  }

  applyBuildingFilter(filterValue: string){    
      debugger
      console.log(filterValue);
      //this.router.navigate(['../',value]);
      this.route.queryParams.subscribe(params => {
        this.building = params['building'];      
      });
      //this.inventoryService.inv.next(filterValue);  
      //this.inventoryService.castInventory.subscribe(inv => this.getInventoryBulding());
 
  }

  getInventoryBulding(): any{
    this.inventoryItemService.getBuildings().subscribe(data=>{
    this.inventoryBuldinglist = data;
    console.log(this.inventoryBuldinglist);
    // console.log(data);
    });
  }

  getInventoryFloor(): any{
    this.inventoryItemService.getFloor().subscribe(data=>{
    this.inventoryFloorlist = data;
    console.log(this.inventoryFloorlist);
    // console.log(data);
    });
  }

}
