import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from "rxjs/Observable";
import { User } from './../authentication/models/user';
import { AuthenticationService } from '../authentication/services/authentication.service';

@Injectable()
export class AnonymousGuard implements CanActivate {
    constructor(private router : Router, private authService: AuthenticationService) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
        if (this.authService.isAuthenticated()) {
            this.router.navigate(['/'])
            return false;
        }

        return true;
    }

}