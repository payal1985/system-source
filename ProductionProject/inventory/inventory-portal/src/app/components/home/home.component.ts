import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ThemePalette } from '@angular/material/core';
import {DateAdapter} from '@angular/material/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  @ViewChild('picker') picker: any;

   public date: Date;
  public disabled = false;
  public showSpinners = true;
  public showSeconds = false;
  public touchUi = false;
  public enableMeridian = false;
  // public minDate: moment.Moment;
  // public maxDate: moment.Moment;
  public stepHour = 1;
  public stepMinute = 1;
  public stepSecond = 1;
  //public color: ThemePalette = 'primary';

  public formGroup = new FormGroup({
    date: new FormControl(null, [Validators.required]),
    date2: new FormControl(null, [Validators.required])
  })
  public dateControl = new FormControl(new Date(2021,9,4,5,6,7));
  public dateControlMinMax = new FormControl(new Date());

//   public options = [
//     { value: true, label: 'True' },
//     { value: false, label: 'False' }
//   ];

//   public listColors = ['primary', 'accent', 'warn'];

//   public stepHours = [1, 2, 3, 4, 5];
//   public stepMinutes = [1, 5, 10, 15, 20, 25];
//   public stepSeconds = [1, 5, 10, 15, 20, 25];

//   public codeDatePicker = `
// <mat-form-field>
//   <input matInput [ngxMatDatetimePicker]="picker" 
//                   placeholder="Choose a date" 
//                   [formControl]="dateControl"
//                   [min]="minDate" [max]="maxDate" 
//                   [disabled]="disabled">
//   <mat-datepicker-toggle matSuffix [for]="picker">
//   </mat-datepicker-toggle>
//   <ngx-mat-datetime-picker #picker 
//     [showSpinners]="showSpinners" 
//     [showSeconds]="showSeconds"
//     [stepHour]="stepHour" [stepMinute]="stepMinute" 
//     [stepSecond]="stepSecond"
//     [touchUi]="touchUi"
//     [color]="color">
//   </ngx-mat-datetime-picker>
// </mat-form-field>`;




 

//   public code1 = `formGroup.get('date').value?.toLocaleString()`;

//   public codeFormGroup2 = `
//   <form [formGroup]="formGroup">
//     <ngx-mat-timepicker formControlName="date2"></ngx-mat-timepicker>
//   </form>`;

//   public code2 = `formGroup.get('date2').value?.toLocaleString()`;

  constructor() {
  }

  ngOnInit() {
    //this.date = new Date(2021,9,4,5,6,7);
  }

  // toggleMinDate(evt: any) {
  //   if (evt.checked) {
  //     this._setMinDate();
  //   } else {
  //     this.minDate = null;
  //   }
  // }

  // toggleMaxDate(evt: any) {
  //   if (evt.checked) {
  //     this._setMaxDate();
  //   } else {
  //     this.maxDate = null;
  //   }
  // }

  closePicker() {
    this.picker.cancel();
  }

  // private _setMinDate() {
  //   const now = new Date();
  //   this.minDate = new Date();
  //   this.minDate.setDate(now.getDate() - 1);
  // }


  // private _setMaxDate() {
  //   const now = new Date();
  //   this.maxDate = new Date();
  //   this.maxDate.setDate(now.getDate() + 1);
  //}

}

