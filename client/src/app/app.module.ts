import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

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
    HttpClientModule,
    BrowserAnimationsModule, // we need this angular module to make http requests
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
