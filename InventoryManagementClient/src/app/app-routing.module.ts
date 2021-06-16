import {NgModule} from '@angular/core';
import {PreloadAllModules, RouterModule, Routes} from '@angular/router';
import {MainLayoutComponent} from "./shared/components/main-layout/main-layout.component";
import {DashboardPageComponent} from "./dashboard-page/dashboard-page.component";
import {AuthGuard} from "./shared/serviсes/auth.guard";

const routes: Routes = [
  {
    path: '', component: MainLayoutComponent, children: [
      {path: '', redirectTo: '/dashboard', pathMatch: 'full'},
      {path: 'dashboard', component: DashboardPageComponent, canActivate: [AuthGuard]},
      {
        path: 'employees', loadChildren: () => import('./employees/employee.module').then(m => m.EmployeeModule),
        canActivate: [AuthGuard]
      },
      {
        path: 'devices', loadChildren: () => import('./devices/device.module').then(m => m.DeviceModule),
        canActivate: [AuthGuard]
      },
      {
        path: 'accessories', loadChildren: () => import('./accessories/accessory.module').then(m => m.AccessoryModule),
        canActivate: [AuthGuard]
      },
      {
        path: 'components', loadChildren: () => import('./components/component.module').then(m => m.ComponentModule),
        canActivate: [AuthGuard]
      },
      {
        path: 'consumables', loadChildren: () => import('./consumables/consumable.module').then(m => m.ConsumableModule),
        canActivate: [AuthGuard]
      }
    ]
  },
  {path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    preloadingStrategy: PreloadAllModules
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
