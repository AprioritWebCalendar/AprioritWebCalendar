import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { User } from '../../models/user';

@Component({
    selector: 'app-auth-panel',
    templateUrl: './auth-panel.component.html'
})
export class AuthPanelComponent {

    constructor(private authService: AuthenticationService) { }

    isAuthenticated(): boolean {
        return this.authService.isAuthenticated();
    }

    getUser(): User {
        return this.authService.getCurrentUser();
    }

    logout() {
        if (confirm("Do you really want to log out the account?"))
            this.authService.logout();
    }
}
