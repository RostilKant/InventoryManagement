import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {SharedModule} from "../shared/shared.module";
import {AccessoryService} from "./accessory.service";
import { AccessoriesPageComponent } from './accessories-page/accessories-page.component';
import { CreateAccessoryPageComponent } from './create-accessory-page/create-accessory-page.component';
import { EditAccessoryPageComponent } from './edit-accessory-page/edit-accessory-page.component';
import {RouterModule} from "@angular/router";
import {INTERCEPTOR_PROVIDER} from "../shared/models/constants";



@NgModule({
  declarations: [
    AccessoriesPageComponent,
    CreateAccessoryPageComponent,
    EditAccessoryPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {path: '', component: AccessoriesPageComponent, pathMatch: 'full'},
      {path: 'create', component: CreateAccessoryPageComponent},
      {path: ':id/edit', component: EditAccessoryPageComponent}
    ]),
  ],
  exports: [
    RouterModule
  ],
  providers: [
    AccessoryService,
    INTERCEPTOR_PROVIDER
  ]
})
export class AccessoryModule { }
