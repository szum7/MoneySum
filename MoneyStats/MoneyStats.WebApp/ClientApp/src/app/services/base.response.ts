export abstract class BaseResponse<T> {
    public isValid: boolean;
    public errorMessages: Array<string>;
    public data: T;

    isValidResponse(): boolean {
        return this.isValid && (this.errorMessages == null || this.errorMessages.length === 0);
    }
}

class EmptyObject {
}
  
export class EmptyResponse extends BaseResponse<EmptyObject> {
}
  