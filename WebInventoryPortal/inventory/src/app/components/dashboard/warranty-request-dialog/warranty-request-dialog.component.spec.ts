import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarrantyRequestDialogComponent } from './warranty-request-dialog.component';

describe('WarrantyRequestDialogComponent', () => {
  let component: WarrantyRequestDialogComponent;
  let fixture: ComponentFixture<WarrantyRequestDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarrantyRequestDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarrantyRequestDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
