import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { HttpClientModule } from '@angular/common/http';
import { faInfoCircle, faCaretRight, faCaretLeft, faSun, faCog, faTimes } from '@fortawesome/free-solid-svg-icons';

// Components
import { AppComponent } from './app.component';
import { NavComponent } from './components/nav/nav.component';
import { LoadingScreenComponent } from './components/loading-screen/loading-screen.component';

// Pages
import { HomePage } from './pages/home-page/home.page';
import { TransactionsPage } from './pages/transactions-page/transactions.page';

// Services
import { LoadingScreenService } from './services/loading-screen-service/loading-screen.service';
import { RouterService } from './services/router-service/router.service';
import { TransactionService } from './services/transaction-service/transaction.service';
import { BaseHttpService } from './services/base-http.service';

@NgModule({
  declarations: [
    // Components
    AppComponent,
    NavComponent,
    LoadingScreenComponent,
    // Pages
    HomePage,
    TransactionsPage
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FontAwesomeModule,
    RouterModule.forRoot([
      { path: '', component: HomePage },
      { path: 'transactions', component: TransactionsPage },
    ], { useHash: true })
  ],
  providers: [
    LoadingScreenService,
    RouterService,
    BaseHttpService,
    TransactionService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor() {
    library.add(faInfoCircle);
  }
}
