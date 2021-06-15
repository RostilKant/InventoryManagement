import { Component, OnInit } from '@angular/core';
import {ComponentCategory} from "../../shared/enums/component-category.enum";
import {ComponentService} from "../component.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {AssetStatus} from "../../shared/enums/asset-status.enum";
import {CommonComponent} from "../../shared/components/common.component";
import {ComponentForCreationModel} from "../../shared/models/component/componentForCreation.model";

@Component({
  selector: 'app-create-component-page',
  templateUrl: './create-component-page.component.html',
  styleUrls: ['./create-component-page.component.scss']
})
export class CreateComponentPageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  componentCategories: string[] = []
  assetStatuses: string[] = []

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private componentService: ComponentService
  ) {
    this.componentCategories = CommonComponent.getEnumNames(ComponentCategory)
    this.assetStatuses = CommonComponent.getEnumNames(AssetStatus)

    this.form = this.fb.group({
      name: [null, [Validators.required]],
      serial: [null, [Validators.required]],
      category: [null, [Validators.required]],
      status: [null, [Validators.required]],
      manufacturer: [null, [Validators.required]],
      officeAddress: [null],
      purchaseCost: [null, [Validators.required]],
      purchaseDate: [null, [Validators.required]],
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

    const componentForCreationModel: ComponentForCreationModel = {
      name: this.form.value.name,
      serial: this.form.value.serial,
      category: this.form.value.category,
      status: this.form.value.status,
      manufacturer: this.form.value.manufacturer,
      officeAddress: this.form.value.officeAddress,
      purchaseCost: this.form.value.purchaseCost,
      purchaseDate: this.form.value.purchaseDate
    }

    this.componentService.create(componentForCreationModel).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/components'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }
}
