import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendorderDialogComponent } from './sendorder-dialog.component';

describe('SendorderDialogComponent', () => {
  let component: SendorderDialogComponent;
  let fixture: ComponentFixture<SendorderDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SendorderDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SendorderDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
