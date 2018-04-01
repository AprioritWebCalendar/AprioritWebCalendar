import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';
import { LoginModel } from './login.model';
import { NgForm } from '@angular/forms';
import { Response } from '@angular/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  constructor(
    private authService: AuthenticationService,
    private router: Router) { }

  private loginModel: LoginModel = new LoginModel();
  private errors: string[];

  login(loginForm: NgForm) {
    this.errors = null;

    if (!loginForm.valid)
      return;

    this.authService.login(this.loginModel.emailOrUserName, this.loginModel.password)
      .subscribe((response: Response) => {
        if (this.authService.isAuthenticated()) {
          this.router.navigate(['/']);
        }
      },
      (response: Response) => {
        var result = response.json();

        if (result instanceof Array){
          this.errors = result as string[];
        } else {
          this.errors.push("Something happened. Try again!");
        }
      });
  }
}
