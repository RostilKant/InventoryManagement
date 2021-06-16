import {LicenseCategory} from "../../enums/license-category.enum";

export interface LicenseForCreationModel {
  name: string,
  category: LicenseCategory,
  productKey: string,
  licensedToEmail: string,
  expiresAt: Date,
  manufacturer: string,
  purchaseCost: number,
  purchaseDate: Date,
  isReAssignable: boolean
}
