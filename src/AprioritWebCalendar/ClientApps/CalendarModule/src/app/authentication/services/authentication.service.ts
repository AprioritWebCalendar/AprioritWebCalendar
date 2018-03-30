import { Injectable, OnInit } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import "rxjs/add/operator/map";

import { User } from "./../models/user";
import { ErrorArray } from "./../../infrastructure/errorArray";
import { ResponseExceptionHandler } from "../../infrastructure/responseExceptionHandler";

@Injectable()
export class AuthenticationService implements OnInit {

    private currentUser : User;

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
                ResponseExceptionHandler.throwExcepion(response);

                this.currentUser = response.json()["User"];
                sessionStorage.setItem("user", JSON.stringify(this.currentUser));
            });
    }

    register(email : string, userName : string, password : string) {
        return this.http.post("/api/Account/Register", { Email: email, UserName: userName, Password : password})
            .map((response : Response) => {
                ResponseExceptionHandler.throwExcepion(response);
            });
    }

    logout() {
        sessionStorage.removeItem("user");
        this.currentUser = null;
    }
}