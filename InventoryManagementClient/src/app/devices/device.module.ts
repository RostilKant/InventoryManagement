import {NgModule, Provider} from '@angular/core';
import { DevicesPageComponent } from './devices-page/devices-page.component';
import { CreateDevicePageComponent } from './create-device-page/create-device-page.component';
import { EditDevicePageComponent } from './edit-device-page/edit-device-page.component';
import {SharedModule} from "../shared/shared.module";
import {DeviceService} from "./device.service";
import {RouterModule} from "@angular/router";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {AuthInterceptor} from "../shared/serviсes/auth.interceptor";

const INTERCEPTOR_PROVIDER: Provider = {
  provide: HTTP_INTERCEPTORS,
  multi: true,
  useClass: AuthInterceptor
};


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
