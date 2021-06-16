import { Component, OnInit } from '@angular/core';
import {LicenseCategory} from "../../shared/enums/license-category.enum";
import {LicenseModel} from "../../shared/models/license/license.model";
import {LicenseForEditingModel} from "../../shared/models/license/licenseForEditing.model";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {switchMap} from "rxjs/operators";
import {CommonComponent} from "../../shared/components/common.component";
import {LicenseService} from "../license.service";

@Component({
  selector: 'app-edit-license-page',
  templateUrl: './edit-license-page.component.html',
  styleUrls: ['./edit-license-page.component.scss']
})
export class EditLicensePageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  licenseCategories: string[] = []
  currentLicenseId: string = ''

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
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
      purchaseCost: [null, [Validators.required]],
      purchaseDate: [null, [Validators.required]],
      isReAssignable: [false]
    })

    this.route.params
      .pipe(switchMap((params: Params) => {
        return this.licenseService.getById(params['id'])
      }))
      .subscribe((license: LicenseModel) => {
        this.currentLicenseId = license.id

        this.form.setValue({
          name: license.name,
          category: license.category,
          productKey: license.productKey,
          licensedToEmail: license.licensedToEmail,
          expiresAt: license.expiresAt,
          manufacturer: license.manufacturer,
          purchaseCost: license.purchaseCost,
          purchaseDate: license.purchaseDate,
          isReAssignable: license.isReAssignable
        })
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

    const licenseForEditingModel: LicenseForEditingModel = {
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

    this.licenseService.update(this.currentLicenseId, licenseForEditingModel).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/licenses'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }
}
