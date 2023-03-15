import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';

import { AppRoutingModule } from './app-routing.module';
import { MaterialModule } from './material/material.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { DialogComponent } from './components/dialog/dialog.component';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import {MatNativeDateModule} from '@angular/material/core';
import { DatePipe } from '@angular/common';
import { NgxMatDatetimePickerModule,  NgxMatTimepickerModule } from '@angular-material-components/datetime-picker';
import { HomeComponent } from './components/home/home.component';
import { ImageSliderComponent } from './components/image-slider/image-slider.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ImageDialogComponent } from './components/image-dialog/image-dialog.component';
import { ExcelexportComponent } from './components/excelexport/excelexport.component';
import { PdfexportComponent } from './components/pdfexport/pdfexport.component';
import { ImageComponent } from './components/image/image.component';
//import { MatDatetimepickerModule, MatNativeDatetimeModule } from "@mat-datetimepicker/core";
import {SecuredImageComponent} from './components/image/secured-image.component';
import {MyHttpInterceptor} from './components/image/my-http.interceptor';
import { DropzoneDialogComponent } from './components/dropzone-dialog/dropzone-dialog.component';

import { NgxDropzoneModule } from 'ngx-dropzone';
import { NestedTableComponent } from './components/nested-table/nested-table.component';
import { InlineEditTableDialogComponent } from './components/inline-edit-table-dialog/inline-edit-table-dialog.component';
import { AddCartComponent } from './components/add-cart/add-cart.component';
import { AddDialogComponent } from './components/add-cart/add-dialog/add-dialog.component';
import { ScrollableComponent } from './components/scrollable/scrollable.component';
import { TableExpandableRowsExampleComponent } from './components/table-expandable-rows-example/table-expandable-rows-example.component';
//import {ScrollingModule} from '@angular/cdk/scrolling';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,

    DialogComponent,
     HomeComponent,
     ImageSliderComponent,
     ImageDialogComponent,
     ExcelexportComponent,
     PdfexportComponent,
     ImageComponent,SecuredImageComponent, DropzoneDialogComponent, NestedTableComponent, InlineEditTableDialogComponent, AddCartComponent, AddDialogComponent, ScrollableComponent, TableExpandableRowsExampleComponent
  ],
  imports: [
    BrowserModule,
    MaterialModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,   
    FormsModule,
    ReactiveFormsModule,
  MatDatepickerModule,//MatDatetimepickerModule,
  MatNativeDateModule,
    NgxMatDatetimePickerModule,
    NgxMatTimepickerModule,
    NgbModule,
    TableModule,
    DropdownModule,
    ButtonModule,
    NgxDropzoneModule
   // ScrollingModule
  ], 
  // providers: [DatePipe  ,MatDatepickerModule
  //   ,NgxMatDatetimePickerModule,NgxMatTimepickerModule
  // ],
  providers: [DatePipe,{
    provide: HTTP_INTERCEPTORS, useClass: MyHttpInterceptor, multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
