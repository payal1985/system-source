import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WarrentyRequestDilogComponent } from './warrenty-request-dilog.component';

describe('WarrentyRequestDilogComponent', () => {
  let component: WarrentyRequestDilogComponent;
  let fixture: ComponentFixture<WarrentyRequestDilogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WarrentyRequestDilogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(WarrentyRequestDilogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
