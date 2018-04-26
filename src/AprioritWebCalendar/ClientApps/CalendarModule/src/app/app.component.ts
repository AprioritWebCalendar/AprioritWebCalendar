import { Component, ViewContainerRef } from '@angular/core';
import { ToastsManager } from 'ng2-toastr';
import { AuthenticationService } from './authentication/services/authentication.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
    constructor(
        public authService: AuthenticationService,
        toastr: ToastsManager, vcr: ViewContainerRef
    ) {
        toastr.setRootViewContainerRef(vcr);
    }
}
