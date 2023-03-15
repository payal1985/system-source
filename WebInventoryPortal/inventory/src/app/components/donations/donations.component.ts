import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BreakpointObserver,Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { DonationInventoryService } from 'src/app/services/donationinventory.service';
import { donationinventory } from 'src/app/models/donationinventory';
import { ActivatedRoute, Router } from '@angular/router';
import { ImageSliderDialogComponent } from './image-slider-dialog/image-slider-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-donations',
  templateUrl: './donations.component.html',
  styleUrls: ['./donations.component.scss']
})
export class DonationsComponent implements OnInit {

  isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset);
  inventoryCategorylist: string[];
  inventoryList: donationinventory[];

  showCategory:string;
  id: string;

  constructor(private breakpointObserver: BreakpointObserver
    ,private donationInventoryService: DonationInventoryService
    ,private route: ActivatedRoute
    ,public dialog: MatDialog
    ,private routerNav:Router
    ) { 

    this.donationInventoryService.getInventoryCategory().subscribe(data => {
      this.inventoryCategorylist = data;
    });

  }

  ngOnInit(): void {
//debugger;
    this.route.queryParams.subscribe(params => {
      this.id = params['id'];      
    });

    // this.router.navigate(['/products'], { queryParams: { order: 'popular' } });
    // this.routerNav.navigate(['/donations'], {
    //   relativeTo: this.route,
    //   queryParams: {
    //     cat: this.id
    //   },
    //   queryParamsHandling: 'merge'      
    // });

  

    this.showCategory = this.id;

    if(this.id !== "")
    {
      this.donationInventoryService.getInventory(this.id).subscribe(data =>{
        this.inventoryList = data;
      });
    }
  }

  onClick(value:string){ 
    //debugger;
    if(value=="home")
    {
      this.showCategory = "";  
      this.inventoryList = [];

    }
    else
    {
      this.routerNav.navigate(['/donations'], { queryParams: { cat: value } });

      this.showCategory = value;  
      this.donationInventoryService.getInventory(value).subscribe(data =>{
        this.inventoryList = data;
      });
    }
   
  
    console.log(this.inventoryList);      
  }

  openDialog(inv_id:number) {
   
    this.dialog.open(ImageSliderDialogComponent, {
      width: '100%',
      height: '100%',
      data: {id:inv_id}
    });
  }
  
}
