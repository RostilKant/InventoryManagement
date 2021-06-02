import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {UserForRegistrationModel} from "../../shared/models/user/userForRegistration.model";
import {Router} from "@angular/router";
import {CommonComponent} from "../../shared/components/common.component";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {host} from "../../shared/models/constants";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private http: HttpClient,
    private snackbar: MatSnackBar
  ) {
    this.form = this.fb.group({
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      userName: [null, [Validators.required]],
      companyName: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(8)]],
      phoneNumber: [null]
    })
  }

  ngOnInit(): void {
  }

  errorMessage = (control1part: string, control2part: string = '') =>
    CommonComponent.getErrorMessage(this.form, control1part, control2part)

  submit() {
    if (this.form.invalid)
      return

    this.submitted = true

    const user: UserForRegistrationModel = {
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
      userName: this.form.value.userName,
      companyName: this.form.value.companyName,
      email: this.form.value.email,
      password: this.form.value.password,
      phoneNumber: this.form.value.phoneNumber
    }

    this.http.post<UserForRegistrationModel>(`${host}/users`,user).subscribe(() => {
      this.form.reset()
      this.router.navigate(['/auth', 'login'])
      this.submitted = false
    }, (error: HttpErrorResponse) => {
      console.log(error);
      this.displaySnackbars(error)
      this.submitted = false
    })
  }

  public displaySnackbars(error: HttpErrorResponse) {
    let errorsArr: string[] = []
    if (error.error.DuplicateUserName)
      errorsArr.push(error.error.DuplicateUserName[0])

    if (error.error.DuplicateEmail)
      errorsArr.push(error.error.DuplicateEmail[0])

    for (let error of errorsArr){
      console.log(error);
      this.snackbar.open(error, 'Cancel')
    }
  }
}
