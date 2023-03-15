import { Component, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-dialog',
  templateUrl: './add-dialog.component.html',
  styleUrls: ['./add-dialog.component.scss']
})
export class AddDialogComponent implements OnInit {

  typeFormControl = new FormControl('', [Validators.required]);

  constructor(public dialogRef: MatDialogRef<AddDialogComponent>) { }

  ngOnInit(): void {
  }

  onAddType() {
    debugger;
    // Close the dialog, return true
    this.dialogRef.close(true);

  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

}
