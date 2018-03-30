import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { User } from '../../models/user';

@Component({
  selector: 'app-auth-panel',
  templateUrl: './auth-panel.component.html'
})
export class AuthPanelComponent implements OnInit {

  constructor(private authService: AuthenticationService) { }

  ngOnInit() {
    let isAuth : boolean = this.authService.isAuthenticated();
    let user : User = this.authService.getCurrentUser();
  }

  logout() {
    this.authService.logout();
  }
}
