import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageSliderDialogComponent } from './image-slider-dialog.component';

describe('ImageSliderDialogComponent', () => {
  let component: ImageSliderDialogComponent;
  let fixture: ComponentFixture<ImageSliderDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImageSliderDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageSliderDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
