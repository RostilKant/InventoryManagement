import {NgModule, Provider} from "@angular/core";
import {RouterModule} from "@angular/router";
import {EmployeesPageComponent} from "./employees-page/employees-page.component";
import {CreateEmployeePageComponent} from './create-employee-page/create-employee-page.component';
import {EditEmployeePageComponent} from './edit-employee-page/edit-employee-page.component';
import {SharedModule} from "../shared/shared.module";
import {EmployeeService} from "./employee.service";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {AuthInterceptor} from "../shared/serviсes/auth.interceptor";

const INTERCEPTOR_PROVIDER: Provider = {
  provide: HTTP_INTERCEPTORS,
  multi: true,
  useClass: AuthInterceptor
};

@NgModule({
  declarations: [
    EmployeesPageComponent,
    CreateEmployeePageComponent,
    EditEmployeePageComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: '', component: EmployeesPageComponent, pathMatch: 'full'},
      {path: 'create', component: CreateEmployeePageComponent}
    ])
  ],
  exports: [
    RouterModule
  ],
  providers: [
    EmployeeService,
    INTERCEPTOR_PROVIDER
  ]
})

export class EmployeeModule {

}
