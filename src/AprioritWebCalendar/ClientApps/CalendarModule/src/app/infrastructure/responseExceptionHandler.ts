import { Response } from "@angular/http";

export class ResponseExceptionHandler {
    static throwExcepion(response : Response) {
        if (response.status == 400) {
            throw new Error(JSON.stringify(response.json()));
        }
        else if (!response.ok) {
            throw new Error();
        }
    }
}