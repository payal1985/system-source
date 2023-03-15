import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatRippleModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatGridListModule } from '@angular/material/grid-list';
import {MatChipsModule} from '@angular/material/chips';
import {MatSelectModule} from '@angular/material/select';
import {MatDialogModule} from '@angular/material/dialog';
import {MatTooltipModule} from '@angular/material/tooltip';
import { NgImageSliderModule } from 'ng-image-slider';
import { MatTableModule } from '@angular/material/table';
// import { MatSliderModule } from '@angular/material/slider';
// import { MatSlideToggleModule } from '@angular/material/slide-toggle';
// import { MatCarouselModule } from '@ngmodule/material-carousel';
import {InfiniteScrollModule} from 'ngx-infinite-scroll';
import {MatSortModule} from '@angular/material/sort';

const modules = [
  CommonModule,    
  FlexLayoutModule,
  MatToolbarModule,
  MatIconModule,
  MatMenuModule,
  MatButtonModule,
  MatRippleModule,
  MatFormFieldModule,
  MatInputModule,
  MatCardModule,
  MatSidenavModule,
  MatListModule,
  MatGridListModule,
  MatChipsModule,
  MatSelectModule,
  MatDialogModule,
  MatTooltipModule,
  NgImageSliderModule,
  MatTableModule,
  InfiniteScrollModule,MatSortModule
  // MatSliderModule,
  // MatSlideToggleModule,
  // MatCarouselModule

];

@NgModule({
  declarations: [],
  imports: [
    modules
  ],
  exports: [
  modules
  ]
})
export class MaterialModule { }
