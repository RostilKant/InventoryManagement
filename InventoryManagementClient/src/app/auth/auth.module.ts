import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {EmployeesPageComponent} from "../employees/employees-page/employees-page.component";
import {LoginPageComponent} from './login-page/login-page.component';
import {RegisterPageComponent} from './register-page/register-page.component';
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {SharedModule} from "../shared/shared.module";

@NgModule({
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: '', redirectTo: '/auth/login', pathMatch: 'full'},
      {path: 'login', component: LoginPageComponent},
      {path: 'register', component: RegisterPageComponent}
    ])
  ],
  exports: [
    RouterModule
  ],
  declarations: [
    LoginPageComponent,
    RegisterPageComponent
  ]
})

export class AuthModule {

}
