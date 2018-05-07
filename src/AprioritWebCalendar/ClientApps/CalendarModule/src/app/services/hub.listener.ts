import { CustomHttp } from "./custom.http";
import { HubConnection } from "@aspnet/signalr";

export abstract class HubListener {
    constructor(private _hubName: string) {
    }

    protected _connection: HubConnection;

    public Initialize(token: string) : void {
        this._connection = new HubConnection(`/hub/${this._hubName}?token=${token}`);
        console.log(`The listener of ${this._hubName} hub has been initialized.`);
    }

    public Start() : void {
        this._connection.start();
        console.log(`The listener of ${this._hubName} hub is running.`);
    }

    public Stop() : void {
        this._connection.stop();
        console.log(`The listener of ${this._hubName} hub has been stopped.`);
    }
}