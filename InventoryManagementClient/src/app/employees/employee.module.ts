import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {EmployeesPageComponent} from "./employees-page/employees-page.component";

@NgModule({
  declarations:[
    EmployeesPageComponent
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
