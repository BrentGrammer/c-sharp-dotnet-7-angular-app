# C# .NET 7 and Angular app

## Prerequisites

- dotnet sdk (v7)
- node.js (v18)
- [Angular 16 CLI](https://angular.io/guide/setup-local)
  - `npm install -g @angular/cli@16`
- [mkcert](https://github.com/FiloSottile/mkcert) for allowing https on angular client during development
  - note: run install command with elevated privileges (i.e. sudo on mac or as Admin on Windows)
  - make an ssl folder in /client: `mkdir ssl`
  - cd into `ssl` and run: `mkcert -install` (may need to be in elevated prompt with privileges)
  - `mkcert localhost` (you can run this anytime to get a new cert if needed for localhost)
  - in angular.json file add options under the serve block:
  ```json
   "serve": {
      "options": {
        "ssl": true,
        "sslCert": "./ssl/localhost.pem",
        "sslKey": "./ssl/localhost-key.pem"
      },
   }
  ```

## Starting the app

### Starting the client:

- `cd client`
- `ng serve`

### Starting the server:

- open a terminal
- `cd API`
- `dotnet watch` or `dotnet run`
