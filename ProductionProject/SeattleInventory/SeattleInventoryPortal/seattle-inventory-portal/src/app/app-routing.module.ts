import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
 import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { DonationsComponent } from './components/donations/donations.component';
import { OrdersComponent } from './components/orders/orders.component';
import { LoginComponent } from './components/login/login.component';
import { AdmininvComponent } from './components/admininv/admininv.component';
import { AuthGuard } from './guards/auth.guard';
import { Role } from './guards';

//import { MainNavComponent } from './components/main-nav/main-nav.component';


const routes: Routes = [
  {path: '', component: LoginComponent },
  {path: 'index', component: DashboardComponent, canActivate: [AuthGuard]},
   {path:'donations',component:DonationsComponent},
   {path:'orders',component:OrdersComponent, canActivate: [AuthGuard],data: { roles: [Role.Admin]}},
   {path:'admin',component:AdmininvComponent, canActivate: [AuthGuard] },
   {path: 'login', component: LoginComponent },
  // {path: '', component: MainNavComponent},
  {path: '**', component: PageNotFoundComponent},
];


@NgModule({
  declarations: [],
  imports: [RouterModule.forRoot(routes,{ onSameUrlNavigation: 'reload' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
