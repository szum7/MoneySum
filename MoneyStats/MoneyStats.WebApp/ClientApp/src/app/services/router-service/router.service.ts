import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable()
export class RouterService {

  constructor(private router: Router) {
  }

  goToBogus(): void {
    this.router.navigate(['calendar/' + 123 ]);
  }
}
