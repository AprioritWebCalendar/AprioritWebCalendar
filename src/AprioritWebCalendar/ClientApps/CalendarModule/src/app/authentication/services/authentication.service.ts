import { Injectable } from "@angular/core";
import { Http, Response, RequestOptionsArgs, Headers, RequestOptions } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import "rxjs/add/operator/map";
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

import { User } from "./../models/user";
import { CustomHttp } from "../../services/custom.http";
import { Router } from "@angular/router";
import { CalendarListener } from "../../calendar/services/calendar.listener";
import { InvitationListener } from "../../invitation/services/invitation.listener";
import { NotificationListener } from "../../notification/notification.listener";

@Injectable()
export class AuthenticationService {
    private currentUser : User;

    constructor(
        private http : Http, 
        private customHttp : CustomHttp,
        private router: Router,
        private calendarListener: CalendarListener,
        private invitationListener: InvitationListener,
        private notificationListener: NotificationListener
    ) {
    }

    public InitializeUser() : void {
        if (this.customHttp.TokenExists()) {
            this.GetUser()
                .subscribe((response: User) => {
                    this.currentUser = response;
                    console.log("The current user has been got");
                    this.InitializeListeners(this.customHttp.GetTokenString());
                    
                    this.router.navigate(['/']);
                }, e => {
                    this.Logout();
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

    public SetUserTimeZone(timeZone: string) : void {
        this.currentUser.TimeZone = timeZone;
    }

    public SetTelegramId(telegramId: number) : void {
        this.currentUser.TelegramId = telegramId;
        this.currentUser.IsTelegramNotificationEnabled = true;
    }

    public ResetTelegram() : void {
        this.currentUser.TelegramId = null;
        this.currentUser.IsTelegramNotificationEnabled = null;
    }

    public SetTelegramNotificationsEnabled(isEnabled: boolean) : void {
        this.currentUser.IsTelegramNotificationEnabled = isEnabled;
    }

    public Login(emailOrUserName : string, password : string) : Observable<boolean> {
        return this.http.post("/api/Account/Login", { EmailOrUserName : emailOrUserName, Password : password})
            .map((response: Response) => {
                var token = response.json().AccessToken;

                console.log("The token has been saved");

                this.customHttp.ConfigureToken(token);
                this.currentUser = response.json().User;
                this.InitializeListeners(token);
                return true;
            })
            .catch(e => {
                return Observable.throw(e);
            });
    }

    public Register(email : string, userName : string, password : string, timeZone: string) : Observable<boolean> {
        return this.http.post("/api/Account/Register", { Email: email, UserName: userName, Password : password, TimeZone: timeZone })
            .map((response: Response) => {
                return true;
            })
            .catch(e => {
                return Observable.throw(e);
            });
    }

    public Logout() : void {
        this.calendarListener.Stop();
        this.invitationListener.Stop();
        this.notificationListener.Stop();

        this.customHttp.ResetToken();
        this.currentUser = null;
        this.router.navigate(['/auth/login']);
    }

    private GetUser() : Observable<User> {
        return this.customHttp.get("/api/Account")
            .map((response:Response) => {
                return response.json() as User;
            })
            .catch(e => {
              return Observable.throw(e);  
            });
    }

    private InitializeListeners(token: string) : void {
        this.calendarListener.Initialize(token);
        this.invitationListener.Initialize(token);
        this.notificationListener.Initialize(token);
    }
}