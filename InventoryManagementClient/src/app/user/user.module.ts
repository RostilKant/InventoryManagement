import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {SharedModule} from "../shared/shared.module";
import {RouterModule} from "@angular/router";
import {INTERCEPTOR_PROVIDER} from "../shared/models/constants";
import {UserService} from "./user.service";
import { EditUserPageComponent } from './edit-user-page/edit-user-page.component';


@NgModule({
  declarations: [
    EditUserPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {path: 'edit', component: EditUserPageComponent}
    ]),
  ],
  exports: [
    RouterModule
  ],
  providers: [
    UserService,
    INTERCEPTOR_PROVIDER
  ]
})
export class UserModule {
}
