import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PdfexportComponent } from './pdfexport.component';

describe('PdfexportComponent', () => {
  let component: PdfexportComponent;
  let fixture: ComponentFixture<PdfexportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PdfexportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PdfexportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
