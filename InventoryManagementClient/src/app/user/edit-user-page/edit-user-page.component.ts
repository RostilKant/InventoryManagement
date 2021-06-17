import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {MatSnackBar} from "@angular/material/snack-bar";
import {CommonComponent} from "../../shared/components/common.component";
import {UserForRegistrationModel} from "../../shared/models/user/userForRegistration.model";
import {host} from "../../shared/models/constants";
import {UserService} from "../user.service";
import {UserForEditingModel} from "../../shared/models/user/userForEditing.model";
import {ChangePasswordModel} from "../../shared/models/user/changePassword.model";

@Component({
  selector: 'app-edit-user-page',
  templateUrl: './edit-user-page.component.html',
  styleUrls: ['./edit-user-page.component.scss']
})
export class EditUserPageComponent implements OnInit {

  updateForm: FormGroup
  passForm: FormGroup
  submitted: boolean = false

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private userService: UserService,
    private snackbar: MatSnackBar
  ) {
    this.updateForm = this.fb.group({
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      userName: [null, [Validators.required]],
      companyName: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.email]],
      phoneNumber: [null]
    })

    this.passForm = this.fb.group({
      oldPassword: [null, Validators.required],
      newPassword: [null, Validators.required]
    })

    this.userService.getUser().subscribe((user: UserForEditingModel) => {
      this.updateForm.setValue({
        firstName: user.firstName,
        lastName: user.lastName,
        userName: user.userName,
        companyName: user.companyName,
        email: user.email,
        phoneNumber: user.phoneNumber
      })
    })

  }

  ngOnInit(): void {
  }

  errorMessage = (control1part: string, control2part: string = '') =>
    CommonComponent.getErrorMessage(this.updateForm, control1part, control2part)

  submitUpdate() {
    if (this.updateForm.invalid)
      return

    this.submitted = true

    const user: UserForEditingModel = {
      firstName: this.updateForm.value.firstName,
      lastName: this.updateForm.value.lastName,
      userName: this.updateForm.value.userName,
      companyName: this.updateForm.value.companyName,
      email: this.updateForm.value.email,
      phoneNumber: this.updateForm.value.phoneNumber
    }

    this.userService.updateUser(user).subscribe(() => {
      this.updateForm.setValue({
        firstName: user.firstName,
        lastName: user.lastName,
        userName: user.userName,
        companyName: user.companyName,
        email: user.email,
        phoneNumber: user.phoneNumber
      })
      this.snackbar.open('User has been updated', 'Cancel')
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

  submitPass() {
    if (this.passForm.invalid)
      return

    this.submitted = true

    const changePass: ChangePasswordModel = {
      oldPassword: this.passForm.value.oldPassword,
      newPassword: this.passForm.value.newPassword
    }

    this.userService.changeUserPassword(changePass).subscribe(() => {
      this.snackbar.open('Password has been changed', 'Cancel')
      this.passForm.reset()
    }, error => {
      console.log(error)
    })
  }
}
