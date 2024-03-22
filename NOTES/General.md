# General

- Survive as long as the app is alive (through the session until user closes browser or tab etc.)

## Persisting Data

- Can store data in the browser storage

## Subscriptions Observables

- Should unsubscribe from subscriptions for best practice
  - You do not need to unsubscribe from methods that do HTTP requests because they complete.
  - After they complete you're no longer subscribed to the observable.
- Using an Async Pipe will automatically subscribe and unsubscribe for you so you don't need to write the code to handle that explicitly.

## Parent to child communication

- Use an input property to communicate from parent component to child component and pass to it data
- in template:

```html
<select class="form-select">
  <option
    *ngFor="let user of usersFromHomeComponent"
    [value]="user.username"
  ></option>
</select>
```

- in parent component:
  - use binding to set the property in child component to the property in this(parent) component

```html
// html template in parent passing prop to child
<app-register [usersFromHomeComponent]="users"></app-register>
```

```javascript
// parent component ts file
export class HomeComponent {
  users: any;
```

- in child component:

```javascript
export class RegisterComponent {
  // use Input() decorator
  @Input() usersFromHomeComponent: any;
```

## Child to Parent communication

- In child component ts use Output() decorator

```javascript
export class RegisterComponent {
  // use output decorator in child component
  @Output() cancelRegister = new EventEmitter(); // event emitter comes from angular core

  constructor() {}
  ngOnInit() {}
  cancel() {
    // use the event emitter to emit the data you want to output to parent
    this.cancelRegister.emit(false);
  }
}
```

- In parent component create method that captures the data emitted from the child

```javascript
  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
```

- In parent template call the method and pass the emitted information from the child - use parens for outputted data (and square brackets for recieving data like the usersFromHomeComponent):
  - call the method in the parent component passing in the event with a dollar sign

```html
<app-register
  [usersFromHomeComponent]="users"
  (cancelRegister)="cancelRegisterMode($event)"
></app-register>
```

## Services

- Services are singletons and are a good place to share data in the app.

## Angular Routing

- Choosing 'yes' when bootstrapping an angular app creates a app-routing.module.ts file that imports and exports the routes module (AppRoutingModule is put in app module by default)
- the `routes` array in app-routing.module.ts contains a list of the app routes
