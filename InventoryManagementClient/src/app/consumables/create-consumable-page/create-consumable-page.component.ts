import { Component, OnInit } from '@angular/core';
import {ConsumableService} from "../consumable.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {AssetStatus} from "../../shared/enums/asset-status.enum";
import {CommonComponent} from "../../shared/components/common.component";
import {ConsumableCategory} from "../../shared/enums/consumable-category.enum";
import {ConsumableForCreationModel} from "../../shared/models/consumable/consumableForCreation.model";

@Component({
  selector: 'app-create-consumable-page',
  templateUrl: './create-consumable-page.component.html',
  styleUrls: ['./create-consumable-page.component.scss']
})
export class CreateConsumablePageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  consumableCategories: string[] = []
  assetStatuses: string[] = []

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private consumableService: ConsumableService
  ) {
    this.consumableCategories = CommonComponent.getEnumNames(ConsumableCategory)
    this.assetStatuses = CommonComponent.getEnumNames(AssetStatus)

    this.form = this.fb.group({
      name: [null, [Validators.required]],
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

    const consumableForCreationModel: ConsumableForCreationModel = {
      name: this.form.value.name,
      category: this.form.value.category,
      status: this.form.value.status,
      manufacturer: this.form.value.manufacturer,
      officeAddress: this.form.value.officeAddress,
      purchaseCost: this.form.value.purchaseCost,
      purchaseDate: this.form.value.purchaseDate
    }

    this.consumableService.create(consumableForCreationModel).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/consumables'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }
}
