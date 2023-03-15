import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';
import { DonationsComponent } from './components/donations/donations.component';
import { OrderComponent } from './components/order/order.component';
import { LoginComponent } from './components/login/login.component';
//import { AdminComponent } from './components/admin/admin.component';
import { AuthGuard } from './guards/auth.guard';
import { Role } from './models/role';

const routes: Routes = [
  {path: '', component: LoginComponent },
  {path: 'index', component: DashboardComponent, canActivate: [AuthGuard]},
   {path:'donations',component:DonationsComponent, canActivate: [AuthGuard]},
   {path:'orders',component:OrderComponent, canActivate: [AuthGuard],data: { roles: [Role.Admin]}},
   //{path:'orders',component:OrderComponent},
  // {path:'admin',component:AdminComponent, canActivate: [AuthGuard] },
   {path: 'login', component: LoginComponent },
  // {path: '', component: MainNavComponent},
  {path: '**', component: PageNotFoundComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
