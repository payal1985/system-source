import { Component, Inject, OnInit, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { client } from 'src/app/model/client';

@Component({
  selector: 'app-add-client-dialog',
  templateUrl: './add-client-dialog.component.html',
  styleUrls: ['./add-client-dialog.component.css']
})
export class AddClientDialogComponent implements OnInit {

  action:string;
  local_data:any;

  constructor(public dialogRef: MatDialogRef<AddClientDialogComponent>,
    //@Optional() is used to prevent error if no data is passed
    @Optional() @Inject(MAT_DIALOG_DATA) public data: client) {
    console.log(data);
    this.local_data = {...data};
    this.action = this.local_data.action;
  }

  ngOnInit(): void {
  }

  doAction(){   
    this.dialogRef.close({event:this.action,data:this.local_data});
  }

  closeDialog(){
    this.dialogRef.close({event:'Cancel'});
  }

  // onChange(event){
  //   alert(event.checked);
  //   console.log('onChange event.checked '+event.checked);
  // }
}
