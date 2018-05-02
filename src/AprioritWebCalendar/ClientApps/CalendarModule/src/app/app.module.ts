import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ApplicationModule, APP_INITIALIZER } from '@angular/core';


import { AppComponent } from './app.component';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule } from '@angular/forms';
import { HttpModule, Http, XHRBackend, RequestOptions } from '@angular/http';

import { BootstrapModalModule, DialogService } from 'ng2-bootstrap-modal';
import { ColorPickerModule } from 'ngx-color-picker';
import { ContextmenuModule } from 'ng2-contextmenu';
import { TypeaheadModule, ButtonsModule, TooltipModule, TimepickerModule, BsDatepickerModule } from 'ngx-bootstrap';
import { ToastModule } from 'ng2-toastr/ng2-toastr';
import { PopoverModule } from 'ngx-bootstrap';
import { ClickOutsideModule } from 'ng-click-outside';

import { AgmCoreModule } from '@agm/core';

import { CalendarModule as ngCalendarModule } from 'angular-calendar';

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
import { CalendarDeleteComponent } from './calendar/components/calendar-delete/calendar-delete.component';
import { ShareCalendarComponent } from './calendar/components/share-calendar/share-calendar.component';
import { SharedUsersListComponent } from './calendar/components/shared-users-list/shared-users-list.component';
import { UserService } from './services/user.service';
import { SelectUserShareComponent } from './calendar/components/select-user-share/select-user-share.component';
import { CalendarViewSwitcherComponent } from './calendar/components/calendar-view-switcher/calendar-view-switcher.component';
import { AddEventButtonComponent } from './calendar/components/add-event-button/add-event-button.component';
import { NotificationsPopoverComponent } from './authentication/components/notifications-popover/notifications-popover.component';
import { EventService } from './event/services/event.service';
import { EventCreateComponent } from './event/components/event-create/event-create.component';
import { EventPeriodComponent } from './event/components/event-period/event-period.component';
import { EventLocationComponent } from './event/components/event-location/event-location.component';
import { EventEditComponent } from './event/components/event-edit/event-edit.component';
import { EventDeleteComponent } from './event/components/event-delete/event-delete.component';
import { EventMoveComponent } from './event/components/event-move/event-move.component';
import { EventShareComponent } from './event/components/event-share/event-share.component';
import { EventShareUsersComponent } from './event/components/event-share-users/event-share-users.component';
import { EventSelectUserShareComponent } from './event/components/event-select-user-share/event-select-user-share.component';
import { EventSearchComponent } from './event/components/event-search/event-search.component';
import { InvitationsIncomingComponent } from './invitation/components/invitations-incoming/invitations-incoming.component';
import { InvitationsIncomingButtonComponent } from './invitation/components/invitations-incoming-button/invitations-incoming-button.component';
import { InvitationService } from './invitation/services/invitation.service';
import { InvitationViewComponent } from './invitation/components/invitation-view/invitation-view.component';
import { CalendarExportComponent } from './calendar/components/calendar-export/calendar-export.component';
import { CalendarIcalService } from './calendar/services/calendar.ical.service';
import { NotificationListener } from './notification/notification.listener';

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
    CalendarEditComponent,
    CalendarDeleteComponent,
    ShareCalendarComponent,
    SharedUsersListComponent,
    SelectUserShareComponent,
    CalendarViewSwitcherComponent,
    AddEventButtonComponent,
    NotificationsPopoverComponent,
    EventCreateComponent,
    EventPeriodComponent,
    EventLocationComponent,
    EventEditComponent,
    EventDeleteComponent,
    EventMoveComponent,
    EventShareComponent,
    EventShareUsersComponent,
    EventSelectUserShareComponent,
    EventSearchComponent,
    InvitationsIncomingComponent,
    InvitationsIncomingButtonComponent,
    InvitationViewComponent,
    CalendarExportComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    BrowserAnimationsModule,

    BootstrapModalModule,
    ColorPickerModule,
    ContextmenuModule,
    TypeaheadModule.forRoot(),
    ToastModule.forRoot(),
    ButtonsModule.forRoot(),
    TooltipModule.forRoot(),
    PopoverModule.forRoot(),

    BsDatepickerModule.forRoot(),
    TimepickerModule.forRoot(),
    ClickOutsideModule,

    ngCalendarModule.forRoot(),
    
    AgmCoreModule.forRoot({
        apiKey: 'AIzaSyBuPpVTIGkimz2VGPdGP5uSYkH4z43zQXM'
    }),

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

    CalendarService,
    UserService,
    EventService,
    InvitationService,
    CalendarIcalService,

    NotificationListener
  ],
  bootstrap: [AppComponent],
  entryComponents: [
      CalendarCreateComponent,
      CalendarEditComponent,
      CalendarDeleteComponent,
      ShareCalendarComponent,
      CalendarExportComponent,

      EventCreateComponent,
      EventEditComponent,
      EventDeleteComponent,
      EventMoveComponent,
      EventShareComponent
    ]
})
export class CalendarModule { }
