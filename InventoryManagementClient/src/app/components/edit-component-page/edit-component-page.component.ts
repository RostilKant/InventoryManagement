import { Component, OnInit } from '@angular/core';
import {ComponentService} from "../component.service";
import {ComponentCategory} from "../../shared/enums/component-category.enum";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {switchMap} from "rxjs/operators";
import {AssetStatus} from "../../shared/enums/asset-status.enum";
import {CommonComponent} from "../../shared/components/common.component";
import {ComponentModel} from "../../shared/models/component/component.model";
import {ComponentForEditingModel} from "../../shared/models/component/componentForEditing.model";

@Component({
  selector: 'app-edit-component-page',
  templateUrl: './edit-component-page.component.html',
  styleUrls: ['./edit-component-page.component.scss']
})
export class EditComponentPageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  componentCategories: string[] = []
  assetStatuses: string[] = []
  currentComponentId: string = ''

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
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
      purchaseDate: [null, [Validators.required]]
    })

    this.route.params
      .pipe(switchMap((params: Params) => {
        return this.componentService.getById(params['id'])
      }))
      .subscribe((component: ComponentModel) => {
        this.currentComponentId = component.id

        this.form.setValue({
          name: component.name,
          serial: component.serial,
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

    const componentForEditingModel: ComponentForEditingModel = {
      name: this.form.value.name,
      serial: this.form.value.serial,
      category: this.form.value.category,
      status: this.form.value.status,
      manufacturer: this.form.value.manufacturer,
      officeAddress: this.form.value.officeAddress,
      purchaseCost: this.form.value.purchaseCost,
      purchaseDate: this.form.value.purchaseDate,
    }

    this.componentService.update(this.currentComponentId, componentForEditingModel).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/components'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }

}
