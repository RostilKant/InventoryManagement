import { NgModule } from '@angular/core';
import { PreloadAllModules, PreloadingStrategy, RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from "./shared/components/main-layout/main-layout.component";
import { DashboardPageComponent } from "./dashboard-page/dashboard-page.component";

const routes: Routes = [
  {
    path: '', component: MainLayoutComponent, children : [
      { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardPageComponent },
      { path: 'employees', loadChildren: () => import('./employees/employee.module').then(m => m.EmployeeModule) }
    ]
  },
  { path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule) }
];

@NgModule({
  imports: [RouterModule.forRoot(routes,{
    preloadingStrategy: PreloadAllModules
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
