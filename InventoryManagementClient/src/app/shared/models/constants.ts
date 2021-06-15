import {Provider} from "@angular/core";
import {HTTP_INTERCEPTORS} from "@angular/common/http";
import {AuthInterceptor} from "../serviсes/auth.interceptor";

export const host: string = 'https://localhost:5001/api'

export const INTERCEPTOR_PROVIDER: Provider = {
  provide: HTTP_INTERCEPTORS,
  multi: true,
  useClass: AuthInterceptor
};

export const employeesDisplayedColumns = new Map<number, string>()
  .set(1, 'fullName')
  .set(2, 'job')
  .set(3, 'department')
  .set(4, 'employmentDate')
  .set(6, 'country')
  .set(8, 'city')

export const employeesColumns: string[] = ['id', 'fullName', 'job', 'department', 'employmentDate',
  'address', 'country', 'state', 'city', 'zipCode']

export const devicesDisplayedColumns = new Map<number, string>()
  .set(1, 'model')
  .set(2, 'serial')
  .set(3, 'category')
  .set(5, 'manufacturer')
  .set(6, 'officeAddress')
  .set(7, 'purchaseCost')

export const devicesColumns: string[] = ['id', 'model', 'serial', 'category', 'status',
  'manufacturer', 'officeAddress', 'purchaseCost', 'purchaseDate', 'updateCost', 'lastUpdateDate',
  'warranty', 'warrantyExpires', 'imei', 'macAddress', 'notes']

export const accessoriesDisplayedColumns = new Map<number, string>()
  .set(1, 'name')
  .set(2, 'category')
  .set(5, 'manufacturer')
  .set(6, 'officeAddress')
  .set(7, 'purchaseCost')

export const accessoriesColumns: string[] = ['id', 'name', 'category', 'status', 'modelNumber',
  'manufacturer', 'officeAddress', 'purchaseCost', 'purchaseDate']



