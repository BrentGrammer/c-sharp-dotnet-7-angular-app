import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root', // automatically adds it to providers array in the root module (i.e. app.module.ts)
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  // use a behavior subject that outside components can subscribe to - allows for setting an initial value
  private currentUserSource = new BehaviorSubject<User | null>(null);
  // make this available outside - $ is convention to indicate the variable is for an observable
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {}

  login(model: any) {
    // we'll store user data in browser storage
    // use the pipe command to accomplish this. The operation in the pipe will happen before the component subscription to this method
    // you need to pass in the generic model returned to http.post<>
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      // use map from rxjs
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((user) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user); // the .next() method is being used to emit a new value to subscribers of the currentUser$ observable
        }
        // IMPORTANT: Remember to return what the top method (http.post here) needs in the map method, otherwise you will get undefined returned.
        // this is for demonstration purposes only and we actually don't use this information or need it returned here
        return user;
      })
    );
  }

  // helper setter that our components can use
  setCurrentUser(user: User) {
    this.currentUserSource.next(user); // the .next() method is being used to emit a new value to subscribers of the currentUser$ observable
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
