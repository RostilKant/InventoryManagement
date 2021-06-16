import { Component, OnInit } from '@angular/core';
import {LicenseService} from "../license.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {CommonComponent} from "../../shared/components/common.component";
import {LicenseCategory} from "../../shared/enums/license-category.enum";
import {LicenseForCreationModel} from "../../shared/models/license/licenseForCreation.model";

@Component({
  selector: 'app-create-license-page',
  templateUrl: './create-license-page.component.html',
  styleUrls: ['./create-license-page.component.scss']
})
export class CreateLicensePageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  licenseCategories: string[] = []

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private licenseService: LicenseService
  ) {
    this.licenseCategories = CommonComponent.getEnumNames(LicenseCategory)

    this.form = this.fb.group({
      name: [null, [Validators.required]],
      category: [null, [Validators.required]],
      productKey: [null],
      licensedToEmail: [null],
      expiresAt: [null, [Validators.required]],
      manufacturer: [null, [Validators.required]],
      officeAddress: [null],
      purchaseCost: [null, [Validators.required]],
      purchaseDate: [null, [Validators.required]],
      isReAssignable: [false]
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

    const licenseForCreationModel: LicenseForCreationModel = {
      name: this.form.value.name,
      category: this.form.value.category,
      productKey: this.form.value.productKey,
      licensedToEmail: this.form.value.licensedToEmail,
      expiresAt: this.form.value.expiresAt,
      manufacturer: this.form.value.manufacturer,
      purchaseCost: this.form.value.purchaseCost,
      purchaseDate: this.form.value.purchaseDate,
      isReAssignable: this.form.value.isReAssignable
    }

    this.licenseService.create(licenseForCreationModel).subscribe(() => {
      this.submitted = false
      console.log(licenseForCreationModel);
      this.router.navigate(['/licenses'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }
}
