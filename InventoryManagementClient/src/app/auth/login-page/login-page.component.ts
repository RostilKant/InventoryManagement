import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {

  form: FormGroup

  constructor(private fb: FormBuilder) {
    this.form = this.fb.group({
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(8)]]
    })
  }

  ngOnInit(): void {

  }

  getErrorMessage(control: string): string {
    if (this.form.get(control.toLocaleLowerCase())?.errors?.required) {
      return `${control} is required`
    }

    if (this.form.get(control.toLocaleLowerCase())?.errors?.minlength) {
      return `${control} must contain at least
      ${this.form.get('password')?.errors?.minlength.requiredLength} symbols`
    }

    if (this.form.get(control.toLocaleLowerCase())?.errors?.email) {
      return `${control} is invalid`
    }
    return ''
  }

  submit() {
    if (this.form.invalid)
      return
  }
}
