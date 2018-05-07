import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { User } from '../../models/user';

@Component({
    selector: 'app-auth-panel',
    templateUrl: './auth-panel.component.html'
})
export class AuthPanelComponent {

    constructor(
        private _authService: AuthenticationService,
    ) { }

    public IsAuthenticated(): boolean {
        return this._authService.IsAuthenticated();
    }

    public GetUser(): User {
        return this._authService.GetCurrentUser();
    }

    public Logout() {
        if (confirm("Do you really want to log out the account?"))
            this._authService.Logout();
    }
}
