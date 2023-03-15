import { Component, OnInit,EventEmitter,Output } from '@angular/core';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  @Output() eventClicked = new EventEmitter<string>();
  events = [
    "one","two","three"
  ];

  //More app code

  onClick(event: string): void {
    this.eventClicked.emit(event);
  }

}
