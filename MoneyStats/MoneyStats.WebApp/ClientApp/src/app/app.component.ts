import { Component } from '@angular/core';
import { LoadingScreenService } from './services/loading-screen-service/loading-screen.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'MoneyStats';

  constructor(public loadingScreen: LoadingScreenService) {
  }
}
