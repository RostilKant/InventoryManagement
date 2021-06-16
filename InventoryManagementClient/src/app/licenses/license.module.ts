import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SharedModule} from "../shared/shared.module";
import {RouterModule} from "@angular/router";
import {LicenseService} from "./license.service";
import {INTERCEPTOR_PROVIDER} from "../shared/models/constants";
import { LicensesPageComponent } from './licenses-page/licenses-page.component';
import { CreateLicensePageComponent } from './create-license-page/create-license-page.component';
import { EditLicensePageComponent } from './edit-license-page/edit-license-page.component';



@NgModule({
  declarations: [
    LicensesPageComponent,
    CreateLicensePageComponent,
    EditLicensePageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {path: '', component: LicensesPageComponent, pathMatch: 'full'},
      {path: 'create', component: CreateLicensePageComponent},
      {path: ':id/edit', component: EditLicensePageComponent}
    ]),
  ],
  exports: [
    RouterModule
  ],
  providers: [
    LicenseService,
    INTERCEPTOR_PROVIDER
  ]
})
export class LicenseModule { }
