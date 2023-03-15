import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderCartDialogComponent } from './order-cart-dialog.component';

describe('OrderCartDialogComponent', () => {
  let component: OrderCartDialogComponent;
  let fixture: ComponentFixture<OrderCartDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderCartDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderCartDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
