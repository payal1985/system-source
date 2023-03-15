import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExcelexportComponent } from './excelexport.component';

describe('ExcelexportComponent', () => {
  let component: ExcelexportComponent;
  let fixture: ComponentFixture<ExcelexportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExcelexportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExcelexportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
