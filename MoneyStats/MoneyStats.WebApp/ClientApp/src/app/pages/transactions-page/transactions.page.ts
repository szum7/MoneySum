import { Component } from '@angular/core';
import { TransactionService } from 'src/app/services/transaction-service/transaction.service';

@Component({
  selector: 'app-transactions-page',
  templateUrl: './transactions.page.html',
  styleUrls: ['./transactions.page.scss']
})
export class TransactionsPage {

  constructor(private transactionService: TransactionService) {
  }

  getTransactions(): void {
    this.transactionService.get().subscribe((response) => {
      console.log(response);
    }, (error) => {
      console.log(error);
    });
  }
}
