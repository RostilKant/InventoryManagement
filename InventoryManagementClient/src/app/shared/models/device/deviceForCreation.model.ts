import {DeviceCategory} from "../../enums/device-category.enum";

export interface DeviceForCreationModel {
  model: string,
  serial: string,
  category: DeviceCategory,
  status: string,
  manufacturer: string,
  officeAddress: string,
  purchaseCost: number,
  purchaseDate: Date,
  updateCost: number,
  lastUpdateDate: Date,
  warranty: string,
  warrantyExpires: Date,
  imei: string,
  macAddress: string,
  notes: string
}
