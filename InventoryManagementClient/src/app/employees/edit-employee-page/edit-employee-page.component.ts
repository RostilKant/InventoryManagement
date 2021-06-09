import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {CommonComponent} from "../../shared/components/common.component";
import {EmployeeService} from "../employee.service";
import {EmployeeForEditingModel} from "../../shared/models/employee/employeeForEditing.model";
import {Subscription} from "rxjs";
import {switchMap} from "rxjs/operators";
import {EmployeeModel} from "../../shared/models/employee/employee.model";
import {HttpErrorResponse} from "@angular/common/http";
import {EmployeeDepartment} from "../../shared/enums/employee-department.enum";

@Component({
  selector: 'app-edit-employee-page',
  templateUrl: './edit-employee-page.component.html',
  styleUrls: ['./edit-employee-page.component.scss']
})
export class EditEmployeePageComponent implements OnInit, OnDestroy {

  form: FormGroup
  submitted: boolean = false
  currentEmployee: EmployeeModel | undefined
  currentEmployeeId: string = ''
  employeeDepartments: string[] = []
  updateSub: Subscription = new Subscription()

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private employeeService: EmployeeService
  ) {
    this.employeeDepartments = CommonComponent.getEnumNames(EmployeeDepartment)

    // noinspection DuplicatedCode
    this.form = this.fb.group({
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      job: [null, [Validators.required]],
      phone: [null, [Validators.required]],
      department: [null, [Validators.required]],
      officeAddress: [null],
      employmentDate: [null, [Validators.required]],
      address: [null, [Validators.required]],
      city: [null, [Validators.required]],
      state: [null, [Validators.required]],
      country: [null, [Validators.required]],
      zipCode: [null, [Validators.required]],
    })

    this.route.params
      .pipe(switchMap((params: Params) => {
        return this.employeeService.getById(params['id'])
      }))
      .subscribe((employee) => {
        this.currentEmployeeId = employee.id
        let firstName = employee.fullName.split(' ')[0]
        let lastName = employee.fullName.split(' ')[1]

        this.form.setValue({
          firstName: firstName,
          lastName: lastName,
          job: employee.job,
          phone: employee.phone,
          department: employee.department,
          officeAddress: employee.officeAddress,
          employmentDate: employee.employmentDate,
          address: employee.address,
          city: employee.city,
          state: employee.state,
          country: employee.country,
          zipCode: employee.zipCode
        })
        console.log(firstName, lastName)
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

    const employeeForEditing: EmployeeForEditingModel = {
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
      job: this.form.value.job,
      phone: this.form.value.phone,
      department: this.form.value.department,
      officeAddress: this.form.value.officeAddress,
      employmentDate: this.form.value.employmentDate,
      address: this.form.value.address,
      city: this.form.value.city,
      state: this.form.value.state,
      country: this.form.value.country,
      zipCode: this.form.value.zipCode
    }
    console.log(employeeForEditing);

    this.updateSub = this.employeeService.update(this.currentEmployeeId, employeeForEditing).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/employees'])
    }, (error: HttpErrorResponse) => {
      this.submitted = false
      console.log(error)
    })
  }

  ngOnDestroy(){
    if (this.updateSub)
      this.updateSub.unsubscribe()
  }
}
