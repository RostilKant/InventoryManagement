import {DeviceCategory} from "../../enums/device-category.enum";
import {AssetStatus} from "../../enums/asset-status.enum";

export interface DeviceModel {
  id: string,
  model: string,
  serial: string,
  category: DeviceCategory,
  status: AssetStatus,
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
