import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {CommonComponent} from "../../shared/components/common.component";
import {EmployeeService} from "../employee.service";
import {EmployeeForCreationModel} from "../../shared/models/employee/employeeForCreation.model";
import {EmployeeDepartment} from "../../shared/enums/employee-department.enum";

@Component({
  selector: 'app-create-employee-page',
  templateUrl: './create-employee-page.component.html',
  styleUrls: ['./create-employee-page.component.scss']
})
export class CreateEmployeePageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  employeeDepartments: string[] = []

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private employeeService: EmployeeService
  ) {
    this.employeeDepartments = CommonComponent.getEnumNames(EmployeeDepartment)

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
  }

  ngOnInit(): void {
  }

  errorMessage = (control1part: string, control2part: string = '') =>
    CommonComponent.getErrorMessage(this.form, control1part, control2part)

  submit() {
    if (this.form.invalid)
      return

    this.submitted = true

    const employeeForCreation: EmployeeForCreationModel = {
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

    this.employeeService.create(employeeForCreation).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/employees'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }
}
