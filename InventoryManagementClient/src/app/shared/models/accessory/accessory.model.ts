import {AssetStatus} from "../../enums/asset-status.enum";
import {AccessoryCategory} from "../../enums/accessory-category.enum";

export interface AccessoryModel{
  id: string,
  name: string,
  category: AccessoryCategory,
  status: AssetStatus,
  modelNumber: string,
  manufacturer: string,
  officeAddress: string,
  purchaseCost: number,
  purchaseDate: Date
}
