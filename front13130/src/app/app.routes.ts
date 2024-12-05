import { Routes } from "@angular/router";
import { RegisterComponent } from "./auth/register/register.component";
import { LoginComponent } from "./auth/login/login.component";
import { DashboardComponent } from "./dashboard/dashboard/dashboard.component";
import { EventComponent } from "./dashboard/event/event.component";

export const routes: Routes = [
  {
    path: "",
    component: RegisterComponent,
  },
  {
    path: "login",
    component: LoginComponent,
  },
  {
    path: "dashboard",
    component: DashboardComponent,
  },
  {
    path: "event/:id",
    component: EventComponent,
  },
];
