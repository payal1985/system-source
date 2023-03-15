import { Component, OnInit, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import {MatDatepickerInputEvent, MatDatepickerInput} from '@angular/material/datepicker';


@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})
export class DialogComponent implements OnInit {

  @Input() disabled: boolean;
  @Input() matDatepicker: boolean;
  @Input() max: null;

   testdate = new FormControl('',[]);
  
  //testdate:Date;

  validDates = {
    // "2018-01-01T08:00:00": true,
    // "2018-01-02T08:00:00": true
    "2018-11-22T07:00:00.000Z": true
  }

  constructor() { }
  
  ngOnInit() {}
  
  events: string[] = [];

  addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
    this.events.push(`${type}: ${event.value}`);
  }

  // Solution: https://stackoverflow.com/questions/48546547/how-to-enable-only-specific-dates-in-angular-5
  
  myFilter = (d: Date): boolean => {
    console.log(d);
    // Using a JS Object as a lookup table of valid dates
    // Undefined will be falsy.
    return this.validDates[d.toISOString()];
  }

}
