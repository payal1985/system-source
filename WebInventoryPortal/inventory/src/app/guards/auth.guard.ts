import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { LoginService } from '../services/login.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

    constructor(private router: Router, private loginService:LoginService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
//debugger;
        const currentUser = this.loginService.currentUserValue;
        if (currentUser) {
            // check if route is restricted by role
            if (route.data['roles'] && route.data['roles'].indexOf(currentUser.role) === -1) {
               // if (route.data.roles && route.data.roles.indexOf(currentUser.role) === -1) {
                // role not authorised so redirect to home page
                this.router.navigate(['/']);
                return false;
            }

            // authorised so return true
            return true;
        }

        // if (this.loginService.isLoggedIn()) {
        //    // debugger;
        //     return true; 
        // }

        // not logged in so redirect to login page with the return url
        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url }});
        return false;
    }
}