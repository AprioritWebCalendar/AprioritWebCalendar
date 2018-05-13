import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../../authentication/services/authentication.service';
import { ToastsManager } from 'ng2-toastr';
import { NgForm } from '@angular/forms';
import { TelegramService } from '../../services/telegram.service';

@Component({
    selector: 'app-settings-telegram',
    templateUrl: './settings-telegram.component.html'
})
export class SettingsTelegramComponent implements OnInit {
    constructor(
        private _telegramService: TelegramService,
        private _authService: AuthenticationService,
        private _toastr: ToastsManager) { }

    public verificationCode: string;

    public telegramId?: number;
    public enabledState?: boolean;

    public errors: string[] = [];

    public ngOnInit(): void {
        let user = this._authService.GetCurrentUser();

        if (user.TelegramId != null) {
            this.telegramId = user.TelegramId;
            this.enabledState = user.IsTelegramNotificationEnabled;
        }
    }
    
    public verifyCode() : void {
        this.errors = [];

        if (this.verificationCode == null)
            return;

        this._telegramService.verifyTelegramCode(this.verificationCode)
            .subscribe(id => {
                this._authService.SetTelegramId(id);
                this.telegramId = id;
                this.enabledState = true;

                this._toastr.success("Telegram account has been connected successfully.");
            }, e => this.processErrors(e));
    }

    public resetTelegram() : void {
        if (!confirm("Do you really want to disconnect Telegram account from the profile?"))
            return;

        this.errors = [];

        this._telegramService.resetTelegram()
            .subscribe(r => {
                this._authService.ResetTelegram();
                this.telegramId = null;
                this.enabledState = null;

                this._toastr.success("Telegram account has been disconnected successfully.");
            }, e => this.processErrors(e));
    }

    public disableNotifications() : void {
        this.errors = [];

        this._telegramService.setNotificationsEnabled(false)
            .subscribe(r => {
                this.enabledState = false;
                this._authService.SetTelegramNotificationsEnabled(false);

                this._toastr.success("Notifications have been disabled succesfully.");
            }, e => this.processErrors(e));
    }

    public enableNotifications() : void {
        this.errors = [];

        this._telegramService.setNotificationsEnabled(true)
            .subscribe(r => {
                this.enabledState = true;
                this._authService.SetTelegramNotificationsEnabled(true);

                this._toastr.success("Notifications have been enabled succesfully.");
            }, e => this.processErrors(e));
    }

    private processErrors(resp: Response) : void {
        var errors = resp.json();

        if (errors instanceof Array) {
            this.errors = errors;
        } else {
            this.errors.push("It seems we have some problems. Try again or reload the page.");
        }
    }
}
