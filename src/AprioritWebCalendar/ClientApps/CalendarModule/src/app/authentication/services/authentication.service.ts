import { Injectable } from "@angular/core";
import { Http, Response, RequestOptionsArgs, Headers, RequestOptions } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import "rxjs/add/operator/map";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { User } from "./../models/user";
import { CustomHttp } from "../../services/custom.http";
import { Router } from "@angular/router";

@Injectable()
export class AuthenticationService {
    private currentUser : User;

    constructor(
        private http : Http, 
        private customHttp : CustomHttp,
        private router: Router
    ) {
        if (customHttp.tokenExists()) {
            this.getUser()
                .subscribe((response: User) => {
                    this.currentUser = response;
                    console.log("The current user has been got");
                });
        }
    }

    isAuthenticated() : boolean {
        return this.currentUser != null;
    }

    getCurrentUser() : User {
        return this.currentUser;
    }

    login(emailOrUserName : string, password : string) {
        return this.http.post("/api/Account/Login", { EmailOrUserName : emailOrUserName, Password : password})
            .map((response: Response) => {
                this.currentUser = response.json()["User"];
                var token = response.json()["AccessToken"];

                sessionStorage.setItem("user", JSON.stringify(this.currentUser));

                console.log("The token has been saved");

                this.customHttp.configureToken(token);
                return this.currentUser;
            })
            .catch(e => {
                return Observable.throw(e);
            });
    }

    register(email : string, userName : string, password : string) {
        return this.http.post("/api/Account/Register", { Email: email, UserName: userName, Password : password})
            .map((response: Response) => {
                
            })
            .catch(e => {
                return Observable.throw(e);
            });
    }

    logout() {
        this.customHttp.resetToken();
        sessionStorage.removeItem("user");
        this.currentUser = null;
        this.router.navigate(['/login']);
    }

    getUser() {
        return this.customHttp.get("/api/Account")
            .map((response:Response) => {
                return response.json() as User;
            })
            .catch(e => {
              return Observable.throw(e);  
            });
    }
}