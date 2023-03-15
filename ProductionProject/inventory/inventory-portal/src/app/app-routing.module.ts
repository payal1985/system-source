import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddCartComponent } from './components/add-cart/add-cart.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DropzoneDialogComponent } from './components/dropzone-dialog/dropzone-dialog.component';
import { ExcelexportComponent } from './components/excelexport/excelexport.component';
import { HomeComponent } from './components/home/home.component';
import { ImageSliderComponent } from './components/image-slider/image-slider.component';
import { ImageComponent } from './components/image/image.component';
import { InlineEditTableDialogComponent } from './components/inline-edit-table-dialog/inline-edit-table-dialog.component';
import { NestedTableComponent } from './components/nested-table/nested-table.component';
import { PdfexportComponent } from './components/pdfexport/pdfexport.component';
import { ScrollableComponent } from './components/scrollable/scrollable.component';
import { TableExpandableRowsExampleComponent } from './components/table-expandable-rows-example/table-expandable-rows-example.component';

const routes: Routes = [
  {path: '', component: DashboardComponent},
  {path:'home',component:HomeComponent},
  {path:'image',component:ImageSliderComponent},
  {path:'excel',component:ExcelexportComponent},
  {path:'pdf',component:PdfexportComponent},
  {path:'imgtest',component:ImageComponent},
  {path:'dropzone',component:DropzoneDialogComponent},
  {path:'nested',component:NestedTableComponent},
  {path:'inlineedit',component:InlineEditTableDialogComponent},
  {path:'addcart',component:AddCartComponent},
  {path:'scroll',component:ScrollableComponent},
  {path:'expandablerows',component:TableExpandableRowsExampleComponent}
  

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
