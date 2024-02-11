import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

/**
 * This is responsible for bootstrapping config for our application.
 * It is the entry point for the angular app.
 * main.ts actually uses this to bootstrap the application.
 */

@NgModule({
  declarations: [
    // declare modules this module needs to load
    AppComponent,
  ],
  imports: [
    // add any other modules that need to be used
    BrowserModule,
    AppRoutingModule,
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
