import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ApplicationModule, APP_INITIALIZER } from '@angular/core';


import { AppComponent } from './app.component';

import { FormsModule } from '@angular/forms';
import { HttpModule, Http, XHRBackend, RequestOptions } from '@angular/http';

import { BootstrapModalModule, DialogService } from 'ng2-bootstrap-modal';
import { ColorPickerModule } from 'ngx-color-picker';
import { ContextmenuModule } from 'ng2-contextmenu';

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
import { CalendarService } from './calendar/services/calendar.service';
import { MainScreenComponent } from './calendar/components/main-screen/main-screen.component';
import { LeftCalendarMenuComponent } from './calendar/components/left-calendar-menu/left-calendar-menu.component';
import { CalendarCreateComponent } from './calendar/components/calendar-create/calendar-create.component';
import { CalendarEditComponent } from './calendar/components/calendar-edit/calendar-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthPanelComponent,
    LoginComponent,
    RegisterComponent,
    AlertComponent,
    AlertArrayComponent,
    MainScreenComponent,
    LeftCalendarMenuComponent,
    CalendarCreateComponent,
    CalendarEditComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,

    BootstrapModalModule,
    ColorPickerModule,
    ContextmenuModule,

    routing
  ],
  providers: [
    {
      provide: CustomHttp,
      deps: [XHRBackend, RequestOptions, ApplicationModule],
      useFactory: (backend, options, aplicationService) => {
        var customHttp = new CustomHttp(backend, options, aplicationService);
        customHttp.getToken();
        return customHttp;
      }
    },

    AuthenticationService,

    AuthorizeGuard,
    AnonymousGuard,

    CalendarService
  ],
  bootstrap: [AppComponent],
  entryComponents: [
      CalendarCreateComponent,
      CalendarEditComponent
    ]
})
export class CalendarModule { }
