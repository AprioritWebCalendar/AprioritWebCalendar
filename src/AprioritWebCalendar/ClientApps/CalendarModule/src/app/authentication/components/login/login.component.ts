import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { ErrorArray } from '../../../infrastructure/errorArray';
import { Router } from '@angular/router';
import { LoginModel } from './login.model';
import { NgForm } from '@angular/forms';

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

  login(loginForm: NgForm) {
    try {
      this.errors = [];

      if (!loginForm.valid)
        return;

      this.authService.login(this.loginModel.emailOrUserName, this.loginModel.password)
        .subscribe(data => {
          if (this.authService.isAuthenticated()){
            this.router.navigate(['/']);
          }
        },
        error => {
          this.errors.push("Something happened. Try again!");
        });
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
