import { Component,OnInit,OnDestroy  } from '@angular/core';
import { MediaObserver, MediaChange } from '@angular/flex-layout';
import { Subscription } from 'rxjs';

// import { MediaObserver, MediaChange } from '@angular/flex-layout';
// import { Subscription } from 'rxjs';
// import { BreakpointObserver } from '@angular/cdk/layout';
// import { MatSidenav } from '@angular/material/sidenav';
// import { delay } from 'rxjs/operators';
//  import {FormControl, Validators} from '@angular/forms';
// import { InventoryService } from './services/inventory.service';

// interface Animal {
//   name: string;
//   sound: string;
// }

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'seattle-inventory-portal';

  mediaSub!: Subscription;
  deviceXs!: boolean;

  constructor(public mediaObserver: MediaObserver) {

  }
  ngOnInit() {
    this.mediaSub = this.mediaObserver.media$.subscribe((res: MediaChange) => {
      console.log(res.mqAlias);
      this.deviceXs = res.mqAlias === "xs" ? true : false;
    })
  }
  ngOnDestroy() {
    this.mediaSub.unsubscribe();
  }
  // public clickedEvent!: string;

  // childEventClicked(event: string) {
  //   this.clickedEvent = event;
  // }

  // @ViewChild(MatSidenav)
  // sidenav!: MatSidenav;
  // // selectFormControl = new FormControl('', Validators.required);
  // inventorylist!: any[];
  // inventoryCategorylist!: string[];
  // animalControl = new FormControl('', Validators.required);
  // selectFormControl = new FormControl('', Validators.required);
  // animals: Animal[] = [
  //   {name: 'Dog', sound: 'Woof!'},
  //   {name: 'Cat', sound: 'Meow!'},
  //   {name: 'Cow', sound: 'Moo!'},
  //   {name: 'Fox', sound: 'Wa-pa-pa-pa-pa-pa-pow!'},
  // ];

  //constructor(){}
  // constructor(private observer: BreakpointObserver,private inventoryService: InventoryService) {
  //   this.inventoryService.castInventory.subscribe(inv => this.getInventory(this.inventorylist));
  //   this.inventoryService.castInventory.subscribe(inv => this.getInventoryCategory());
  // }
  //ngOnInit() {}

//   ngAfterViewInit() {
//     this.observer
//       .observe(['(max-width: 800px)'])
//       .pipe(delay(1))
//       .subscribe((res) => {
//         if (res.matches) {
//           this.sidenav.mode = 'over';
//           this.sidenav.close();
//         } else {
//           this.sidenav.mode = 'side';
//           this.sidenav.open();
//         }
//       });
//   }

//   getInventory(value:any): any {
   
//     this.inventoryService.getInventory(value).subscribe(data => {
//     this.inventorylist = data;  
//     // console.log(this.inventorylist);
//     // console.log(data);
//     });
//   }

// getInventoryCategory(): any{
//   this.inventoryService.getInventoryCategory().subscribe(data=>{
//   this.inventoryCategorylist = data;
//   console.log(this.inventoryCategorylist);
//   // console.log(data);
//   });
// }

  

 

  // mediaSub: Subscription;
  // deviceXs: boolean = false;
  // constructor(public mediaObserver: MediaObserver) {
  //   this.mediaSub = this.mediaObserver.media$.subscribe((res: MediaChange) => {
  //     //console.log(res.mqAlias);
  //   })
  // }
  // ngOnInit() {
  //   this.mediaSub = this.mediaObserver.media$.subscribe((res: MediaChange) => {
  //     //console.log(res.mqAlias);
  //     this.deviceXs = res.mqAlias === "xs" ? true : false;
  //   })
  // }
  // ngOnDestroy() {
  //   this.mediaSub.unsubscribe();
  // }
  
}
