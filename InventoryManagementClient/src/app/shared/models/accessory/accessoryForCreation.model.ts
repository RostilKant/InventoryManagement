import {AccessoryCategory} from "../../enums/accessory-category.enum";
import {AssetStatus} from "../../enums/asset-status.enum";

export interface AccessoryForCreationModel{
  name: string,
  category: AccessoryCategory,
  status: AssetStatus,
  modelNumber: string,
  manufacturer: string,
  officeAddress: string,
  purchaseCost: number,
  purchaseDate: Date
}
