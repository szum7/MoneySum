import { Component } from '@angular/core';
import { RouterService } from 'src/app/services/router-service/router.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  constructor(private router: RouterService) {
  }
}
