import { NgModule } from '@angular/core';
import { DevicesPageComponent } from './devices-page/devices-page.component';
import { CreateDevicePageComponent } from './create-device-page/create-device-page.component';
import { EditDevicePageComponent } from './edit-device-page/edit-device-page.component';
import {SharedModule} from "../shared/shared.module";



@NgModule({
  declarations: [
    DevicesPageComponent,
    CreateDevicePageComponent,
    EditDevicePageComponent
  ],
  imports: [
    SharedModule
  ]
})
export class DeviceModule { }
