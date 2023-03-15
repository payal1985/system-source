import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
 import { DashboardComponent } from './components/dashboard/dashboard.component';
 import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { NewdashboardComponent } from './components/newdashboard/newdashboard.component';
import { MainNavComponent } from './components/main-nav/main-nav.component';
// import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
   //{path: '', component: DashboardComponent},
   {path: '', component: MainNavComponent},
  //  {path: 'home', component: HomeComponent},
  //{path: '', component: NewdashboardComponent},
  {path: '**', component: PageNotFoundComponent},
];


@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes,{ onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
