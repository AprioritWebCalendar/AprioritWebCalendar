import { Routes, RouterModule } from "@angular/router";
import { AuthorizeGuard } from "./guards/authorize.guard";
import { AnonymousGuard } from "./guards/anonymous.guard";
import { AppComponent } from "./app.component";
import { MainScreenComponent } from "./calendar/components/main-screen/main-screen.component";
import { AuthFormComponent } from "./authentication/components/auth-form/auth-form.component";
import { SettingsMainComponent } from "./settings/components/settings-main/settings-main.component";

const appRoutes: Routes = [
    { path: '', component: MainScreenComponent, canActivate: [AuthorizeGuard] },
    { path: 'auth/:action', component: AuthFormComponent, canActivate: [AnonymousGuard] },
    { path: 'settings', component: SettingsMainComponent, canActivate: [AuthorizeGuard] },

    { path: '**', redirectTo: '/' }
];

export const routing = RouterModule.forRoot(appRoutes);