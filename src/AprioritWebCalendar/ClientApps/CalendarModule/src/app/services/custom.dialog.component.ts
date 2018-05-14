import { Injectable } from "@angular/core";
import { DialogComponent } from "ng2-bootstrap-modal";

@Injectable()
export class CustomDialogComponent<T, T1> extends DialogComponent<T, T1> {
    close(): void {
        document.body.classList.remove("modal-open");
        super.close();
    }
}