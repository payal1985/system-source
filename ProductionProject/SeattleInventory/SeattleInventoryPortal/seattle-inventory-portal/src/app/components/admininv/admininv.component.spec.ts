import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdmininvComponent } from './admininv.component';

describe('AdmininvComponent', () => {
  let component: AdmininvComponent;
  let fixture: ComponentFixture<AdmininvComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdmininvComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdmininvComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
