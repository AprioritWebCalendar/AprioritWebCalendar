import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { AuthenticationService } from '../../../authentication/services/authentication.service';

@Component({
    selector: 'app-settings-timezone',
    templateUrl: './settings-timezone.component.html'
})
export class SettingsTimezoneComponent implements OnInit {

    constructor(
        private _settingsService: SettingsService,
        private _authService: AuthenticationService
    ) { }

    public timeZone: string;
    public preferredTimeZone: string;
    public isError: boolean;

    @Output()
    public onSuccess = new EventEmitter<string>();

    @Output()
    public onError = new EventEmitter<string>();

    public ngOnInit() : void {
        this.preferredTimeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

        this._settingsService.getTimeZone()
            .subscribe(tz => this.timeZone = tz,
            e => {
                this.isError = true;
                this.onError.emit("Unable to get user's timezone");
            });
    }

    public saveTimeZone() : void {
        if (this.timeZone == null)
            return;

        this._settingsService.saveTimeZone(this.timeZone)
            .subscribe(r => {
                this._authService.SetUserTimeZone(this.timeZone);
                this.onSuccess.emit("The TimeZone has been changed successfully");
            }, e => {
                if (e instanceof Array) {
                    this.onError.emit((e as string[]).join('\n'));
                }
            });
    }

    public setPreferred() : void {
        this.timeZone = this.preferredTimeZone;
    }

}
