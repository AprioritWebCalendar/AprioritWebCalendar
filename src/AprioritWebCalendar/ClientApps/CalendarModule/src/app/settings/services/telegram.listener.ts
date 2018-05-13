import { Injectable } from "@angular/core";
import { HubListener } from "../../services/hub.listener";

@Injectable()
export class TelegramListener extends HubListener {
    constructor() {
        super("telegram");
    }

    public OnTelegramReseted(callback: () => void) : void {
        this._connection.off("telegramReseted");
        this._connection.on("telegramReseted", () => callback());
    }

    public OnNotificationsDisabled(callback: () => void) : void {
        this._connection.off("telegramNotificationsDisabled");
        this._connection.on("telegramNotificationsDisabled", () => callback());
    }

    public OnNotificationsEnabled(callback: () => void) : void {
        this._connection.off("telegramNotificationsEnabled");
        this._connection.on("telegramNotificationsEnabled", () => callback());
    }
}