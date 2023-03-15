import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlexLayoutContentComponent } from './flex-layout-content.component';

describe('FlexLayoutContentComponent', () => {
  let component: FlexLayoutContentComponent;
  let fixture: ComponentFixture<FlexLayoutContentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FlexLayoutContentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FlexLayoutContentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
