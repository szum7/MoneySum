import { Component } from '@angular/core';
import { RouterService } from 'src/app/services/router-service/router.service';

@Component({
  selector: 'app-nav-narrow',
  templateUrl: './nav-narrow.component.html',
  styleUrls: ['./nav-narrow.component.scss']
})
export class NavNarrowComponent {

  constructor(private router: RouterService) {
  }
}
