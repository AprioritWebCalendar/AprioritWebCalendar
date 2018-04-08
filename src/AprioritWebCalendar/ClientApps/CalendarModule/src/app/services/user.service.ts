import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { CustomHttp } from "./custom.http";
import { User } from "../authentication/models/user";

@Injectable()
export class UserService {
    private baseUrl: string = "/api/User/";

    constructor(private http: CustomHttp){

    }

    public findUsersByEmailOrUserName(emailOrUserName: string) : Observable<User[]> {
        return this.http.get(this.baseUrl + emailOrUserName)
            .map(response => response.json())
            .catch(e => Observable.throw(e));
    }
}