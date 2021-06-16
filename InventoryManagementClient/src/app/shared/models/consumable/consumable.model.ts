import {AssetStatus} from "../../enums/asset-status.enum";
import {ConsumableCategory} from "../../enums/consumable-category.enum";

export interface ConsumableModel {
  id: string,
  name: string,
  category: ConsumableCategory,
  status: AssetStatus,
  manufacturer: string,
  officeAddress: string,
  purchaseCost: number,
  purchaseDate: Date
}
