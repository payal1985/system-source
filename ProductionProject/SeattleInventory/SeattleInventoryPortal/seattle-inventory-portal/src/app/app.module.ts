import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';

import { HttpClientModule } from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';

import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

import { AddCartDialogComponent } from './components/dashboard/add-cart-dialog/add-cart-dialog.component';
import { ImageCarouselDialogComponent } from './components/dashboard/image-carousel-dialog/image-carousel-dialog.component';
import { CartDialogComponent } from './components/dashboard/cart-dialog/cart-dialog.component';
import { SendorderDialogComponent } from './components/dashboard/sendorder-dialog/sendorder-dialog.component';

import { DatePipe } from '@angular/common';
import {MatNativeDateModule} from '@angular/material/core';
import { MatDialogRef } from '@angular/material/dialog';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DonationsComponent } from './components/donations/donations.component';
import { ImageSliderComponent } from './components/donations/image-slider/image-slider.component';
import { OrdersComponent } from './components/orders/orders.component';

import { LoginComponent } from './components/login/login.component';
import { AdmininvComponent } from './components/admininv/admininv.component';
import { AuthGuard } from './guards/auth.guard';
import { AddClientDialogComponent } from './components/admininv/add-client-dialog/add-client-dialog.component';
import { SecuredImageComponent } from './components/dashboard/image-carousel-dialog/secured-image.component';
import { FilterPipe }  from './components/dashboard/filter.pipe';
import { OrderCartDialogComponent } from './components/dashboard/order-cart-dialog/order-cart-dialog.component';


//import { DateFormat } from './shared/date-format';
// import { CalendarModule, DateAdapter } from 'angular-calendar';
// import { adapterFactory } from 'angular-calendar/date-adapters/date-fns'; 


@NgModule({
  declarations: [
    AppComponent,

    DashboardComponent,
    PageNotFoundComponent,
 
    AddCartDialogComponent,
    ImageCarouselDialogComponent,
    CartDialogComponent,
    SendorderDialogComponent,
    DonationsComponent,
    ImageSliderComponent,
    OrdersComponent,
   
    LoginComponent,
    AdmininvComponent,
    AddClientDialogComponent,
    SecuredImageComponent,
    FilterPipe,
    OrderCartDialogComponent
    
    
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MatNativeDateModule,
    NgbModule
  ],
  providers: [DatePipe,AuthGuard
    //,MatDialogRef
   // { provide: DateAdapter, useClass: DateFormat }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  // constructor(private dateAdapter: DateAdapter<Date>) {
  //   dateAdapter.setLocale("en-US"); // MM/DD/YYYY
  // }
}
