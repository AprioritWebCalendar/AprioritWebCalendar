import { CustomHttp } from "../services/custom.http";
import { Injectable } from "@angular/core";
import { HubConnection, IHubConnectionOptions } from '@aspnet/signalr';

@Injectable()
export class NotificationListener {
    constructor(
        private customHttp: CustomHttp
    ) {

    }

    public Start() : void {
        let connection = new HubConnection("http://localhost:65067/notification-hub?token=" + this.customHttp.getTokenString());
        connection.start();
        console.log("The NotificationListener is running...");
    }
}