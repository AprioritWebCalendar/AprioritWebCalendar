import { Component } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';
import { RegisterModel } from './register.model';
import { NgForm } from '@angular/forms';
import { Response } from '@angular/http';
import { ToastsManager } from 'ng2-toastr';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html'
})
export class RegisterComponent {
    constructor(
        private authService: AuthenticationService,
        private router: Router,
        private toastr: ToastsManager
    ) { }

    public registerModel: RegisterModel = new RegisterModel();
    public errors: string[] = [];

    register(registerForm: NgForm) {
        this.errors = null;

        if (!registerForm.valid)
            return;

        this.authService.register(this.registerModel.email, this.registerModel.userName, this.registerModel.password)
            .subscribe((response: Response) => {
                this.toastr.success("You have been registered successfully. Sign in to continue.");
                this.router.navigate(['/']);
            },
                (response: Response) => {
                    var result = response.json();

                    if (result instanceof Array) {
                        this.errors = result as string[];
                    } else {
                        this.errors.push("Something happened. Try again!");
                    }
                });
    }
}
