import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { ErrorArray } from '../../../infrastructure/errorArray';
import { Router } from '@angular/router';
import { RegisterModel } from './register.model';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})
export class RegisterComponent  {
  constructor(
    private authService: AuthenticationService,
    private router: Router){}

  private registerModel: RegisterModel = new RegisterModel();
  private errors: string[];

  register(registerForm: NgForm) {
    try {
      this.authService.register(this.registerModel.email, this.registerModel.userName, this.registerModel.password);
      this.router.navigate(['/']);
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
