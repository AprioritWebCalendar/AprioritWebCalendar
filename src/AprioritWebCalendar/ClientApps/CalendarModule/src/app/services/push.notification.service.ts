import { Injectable } from "@angular/core";
import { ToastsManager } from "ng2-toastr";

@Injectable()
export class PushNotificationService {
    constructor(private toastr: ToastsManager) {

    }

    public PushNotification(text: string, title: string) : void {
        /*
            positionClass doesn't work and this bug hasn't been fixed.
            So notification is always on top right.
        */

        this.toastr.info(text, title, { positionClass: "toast-bottom-right" });
    }
}