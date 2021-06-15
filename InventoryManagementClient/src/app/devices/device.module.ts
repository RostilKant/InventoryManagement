import {NgModule} from '@angular/core';
import { DevicesPageComponent } from './devices-page/devices-page.component';
import { CreateDevicePageComponent } from './create-device-page/create-device-page.component';
import { EditDevicePageComponent } from './edit-device-page/edit-device-page.component';
import {SharedModule} from "../shared/shared.module";
import {DeviceService} from "./device.service";
import {RouterModule} from "@angular/router";
import {INTERCEPTOR_PROVIDER} from "../shared/models/constants";

@NgModule({
  declarations: [
    DevicesPageComponent,
    CreateDevicePageComponent,
    EditDevicePageComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild([
      {path: '', component: DevicesPageComponent, pathMatch: 'full'},
      {path: 'create', component: CreateDevicePageComponent},
      {path: ':id/edit', component: EditDevicePageComponent}

    ]),
  ],
  exports: [
    RouterModule
  ],
  providers: [
    DeviceService,
    INTERCEPTOR_PROVIDER
  ]
})
export class DeviceModule { }
