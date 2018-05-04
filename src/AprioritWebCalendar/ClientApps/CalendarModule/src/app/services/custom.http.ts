import { Injectable } from '@angular/core';
import { Http, ConnectionBackend, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CustomHttp extends Http {
    private token: string;

    constructor(backend: ConnectionBackend, defaultOptions: RequestOptions, private router: Router){
        super(backend, defaultOptions);
    }

    public configureToken(token: string) {
        if (token == null)
            return;

        this.token = token;
        sessionStorage.setItem("token", token);
        this.attachToken(this._defaultOptions);
        console.log("The CustomHttp has been configured.");
    }

    public resetToken() {
        this.token = null;
        sessionStorage.removeItem("token");
        this._defaultOptions.headers.delete("Authorization");
    }

    public getToken() {
        return new Promise((resolve, reject) => {
            var token = sessionStorage.getItem("token");

            if (token == null) {
                console.log("The token wasn't found");
                return;
            }
    
            this.configureToken(token);
            console.log("The token has been found"); 
        });
    }

    public getTokenString() : string {
        return this.token;
    }

    public attachToken(options: RequestOptions) {
        options.headers.set("Authorization", `Bearer ${this.token}`);
    }

    public tokenExists() : boolean {
        return this.token != null;
    }

    processError(error: any) : Observable<any> {
        switch (error.status) {
            case 401:
                this.router.navigate(['login']);
                break;
        }

        return Observable.throw(error);
    }
}
