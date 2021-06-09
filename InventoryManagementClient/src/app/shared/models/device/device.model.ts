import {DeviceCategory} from "../../enums/device-category.enum";

export interface DeviceModel {
  model: string,
  serial: string,
  category: DeviceCategory,
  status: "Archived",
  manufacturer: "string",
  officeAddress: "string",
  purchaseCost: 0,
  purchaseDate: "2021-06-09T17:39:45.630Z",
  updateCost: 0,
  lastUpdateDate: "2021-06-09T17:39:45.630Z",
  warranty: "string",
  warrantyExpires: "2021-06-09T17:39:45.630Z",
  imei: "string",
  macAddress: "string",
  notes: "string"
}
