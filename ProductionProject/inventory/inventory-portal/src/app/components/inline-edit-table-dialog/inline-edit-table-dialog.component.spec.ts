import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InlineEditTableDialogComponent } from './inline-edit-table-dialog.component';

describe('InlineEditTableDialogComponent', () => {
  let component: InlineEditTableDialogComponent;
  let fixture: ComponentFixture<InlineEditTableDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InlineEditTableDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InlineEditTableDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
