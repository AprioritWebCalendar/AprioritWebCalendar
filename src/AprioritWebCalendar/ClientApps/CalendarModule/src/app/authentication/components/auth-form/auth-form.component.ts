import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { LoginModel } from './login.model';
import { RegisterModel } from './register.model';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastsManager } from 'ng2-toastr';

@Component({
    selector: 'app-auth-form',
    templateUrl: './auth-form.component.html',
    styleUrls: ['./auth-form.component.css']
})
export class AuthFormComponent {

    constructor(
        private _authService: AuthenticationService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _toastr: ToastsManager) {

            let action = _activatedRoute.snapshot.params["action"];

            if (action != null && action == "register")
                this.IsLoginOpened = false;
    }
    
    public LoginModel: LoginModel = new LoginModel();
    public RegisterModel: RegisterModel = new RegisterModel();

    public IsLoginOpened: boolean = true;
    public Errors: string[] = [];

    public Login(form: NgForm) : void {
        this.Errors = [];

        if (!form.valid)
            return;

        this.loginRequest(this.LoginModel.EmailOrUserName, this.LoginModel.Password);
    }

    public Register(form: NgForm) : void {
        this.Errors = [];

        if (!form.valid)
            return;

        this._authService.Register(this.RegisterModel)
            .subscribe(r => {
                // this._toastr.success("You have been registered successfully. Sign in to continue.");
                // this.IsLoginOpened = true;

                this.loginRequest(this.RegisterModel.Email, this.RegisterModel.Password);
            }, r => {
                var result = r.json();

                if (result instanceof Array) {
                    this.Errors = result as string[];
                } else {
                    this.Errors.push("Something happened. Try again!");
                }
            });
    }

    public SwitchForms(isLoginOpened: boolean, loginForm: NgForm, registerForm: NgForm) : void {
        if (this.IsLoginOpened != isLoginOpened) {
            this.IsLoginOpened = !this.IsLoginOpened;
            this.Errors = [];
            this.RegisterModel = new RegisterModel();
            this.LoginModel = new LoginModel();

            if (isLoginOpened) {
                registerForm.reset();
            } else {
                loginForm.reset();
            }
        }
    }

    public SetRecaptcha(response: string) : void {
        this.RegisterModel.RecaptchaToken = response;
    }

    private loginRequest(emailOrUserName: string, password: string) : void {
        this._authService.Login(emailOrUserName, password)
            .subscribe(r => {
                this._router.navigate(['/']);
            }, response => {
                var result = response.json();

                if (result instanceof Array) {
                    this.Errors = result as string[];
                } else {
                    this.Errors.push("Something happened. Try again!");
                }
            });
    }
}
