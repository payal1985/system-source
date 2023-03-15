import { Component, OnInit,Input } from '@angular/core';

@Component({
  selector: 'app-flex-layout-header',
  templateUrl: './flex-layout-header.component.html',
  styleUrls: ['./flex-layout-header.component.css']
})
export class FlexLayoutHeaderComponent implements OnInit {

  @Input() deviceXs!: boolean;

  constructor() { }

  ngOnInit(): void {
  }

}
