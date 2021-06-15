import {NgModule} from "@angular/core";
import {HttpClientModule} from "@angular/common/http";
import {CommonModule} from "@angular/common";
import {AuthService} from "./serviсes/auth.service";

import {MatToolbarModule} from "@angular/material/toolbar";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatIconModule} from "@angular/material/icon";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {MatButtonModule} from "@angular/material/button";
import {MatInputModule} from "@angular/material/input";
import {MatCardModule} from "@angular/material/card";
import {AuthGuard} from "./serviсes/auth.guard";
import {MatSnackBarModule} from "@angular/material/snack-bar";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatNativeDateModule} from "@angular/material/core";
import {MatTableModule} from "@angular/material/table";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {MatPaginatorModule} from "@angular/material/paginator";
import { RemoveUnderscorePipe } from './pipes/remove-underscore.pipe';
import {MatSelectModule} from "@angular/material/select";
import {MatSortModule} from "@angular/material/sort";
import {MatCheckboxModule} from "@angular/material/checkbox";
import {MatExpansionModule} from "@angular/material/expansion";
import {MatListModule} from "@angular/material/list";
import {MatChipsModule} from "@angular/material/chips";

@NgModule({
  imports: [
  ],
    exports: [
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,

        RemoveUnderscorePipe,

        MatToolbarModule,
        MatFormFieldModule,
        MatButtonModule,
        MatIconModule,
        MatButtonModule,
        MatInputModule,
        MatCardModule,
        MatSnackBarModule,
        MatDatepickerModule,
        MatNativeDateModule,
        MatTableModule,
        MatProgressSpinnerModule,
        MatPaginatorModule,
        MatSelectModule,
        MatSortModule,
        MatCheckboxModule,
        MatExpansionModule,
        MatListModule,
        MatChipsModule
    ],
  providers: [
    AuthService,
    AuthGuard
  ],
  declarations: [
    RemoveUnderscorePipe
  ]
})
export class SharedModule {

}
