import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EmptyResponse } from './base.response';

@Injectable()
export class BaseHttpService {

    constructor() {
        // TODO
        // Could have a generic map and logic property but typescript can't (really) instanciate generic types.
        // Could do it with constructor parameters, but then the injections would also have to be passed in.
    }

    getOptions(): { headers: HttpHeaders } {
        const header: HttpHeaders = new HttpHeaders({
            'Content-Type':  'application/json',
            'Authorization': 'Bearer ' + null
        });
        return {headers: header};
    }

    mapEmptyResponse(response: any): EmptyResponse {
        const ret = new EmptyResponse();
        ret.data = null;
        ret.errorMessages = response.errorMessages;
        ret.isValid = response.isValid;
        return ret;
    }
}
