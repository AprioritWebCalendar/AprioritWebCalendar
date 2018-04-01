import { Injectable } from '@angular/core';
import { Http, ConnectionBackend, RequestOptions } from '@angular/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class CustomHttp extends Http {
    constructor(backend: ConnectionBackend, defaultOptions: RequestOptions, private router: Router){
        super(backend, defaultOptions);
    }

    configureToken(token: string) {
        if (token == null)
            return;

        this._defaultOptions.headers.set("Authorization", `Bearer ${token}`);
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
