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



  constructor() {

  }
  ngOnInit() {
    
  }
  ngOnDestroy() {
    
  }

  
}
