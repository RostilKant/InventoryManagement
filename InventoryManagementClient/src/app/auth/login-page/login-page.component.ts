import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../shared/serviсes/auth.service";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {UserForAuthenticationModel} from "../../shared/models/user/userForAuthentication.model";
import {Subject} from "rxjs";
import {CommonComponent} from "../../shared/components/common.component";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  error$: Subject<string> = this.auth.error$
  guardMessage: string = ''

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      email: [null, [Validators.required, Validators.email]],
      password: [null, [Validators.required, Validators.minLength(8)]]
    })

    route.queryParams.subscribe((params: Params) => {
      if (params['sessionExpired'])
        this.guardMessage = 'Session expired! Please, login again.'
      else if (params['pleaseLogin'])
        this.guardMessage = 'Please, login at first!'
    })
  }

  ngOnInit(): void {

  }

  errorMessage = (control: string) => CommonComponent.getErrorMessage(this.form, control)

  submit() {
    if (this.form.invalid)
      return

    this.submitted = true

    const user: UserForAuthenticationModel = {
      email: this.form.value.email,
      password: this.form.value.password
    }

    this.auth.login(user).subscribe(() => {
      this.form.reset()
      this.router.navigate(['/dashboard'])
      this.submitted = false
    }, error => {
      console.log(error)
      this.submitted = false
    })
  }
}
