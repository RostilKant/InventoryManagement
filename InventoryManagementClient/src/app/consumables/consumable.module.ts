import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConsumablesPageComponent } from './consumables-page/consumables-page.component';
import {SharedModule} from "../shared/shared.module";
import {RouterModule} from "@angular/router";
import {ConsumableService} from "./consumable.service";
import {INTERCEPTOR_PROVIDER} from "../shared/models/constants";
import { EditConsumablePageComponent } from './edit-consumable-page/edit-consumable-page.component';
import { CreateConsumablePageComponent } from './create-consumable-page/create-consumable-page.component';

@NgModule({
  declarations: [
    ConsumablesPageComponent,
    EditConsumablePageComponent,
    CreateConsumablePageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {path: '', component: ConsumablesPageComponent, pathMatch: 'full'},
      {path: 'create', component: CreateConsumablePageComponent},
      {path: ':id/edit', component: EditConsumablePageComponent}

    ]),
  ],
  exports: [
    RouterModule
  ],
  providers: [
    ConsumableService,
    INTERCEPTOR_PROVIDER
  ]
})
export class ConsumableModule { }
