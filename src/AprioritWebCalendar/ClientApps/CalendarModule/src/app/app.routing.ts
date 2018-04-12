import { Routes, RouterModule } from "@angular/router";
import { AuthorizeGuard } from "./guards/authorize.guard";
import { AnonymousGuard } from "./guards/anonymous.guard";
import { LoginComponent } from "./authentication/components/login/login.component";
import { RegisterComponent } from "./authentication/components/register/register.component";
import { AppComponent } from "./app.component";
import { MainScreenComponent } from "./calendar/components/main-screen/main-screen.component";

const appRoutes: Routes = [
    { path: '', component: MainScreenComponent, canActivate: [AuthorizeGuard] },
    { path: 'login', component: LoginComponent, canActivate: [AnonymousGuard] },
    { path: 'register', component: RegisterComponent, canActivate: [AnonymousGuard] },

    { path: '**', redirectTo: '/' }
];

export const routing = RouterModule.forRoot(appRoutes);