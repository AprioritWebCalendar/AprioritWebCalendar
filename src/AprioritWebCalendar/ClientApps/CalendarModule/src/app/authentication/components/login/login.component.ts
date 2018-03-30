import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { ErrorArray } from '../../../infrastructure/errorArray';
import { Router } from '@angular/router';
import { LoginModel } from './login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  constructor(
    private authService: AuthenticationService,
    private router: Router) { }

  private loginModel: LoginModel = new LoginModel();
  private errors: Array<string>;

  login() {
    try {
      this.authService.login(this.loginModel.emailOrUserName, this.loginModel.password);

      if (this.authService.isAuthenticated()){
        this.router.navigate(['/']);
      }
    }
    catch (e) {
      if (e instanceof ErrorArray){
        this.errors = e.getErrors();
      } else {
        alert("Something happened. Try again!");
      }
    }
  }
}
