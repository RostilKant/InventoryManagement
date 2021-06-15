import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ActivatedRoute, Params, Router} from "@angular/router";
import {DeviceService} from "../device.service";
import {CommonComponent} from "../../shared/components/common.component";
import {DeviceCategory} from "../../shared/enums/device-category.enum";
import {AssetStatus} from "../../shared/enums/asset-status.enum";
import {DeviceForEditingModel} from "../../shared/models/device/deviceForEditing.model";
import {switchMap} from "rxjs/operators";
import {DeviceModel} from "../../shared/models/device/device.model";

@Component({
  selector: 'app-edit-device-page',
  templateUrl: './edit-device-page.component.html',
  styleUrls: ['./edit-device-page.component.scss']
})
export class EditDevicePageComponent implements OnInit {

  form: FormGroup
  submitted: boolean = false
  deviceCategories: string[] = []
  assetStatuses: string[] = []
  currentDeviceId: string = ''

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private deviceService: DeviceService
  ) {
    this.deviceCategories = CommonComponent.getEnumNames(DeviceCategory)
    this.assetStatuses = CommonComponent.getEnumNames(AssetStatus)

    this.form = this.fb.group({
      model: [null, [Validators.required]],
      serial: [null, [Validators.required]],
      category: [null, [Validators.required]],
      status: [null, [Validators.required]],
      manufacturer: [null, [Validators.required]],
      officeAddress: [null],
      purchaseCost: [null, [Validators.required]],
      purchaseDate: [null, [Validators.required]],
      updateCost: [null],
      lastUpdateDate: [null],
      warranty: [null],
      warrantyExpires: [null],
      imei: [null],
      macAddress: [null],
      notes: [null]
    })

    this.route.params
      .pipe(switchMap((params: Params) => {
        return this.deviceService.getById(params['id'])
      }))
      .subscribe((device: DeviceModel) => {
        this.currentDeviceId = device.id

        this.form.setValue({
          model: device.model,
          serial: device.serial,
          category: device.category,
          status: device.status,
          manufacturer: device.manufacturer,
          officeAddress: device.officeAddress,
          purchaseCost: device.purchaseCost,
          purchaseDate: device.purchaseDate,
          updateCost: device.updateCost,
          lastUpdateDate: device.lastUpdateDate,
          warranty: device.warranty,
          warrantyExpires: device.warrantyExpires,
          imei: device.imei,
          macAddress: device.macAddress,
          notes: device.notes,
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

    const deviceForEditingModel: DeviceForEditingModel = {
      model: this.form.value.model,
      serial: this.form.value.serial,
      category: this.form.value.category,
      status: this.form.value.status,
      manufacturer: this.form.value.manufacturer,
      officeAddress: this.form.value.officeAddress,
      purchaseCost: this.form.value.purchaseCost,
      purchaseDate: this.form.value.purchaseDate,
      updateCost: this.form.value.updateCost,
      lastUpdateDate: this.form.value.lastUpdateDate,
      warranty: this.form.value.warranty,
      warrantyExpires: this.form.value.warrantyExpires,
      imei: this.form.value.imei,
      macAddress: this.form.value.macAddress,
      notes: this.form.value.notes
    }

    this.deviceService.update(this.currentDeviceId, deviceForEditingModel).subscribe(() => {
      this.submitted = false
      this.router.navigate(['/devices'])
    }, error => {
      this.submitted = false
      console.log(error)
    })
  }
}
