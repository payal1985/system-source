import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageCarouselDialogComponent } from './image-carousel-dialog.component';

describe('ImageCarouselDialogComponent', () => {
  let component: ImageCarouselDialogComponent;
  let fixture: ComponentFixture<ImageCarouselDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImageCarouselDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageCarouselDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
