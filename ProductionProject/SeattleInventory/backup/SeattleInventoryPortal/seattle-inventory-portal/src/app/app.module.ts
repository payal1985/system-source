import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';
import { HeaderComponent } from './components/header/header.component';
import { ContentComponent } from './components/content/content.component';
import { HttpClientModule } from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { SideNavContentComponent } from './components/side-nav-content/side-nav-content.component';
import { MainContentComponent } from './components/main-content/main-content.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { EventsComponent } from './events/events.component';
import { EventdetailsComponent } from './eventdetails/eventdetails.component';
import { NewdashboardComponent } from './components/newdashboard/newdashboard.component';
import { HomeComponent } from './components/home/home.component';
import { FlexLayoutContentComponent } from './components/flex-layout-content/flex-layout-content.component';
import { FlexLayoutHeaderComponent } from './components/flex-layout-header/flex-layout-header.component';
import { AddCartDialogComponent } from './components/dashboard/add-cart-dialog/add-cart-dialog.component';
import { ImageCarouselDialogComponent } from './components/dashboard/image-carousel-dialog/image-carousel-dialog.component';
import { CartDialogComponent } from './components/dashboard/cart-dialog/cart-dialog.component';
import { SendorderDialogComponent } from './components/dashboard/sendorder-dialog/sendorder-dialog.component';
import { MainNavComponent } from './components/main-nav/main-nav.component';
import { DatePipe } from '@angular/common';
import {MatNativeDateModule} from '@angular/material/core';


//import { DateFormat } from './shared/date-format';
// import { CalendarModule, DateAdapter } from 'angular-calendar';
// import { adapterFactory } from 'angular-calendar/date-adapters/date-fns'; 


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    ContentComponent,
    SideNavContentComponent,
    MainContentComponent,
    DashboardComponent,
    PageNotFoundComponent,
    EventsComponent,
    EventdetailsComponent,
    NewdashboardComponent,
    HomeComponent,
    FlexLayoutContentComponent,
    FlexLayoutHeaderComponent,
    AddCartDialogComponent,
    ImageCarouselDialogComponent,
    CartDialogComponent,
    SendorderDialogComponent,
    MainNavComponent,
    
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MaterialModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    MatNativeDateModule
  ],
  providers: [DatePipe
   // { provide: DateAdapter, useClass: DateFormat }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  // constructor(private dateAdapter: DateAdapter<Date>) {
  //   dateAdapter.setLocale("en-US"); // MM/DD/YYYY
  // }
}
