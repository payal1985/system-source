import { Component, OnInit,Input } from '@angular/core';

@Component({
  selector: 'app-eventdetails',
  templateUrl: './eventdetails.component.html',
  styleUrls: ['./eventdetails.component.css']
})
export class EventdetailsComponent implements OnInit {

  @Input() event!: string;

  constructor() { }

  ngOnInit(): void {
  }

}
