import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {AssetStatus} from "../../shared/enums/asset-status.enum";
import {CommonComponent} from "../../shared/components/common.component";
import {AccessoryCategory} from "../../shared/enums/accessory-category.enum";
import {AccessoryService} from "../accessory.service";
import {AccessoryForCreationModel} from "../../shared/models/accessory/accessoryForCreation.model";

@Component({
  selector: 'app-create-accessory-page',
  templateUrl: './create-accessory-page.component.html',
  styleUrls: ['./create-accessory-page.component.scss']
})
export class CreateAccessoryPageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  accessoryCategories: string[] = []
  assetStatuses: string[] = []

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private accessoryService: AccessoryService
  ) {
    this.accessoryCategories = CommonComponent.getEnumNames(AccessoryCategory)
    this.assetStatuses = CommonComponent.getEnumNames(AssetStatus)

    this.form = this.fb.group({
      name: [null, [Validators.required]],
      category: [null, [Validators.required]],
      status: [null, [Validators.required]],
      modelNumber: [null, [Validators.required]],
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

    const accessoryForCreationModel: AccessoryForCreationModel = {
      name: this.form.value.name,
      category: this.form.value.category,
      status: this.form.value.status,
      modelNumber: this.form.value.modelNumber,
      manufacturer: this.form.value.manufacturer,
      officeAddress: this.form.value.officeAddress,
      purchaseCost: this.form.value.purchaseCost,
      purchaseDate: this.form.value.purchaseDate
    }

    this.accessoryService.create(accessoryForCreationModel).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/accessories'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }

}
