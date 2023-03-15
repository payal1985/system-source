import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { inventorybuilding } from 'src/app/models/inventorybuilding';
import { inventoryfloor } from 'src/app/models/inventoryfloor';
import { InventoryItemService } from 'src/app/services/inventoryitem.service';

@Component({
  selector: 'app-add-dialog',
  templateUrl: './add-dialog.component.html',
  styleUrls: ['./add-dialog.component.scss']
})
export class AddDialogComponent implements OnInit {

  type: string;

  typeFormControl = new FormControl('', [Validators.required]);

  constructor(public dialogRef: MatDialogRef<AddDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ,public inventoryItemService:InventoryItemService) { 

      this.type = this.data.type;     

    }

  ngOnInit(): void {
  }

  onAddType(): void {
    // Close the dialog, return true
    //debugger;
    if(this.type == 'Building')
    {
      var invbldg = new inventorybuilding();
      invbldg.clientId = this.data.clientId;
      invbldg.inventoryBuildingName = this.typeFormControl.value;
      invbldg.userId = this.data.userId;


      this.inventoryItemService.postBuilding(invbldg).subscribe({
        next: _ => {
         // debugger;
          //this.loginValid = _ != null ? this.loginService.setUserFlag(_) : false;
          
          if(_)
            this.dialogRef.close(_);
        },
        error: _ => {
          alert(_.message + "\n Unable to create building");
          //this.clicked=false;
          console.log(_); 
        }
      });

      // this.inventoryItemService.postBuilding(invbldg).subscribe(
      //   response => { 
      //     if(response)
      //     {
      //       debugger;
      //       this.dialogRef.close(response);
      //     }
      //     console.log(response); 
      //   },
      //   error => {      
      //    // debugger;     
      //     alert(error.message + "\n Unable to create building");
      //     //this.clicked=false;
      //     console.log(error); 
      //   });
    }
    else if(this.type === 'Floor')
    {
      var invflr = new inventoryfloor();
      invflr.clientId = this.data.clientId;
      invflr.inventoryFloorName = this.typeFormControl.value;
      invflr.userId = this.data.userId;

      this.inventoryItemService.postFloor(invflr).subscribe({
        next: _ => {
         // debugger;
          //this.loginValid = _ != null ? this.loginService.setUserFlag(_) : false;
          
          if(_)
            this.dialogRef.close(_);
        },
        error: _ => {
          alert(_.message + "\n Unable to create floor");
          //this.clicked=false;
          console.log(_); 
        }
      });
      // this.inventoryItemService.postFloor(invflr).subscribe(
      //   response => { 
      //     if(response)
      //     {
      //      // debugger;
      //       this.dialogRef.close(response);
      //     }
      //     console.log(response); 
      //   },
      //   error => {      
      //     //debugger;     
      //     alert(error.message + "\n Unable to create floor");
      //     //this.clicked=false;
      //     console.log(error); 
      //   });
    }

  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

}
