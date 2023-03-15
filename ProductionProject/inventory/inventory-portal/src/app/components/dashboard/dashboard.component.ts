import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { inventory } from 'src/app/models/inventory.model';
import { InventoryService } from 'src/app/services/inventory.service';
import { DialogComponent } from '../dialog/dialog.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  name = 'Brun';
  animal = 'Lour';
  date:Date;
  inventoryList: inventory[];

  constructor(public dialog: MatDialog, public datepipe:DatePipe
    ,private inventoryService: InventoryService
    ) { }

  ngOnInit(): void {
    this.getInventory();

  }
  
  getInventory(){
    //this.inventoryService.getInventory(this.categoryId,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId,this.invsum,this.start).subscribe(data =>{
    //this.inventoryService.getInventory(this.categoryId,this.currentClient.clientId,this.buildingId,this.floorId,this.roomId,this.condId,this.start).subscribe(data =>{
      this.inventoryService.getInventory(7,127,-1,-1,'',-1,0).subscribe(data =>{
   debugger
      if(this.inventoryList === undefined){
        this.inventoryList = data;
      }else{
        this.inventoryList.push(...data);
      }
      console.log(this.inventoryList);    
  
    });
   
   
  }
  openDialog(): void {
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '250px',
      data: {
        name: this.name,
        animal: this.animal
      }
    })
  }

  myFunction(){
    this.date=new Date();
    let latest_date = this.datepipe.transform(this.date,"yyyy-MM-dd hh:mm:ss");
    console.log(latest_date);
  }
}
