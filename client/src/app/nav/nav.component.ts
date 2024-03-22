import { Component } from '@angular/core';
import { AccountService } from '../_services/account.service';
// import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent {
  model: any = {};
  // this is unnecessary since we use account service directly in the template, but left for the of() use.
  // currentUser$: Observable<User | null> = of(null); // use rxjs of() to initialize to satisfy observable type - we can't just use null, has to be an Observable OF null.

  // use this in the html template and pipe it to 'async': currentUser$ | async
  /**
   * This is known as the 'async pipe' in angular.   Anything we use after a | is referred to as a pipe.
   * In order to 'observe' observables we need to subscribe to them in Angular.
   * The async pipe will take care of both subscribing and unsubscribing to the observable in the
   * currentUser$ observable.
   */
  // make account service public to use it directly in the template to check current user
  constructor(public accountService: AccountService) {}

  ngOnInit(): void {}

  login() {
    this.accountService.login(this.model).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => console.log(error),
    });
  }

  logout() {
    this.accountService.logout(); // removes user from localstorage
  }
}
