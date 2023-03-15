import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { AddDialogComponent } from './add-dialog/add-dialog.component';

@Component({
  selector: 'app-add-cart',
  templateUrl: './add-cart.component.html',
  styleUrls: ['./add-cart.component.scss']
})
export class AddCartComponent implements OnInit {

  title = 'angular-mat-select-app';

  selected: string='Select';
  selectedFormControl = new FormControl('');

  currencies = [
    { value: 'us', text: 'U.S. Dollar $' },
    { value: 'euro', text: 'Euro €' },
    { value: 'yen', text: 'Japanese Yen ¥' },
    { value: 'pound', text: 'Pounds £' },
    { value: 'inr', text: 'Indian Rupee ₹' }
  ];

   
  constructor(
    private dialog: MatDialog

  ) { 
    this.selectedFormControl.setValue('PAYAL');

  }

  ngOnInit(): void {
    this.selectedFormControl.setValue('PAYAL');
  }

  addDialog(type:string): void {
    //const message = `Are you sure you want to do this?`;
  
    //const dialogData = type;
  
    const dialogRef = this.dialog.open(AddDialogComponent, {
      // width: '25%',
      // height:'25%',
     panelClass: 'add-building-dialog'
    // data: {type:type}
    });
  
   // dialogRef.backdropClick().subscribe(() => { ; this.close(type,dialogRef); });
   //setTimeout(()=>{
    dialogRef.afterClosed().subscribe(dialogResult => {
      debugger;
      if(dialogResult)
      {
        this.selectedFormControl.setValue('PAYAL');

      }
      this.selected = 'TEST';
 
  
  
  });
  }

}
