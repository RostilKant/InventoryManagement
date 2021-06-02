import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {EmployeesPageComponent} from "./employees-page/employees-page.component";
import { CreateEmployeePageComponent } from './create-employee-page/create-employee-page.component';
import { EditEmployeePageComponent } from './edit-employee-page/edit-employee-page.component';

@NgModule({
  declarations:[
    EmployeesPageComponent,
    CreateEmployeePageComponent,
    EditEmployeePageComponent
  ],
  imports:[
    CommonModule,
    RouterModule.forChild([
      {path: '', component: EmployeesPageComponent}
    ])
  ],
  exports: [
    RouterModule
  ]
})

export class EmployeeModule{

}
