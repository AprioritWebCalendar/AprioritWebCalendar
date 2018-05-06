import { Injectable } from '@angular/core';
import { Http, ConnectionBackend, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CustomHttp extends Http {
    private token: string;

    constructor(backend: ConnectionBackend, defaultOptions: RequestOptions){
        super(backend, defaultOptions);
    }

    public ConfigureToken(token: string) {
        if (token == null)
            return;

        this.token = token;
        sessionStorage.setItem("token", token);
        this.AttachToken(this._defaultOptions);
        console.log("The CustomHttp has been configured.");
    }

    public ResetToken() {
        this.token = null;
        sessionStorage.removeItem("token");
        this._defaultOptions.headers.delete("Authorization");
    }

    public InitializeToken() {
        return new Promise((resolve, reject) => {
            var token = sessionStorage.getItem("token");

            if (token == null) {
                console.log("The token wasn't found");
                return;
            }
    
            this.ConfigureToken(token);
            console.log("The token has been found"); 
        });
    }

    public GetTokenString() : string {
        return this.token;
    }

    public AttachToken(options: RequestOptions) {
        options.headers.set("Authorization", `Bearer ${this.token}`);
    }

    public TokenExists() : boolean {
        return this.token != null;
    }
}
