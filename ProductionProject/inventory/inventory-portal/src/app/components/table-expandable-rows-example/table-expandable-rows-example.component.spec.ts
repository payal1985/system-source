import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableExpandableRowsExampleComponent } from './table-expandable-rows-example.component';

describe('TableExpandableRowsExampleComponent', () => {
  let component: TableExpandableRowsExampleComponent;
  let fixture: ComponentFixture<TableExpandableRowsExampleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TableExpandableRowsExampleComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TableExpandableRowsExampleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
