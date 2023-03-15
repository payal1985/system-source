import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MaterialModule } from './material/material.module';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { DonationsComponent } from './components/donations/donations.component';
import { LoginComponent } from './components/login/login.component';
import { OrderComponent } from './components/order/order.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

import { CartDialogComponent } from './components/dashboard/cart-dialog/cart-dialog.component';
import { ImageCarouselDialogComponent } from './components/dashboard/image-carousel-dialog/image-carousel-dialog.component';

import { ImageSliderDialogComponent } from './components/donations/image-slider-dialog/image-slider-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { DatePipe } from '@angular/common';
import {MatNativeDateModule} from '@angular/material/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FilterPipe }  from './components/dashboard/filter.pipe';
import { AuthGuard } from './guards/auth.guard';
import { OrderCartDialogComponent } from './components/dashboard/order-cart-dialog/order-cart-dialog.component';
import { SpinnerComponent } from './features/spinner/spinner.component';
import { SpinnerInterceptor } from './core/http-inteceptor/spinner.interceptor';
import { AddCartDialogComponent } from './components/dashboard/add-cart-dialog/add-cart-dialog.component';
import { AddDialogComponent } from './components/dashboard/cart-dialog/add-dialog/add-dialog.component';
import { WarrentyRequestDilogComponent } from './components/dashboard/warrenty-request-dilog/warrenty-request-dilog.component';
import { WarrantyRequestDialogComponent } from './components/dashboard/warranty-request-dialog/warranty-request-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,   
    DonationsComponent,
    LoginComponent,
    OrderComponent,
    PageNotFoundComponent,
  
    CartDialogComponent,
    ImageCarouselDialogComponent,
   
    ImageSliderDialogComponent,
    FilterPipe,
    OrderCartDialogComponent,
    SpinnerComponent,
    AddCartDialogComponent,
    AddDialogComponent,
    WarrentyRequestDilogComponent,
    WarrantyRequestDialogComponent

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MatNativeDateModule,
    NgbModule
  ],
  providers: [DatePipe,AuthGuard,
  {provide: HTTP_INTERCEPTORS, useClass: SpinnerInterceptor,multi:true}
],
  bootstrap: [AppComponent]
})
export class AppModule { }
