import { Component, OnInit } from '@angular/core';
import { ToastsManager } from 'ng2-toastr';

@Component({
    selector: 'app-settings-main',
    templateUrl: './settings-main.component.html'
})
export class SettingsMainComponent {
    constructor(private _toastr: ToastsManager) {   
    }

    public successToast(message: string) : void {
        this._toastr.success(message);
    }

    public errorToast(message: string) : void {
        this._toastr.error(message);
    }
}
