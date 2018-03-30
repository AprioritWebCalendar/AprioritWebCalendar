export class ErrorArray extends Error {
    constructor(private errors : Array<string>){
        super();
    }

    getErrors() : Array<string> {
        return this.errors;
    }
}