# General

- Survive as long as the app is alive (through the session until user closes browser or tab etc.)

## Persisting Data

- Can store data in the browser storage

## Subscriptions Observables

- Should unsubscribe from subscriptions for best practice
  - You do not need to unsubscribe from methods that do HTTP requests because they complete.
  - After they complete you're no longer subscribed to the observable.
- Using an Async Pipe will automatically subscribe and unsubscribe for you so you don't need to write the code to handle that explicitly.
