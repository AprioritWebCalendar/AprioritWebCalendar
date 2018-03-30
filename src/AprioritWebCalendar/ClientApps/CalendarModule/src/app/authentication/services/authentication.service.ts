import { Injectable, OnInit } from "@angular/core";
import { Http, Response, RequestOptionsArgs, Headers, RequestOptions } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import "rxjs/add/operator/map";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { User } from "./../models/user";

@Injectable()
export class AuthenticationService {

    private currentUser : User;
    private token : string;

    constructor(private http : Http) {
        var token = sessionStorage.getItem("token");

        if (token == null)
            return;

        this.getUser(token)
            .subscribe((response : User) => {
                this.token = token;
                this.currentUser = response;
            });
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
                this.token = response.json()["AccessToken"];

                sessionStorage.setItem("user", JSON.stringify(this.currentUser));
                sessionStorage.setItem("token", this.token);

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
        sessionStorage.removeItem("user");
        sessionStorage.removeItem("token");
        this.currentUser = null;
        this.token = null;
    }

    getUser(token: string) {
        var options = new RequestOptions({
            headers: new Headers({"Authorization" : "Bearer " + token })
        })

        return this.http.get("/api/Account", options)
            .map((response:Response) => {
                return response.json() as User;
            })
            .catch(e => {
              return Observable.throw(e);  
            });
    }
}