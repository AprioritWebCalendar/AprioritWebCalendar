import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from "rxjs/Observable";
import { User } from './../authentication/models/user';

@Injectable()
export class AnonymousGuard implements CanActivate {
    constructor(private router : Router) {}

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
        try {
            let user : User = JSON.parse(sessionStorage.getItem("user"));
            let token : string = sessionStorage.getItem("token");

            if (user != null || token != null)
                throw new Error();

            return true;
        }
        catch(e) {
            this.router.navigate(['/'])
            return false;
        }
    }

}