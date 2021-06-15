import {ComponentCategory} from "../../enums/component-category.enum";
import {AssetStatus} from "../../enums/asset-status.enum";

export interface ComponentForEditingModel {
  name: string,
  serial: string,
  category: ComponentCategory,
  status: AssetStatus,
  manufacturer: string,
  officeAddress: string,
  purchaseCost: number,
  purchaseDate: Date
}
