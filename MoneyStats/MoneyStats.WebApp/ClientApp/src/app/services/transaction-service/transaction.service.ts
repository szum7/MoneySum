import { Injectable, Inject } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base-http.service';
import { Transaction } from './models/transaction.model';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

class TransactionServiceMap {
    protected extractTransactions(response: any): any {
        return response;
    }
}

class TransactionServiceLogic extends TransactionServiceMap {

}

@Injectable()
export class TransactionService extends TransactionServiceLogic {

    constructor(
        private base: BaseHttpService, 
        private http: HttpClient, 
        @Inject('BASE_URL') private baseUrl: string) {
        super();
    }

    get(): Observable<any> { // Array<Transaction>
        return this.http
        .get<any>(this.baseUrl + 'api/Transaction/get')
        .pipe(map(this.extractTransactions));
    }

}