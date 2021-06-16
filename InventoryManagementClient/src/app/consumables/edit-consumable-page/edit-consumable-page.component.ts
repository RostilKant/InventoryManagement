import { Component, OnInit } from '@angular/core';
import {ConsumableService} from "../consumable.service";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {switchMap} from "rxjs/operators";
import {AssetStatus} from "../../shared/enums/asset-status.enum";
import {CommonComponent} from "../../shared/components/common.component";
import {ConsumableCategory} from "../../shared/enums/consumable-category.enum";
import {ConsumableModel} from "../../shared/models/consumable/consumable.model";
import {ConsumableForEditingModel} from "../../shared/models/consumable/consumableForEditing.model";

@Component({
  selector: 'app-edit-consumable-page',
  templateUrl: './edit-consumable-page.component.html',
  styleUrls: ['./edit-consumable-page.component.scss']
})
export class EditConsumablePageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  consumableCategories: string[] = []
  assetStatuses: string[] = []
  currentConsumableId: string = ''

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
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
      purchaseDate: [null, [Validators.required]]
    })

    this.route.params
      .pipe(switchMap((params: Params) => {
        return this.consumableService.getById(params['id'])
      }))
      .subscribe((component: ConsumableModel) => {
        this.currentConsumableId = component.id

        this.form.setValue({
          name: component.name,
          category: component.category,
          status: component.status,
          manufacturer: component.manufacturer,
          officeAddress: component.officeAddress,
          purchaseCost: component.purchaseCost,
          purchaseDate: component.purchaseDate,
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

    const consumableForEditingModel: ConsumableForEditingModel = {
      name: this.form.value.name,
      category: this.form.value.category,
      status: this.form.value.status,
      manufacturer: this.form.value.manufacturer,
      officeAddress: this.form.value.officeAddress,
      purchaseCost: this.form.value.purchaseCost,
      purchaseDate: this.form.value.purchaseDate,
    }

    this.consumableService.update(this.currentConsumableId, consumableForEditingModel).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/consumables'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }

}
