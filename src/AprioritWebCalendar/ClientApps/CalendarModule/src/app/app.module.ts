import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import { AppComponent } from './app.component';

import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AuthPanelComponent } from './authentication/components/auth-panel/auth-panel.component';

import { AuthorizeGuard } from './guards/authorize.guard';
import { AnonymousGuard } from './guards/anonymous.guard';

import { AuthenticationService } from './authentication/services/authentication.service';

import { routing } from './app.routing';
import { LoginComponent } from './authentication/components/login/login.component';
import { RegisterComponent } from './authentication/components/register/register.component';
import { AlertComponent } from './shared/alert/alert.component';
import { AlertArrayComponent } from './shared/alert-array/alert-array.component';

import { EqualTextValidator } from 'angular2-text-equality-validator';
import { CustomHttp } from './services/custom.http';

@NgModule({
  declarations: [
    AppComponent,
    AuthPanelComponent,
    LoginComponent,
    RegisterComponent,
    AlertComponent,
    AlertArrayComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    routing
  ],
  providers: [
    AuthorizeGuard,
    AnonymousGuard,

    AuthenticationService,
    CustomHttp
  ],
  bootstrap: [AppComponent]
})
export class CalendarModule { }
