import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {switchMap} from "rxjs/operators";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {AssetStatus} from "../../shared/enums/asset-status.enum";
import {CommonComponent} from "../../shared/components/common.component";
import {AccessoryCategory} from "../../shared/enums/accessory-category.enum";
import {AccessoryService} from "../accessory.service";
import {AccessoryModel} from "../../shared/models/accessory/accessory.model";
import {AccessoryForEditingModel} from "../../shared/models/accessory/accessoryForEditing.model";

@Component({
  selector: 'app-edit-accessory-page',
  templateUrl: './edit-accessory-page.component.html',
  styleUrls: ['./edit-accessory-page.component.scss']
})
export class EditAccessoryPageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  accessoryCategories: string[] = []
  assetStatuses: string[] = []
  currentAccessoryId: string = ''

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
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
      purchaseDate: [null, [Validators.required]]
    })

    this.route.params
      .pipe(switchMap((params: Params) => {
        return this.accessoryService.getById(params['id'])
      }))
      .subscribe((accessory: AccessoryModel) => {
        this.currentAccessoryId = accessory.id

        this.form.setValue({
          name: accessory.name,
          category: accessory.category,
          status: accessory.status,
          modelNumber: accessory.modelNumber,
          manufacturer: accessory.manufacturer,
          officeAddress: accessory.officeAddress,
          purchaseCost: accessory.purchaseCost,
          purchaseDate: accessory.purchaseDate,
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

    const accessoryForEditingModel: AccessoryForEditingModel = {
      name: this.form.value.name,
      category: this.form.value.category,
      status: this.form.value.status,
      modelNumber: this.form.value.modelNumber,
      manufacturer: this.form.value.manufacturer,
      officeAddress: this.form.value.officeAddress,
      purchaseCost: this.form.value.purchaseCost,
      purchaseDate: this.form.value.purchaseDate,
    }

    this.accessoryService.update(this.currentAccessoryId, accessoryForEditingModel).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/accessories'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }
}
