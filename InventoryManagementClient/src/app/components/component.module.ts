import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsPageComponent } from './components-page/components-page.component';
import {SharedModule} from "../shared/shared.module";
import {RouterModule} from "@angular/router";
import {ComponentService} from "./component.service";
import {INTERCEPTOR_PROVIDER} from "../shared/models/constants";
import { EditComponentPageComponent } from './edit-component-page/edit-component-page.component';
import { CreateComponentPageComponent } from './create-component-page/create-component-page.component';

@NgModule({
  declarations: [
    ComponentsPageComponent,
    EditComponentPageComponent,
    CreateComponentPageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild([
      {path: '', component: ComponentsPageComponent, pathMatch: 'full'},
      {path: 'create', component: CreateComponentPageComponent},
      {path: ':id/edit', component: EditComponentPageComponent}

    ]),  ],
  exports: [
    RouterModule
  ],
  providers: [
    ComponentService,
    INTERCEPTOR_PROVIDER
  ]
})
export class ComponentModule { }
