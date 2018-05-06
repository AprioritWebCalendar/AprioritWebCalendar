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
    }

    public InitializeUser() : void {
        if (this.customHttp.TokenExists()) {
            this.getUser()
                .subscribe((response: User) => {
                    this.currentUser = response;
                    console.log("The current user has been got");
                    this.router.navigate(['/']);
                });
        } else {
            console.log("AuthenticationService: unable to get token.");
        }
    }

    public IsAuthenticated() : boolean {
        return this.currentUser != null;
    }

    public GetCurrentUser() : User {
        return this.currentUser;
    }

    public Login(emailOrUserName : string, password : string) : Observable<boolean> {
        return this.http.post("/api/Account/Login", { EmailOrUserName : emailOrUserName, Password : password})
            .map((response: Response) => {
                var token = response.json().AccessToken;

                console.log("The token has been saved");

                this.customHttp.ConfigureToken(token);
                this.currentUser = response.json().User;
                return true;
            })
            .catch(e => {
                return Observable.throw(e);
            });
    }

    public Register(email : string, userName : string, password : string) : Observable<boolean> {
        return this.http.post("/api/Account/Register", { Email: email, UserName: userName, Password : password})
            .map((response: Response) => {
                return true;
            })
            .catch(e => {
                return Observable.throw(e);
            });
    }

    public Logout() : void {
        this.customHttp.ResetToken();
        this.currentUser = null;
        this.router.navigate(['/auth/login']);
    }

    private getUser() : Observable<User> {
        return this.customHttp.get("/api/Account")
            .map((response:Response) => {
                return response.json() as User;
            })
            .catch(e => {
              return Observable.throw(e);  
            });
    }
}