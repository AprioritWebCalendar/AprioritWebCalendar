import { Component, ViewContainerRef } from '@angular/core';
import { ToastsManager } from 'ng2-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
    constructor(toastr: ToastsManager, vcr: ViewContainerRef) {
        toastr.setRootViewContainerRef(vcr);
    }
}
