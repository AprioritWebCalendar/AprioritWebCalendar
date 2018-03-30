import { Response } from "@angular/http";

import { ErrorArray } from "../infrastructure/errorArray";

export class ResponseExceptionHandler {
    static throwExcepion(response : Response) {
        if (response.status == 400)
                throw new ErrorArray(response.json());
            else if (!response.ok)
                throw new Error();
    }
}