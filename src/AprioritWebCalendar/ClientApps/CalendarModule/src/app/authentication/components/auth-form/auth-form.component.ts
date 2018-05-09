import { Component, OnInit } from '@angular/core';
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

        this._authService.Login(this.LoginModel.EmailOrUserName, this.LoginModel.Password)
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

    public Register(form: NgForm) : void {
        this.Errors = [];

        if (!form.valid)
            return;

        this._authService.Register(this.RegisterModel.Email, this.RegisterModel.UserName, this.RegisterModel.Password, this.RegisterModel.TimeZone)
            .subscribe(r => {
                this._toastr.success("You have been registered successfully. Sign in to continue.");
                this.IsLoginOpened = true;
            }, r => {
                var result = r.json();

                if (result instanceof Array) {
                    this.Errors = result as string[];
                } else {
                    this.Errors.push("Something happened. Try again!");
                }
            });
    }

    public SwitchForms(isLoginOpened: boolean) : void {
        if (this.IsLoginOpened != isLoginOpened) {
            this.IsLoginOpened = !this.IsLoginOpened;
            this.Errors = [];
            this.RegisterModel = new RegisterModel();
            this.LoginModel = new LoginModel();
        }
    }
}
