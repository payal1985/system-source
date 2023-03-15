import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlexLayoutHeaderComponent } from './flex-layout-header.component';

describe('FlexLayoutHeaderComponent', () => {
  let component: FlexLayoutHeaderComponent;
  let fixture: ComponentFixture<FlexLayoutHeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FlexLayoutHeaderComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FlexLayoutHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
