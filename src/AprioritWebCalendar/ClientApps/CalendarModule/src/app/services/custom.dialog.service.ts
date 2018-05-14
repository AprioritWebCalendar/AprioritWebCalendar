import { Injectable, Type } from "@angular/core";
import { DialogService, DialogComponent, DialogOptions } from "ng2-bootstrap-modal";
import { Observable } from "rxjs/Observable";

@Injectable()
export class CustomDialogService extends DialogService {
    addDialog<T, T1>(component: Type<DialogComponent<T, T1>>, data?: T, options?: DialogOptions): Observable<T1> {
        document.body.classList.add("modal-open");
        return super.addDialog(component, data, options);
    }
}