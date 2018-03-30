import { Injectable, OnInit } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import "rxjs/add/operator/map";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { User } from "./../models/user";
import { ResponseExceptionHandler } from "../../infrastructure/responseExceptionHandler";

@Injectable()
export class AuthenticationService implements OnInit {

    private currentUser : User;
    private token : string;

    constructor(private http : Http) {}

    ngOnInit(): void {
        this.currentUser = JSON.parse(sessionStorage.getItem("user"));
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
        this.currentUser = null;
    }
}