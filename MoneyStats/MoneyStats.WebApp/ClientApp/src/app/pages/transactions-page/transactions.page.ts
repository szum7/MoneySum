import { Component } from '@angular/core';
import { TransactionService } from 'src/app/services/transaction-service/transaction.service';

@Component({
    selector: 'app-transactions-page',
    templateUrl: './transactions.page.html',
    styleUrls: ['./transactions.page.scss']
})
export class TransactionsPage {

    transactions: Array<any>;
    groupedTransactions: Array<any>;

    constructor(private transactionService: TransactionService) {
        this.program();
    }

    private program() {
        let self = this;
        this.getTransactions(function (response) {
            self.transactions = response;
            self.processTransactionsToPage();
            console.log(self.groupedTransactions);
        }, function (error) {
            console.log(error);
        });
    }

    private getAllDatesArray(arr: Array<any>, key: string): Array<any> {

        let minDate: Date = new Date(arr[0][key]),
            maxDate: Date = new Date(arr[arr.length - 1][key]),
            allDates: Array<Date> = this.getDatesBetweenDates(minDate, maxDate),
            merged: Array<any> = [],
            i: number = 0,
            j: number = 0;

        while (i < arr.length && j < allDates.length) {

            let date = new Date(arr[i][key]);
            if (date.getTime() < allDates[j].getTime()) {
                merged.push(arr[i]);
            } else if (date.getTime() > allDates[j].getTime()) {
                merged.push({
                    'date': allDates[j].toJSON(),
                    'transactions': []
                });
                j++;
            } else {
                merged.push(arr[i]);
                i++;
                j++;
            }
        }
        while (i < arr.length) {
            merged.push(arr[i]);
            i++;
        }
        return merged;
    }

    private getDatesBetweenDates(startDate: Date, endDate: Date): Array<Date> {
        var dates = [],
            currentDate = startDate,
            addDays = function (days) {
                var date = new Date(this.valueOf());
                date.setDate(date.getDate() + days);
                return date;
            };
        while (currentDate <= endDate) {
            dates.push(currentDate);
            currentDate = addDays.call(currentDate, 1);
        }
        return dates;
    }

    private processTransactionsToPage() {
        if (this.transactions == null || this.transactions.length == 0) {
            return;
        }

        this.groupedTransactions = this.groupArray(this.transactions, 'accountingDate');
        this.sortArrayByDate(this.groupedTransactions, 'date');
    }

    private getTransactions(successCallback: Function, errorCallback: Function): void {
        this.transactionService.getWithEntities().subscribe((response) => {
            successCallback(response);
        }, (error) => {
            errorCallback(error);
        });
    }

    private sortArrayByDate(arr: Array<any>, property: string) {
        arr.sort(function (a, b) {
            let d1: Date = new Date(a[property]);
            let d2: Date = new Date(b[property]);
            return d1.getTime() - d2.getTime();
        });
    }

    private groupArray(arr: Array<any>, property: string): Array<any> {
        let i = 0, val, index, values = [], ret = [];
        for (; i < arr.length; i++) {
            val = arr[i][property];
            index = values.indexOf(val);
            if (index > -1)
                ret[index].transactions.push(arr[i]);
            else {
                values.push(val);
                ret.push({
                    'date': val,
                    'transactions': [arr[i]]
                });
            }
        }
        return ret;
    }
}
