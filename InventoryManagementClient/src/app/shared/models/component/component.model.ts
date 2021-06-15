import {AssetStatus} from "../../enums/asset-status.enum";
import {ComponentCategory} from "../../enums/component-category.enum";

export interface ComponentModel{
  id: string,
  name: string,
  serial: string,
  category: ComponentCategory,
  status: AssetStatus,
  manufacturer: string,
  officeAddress: string,
  purchaseCost: number,
  purchaseDate: Date
}
