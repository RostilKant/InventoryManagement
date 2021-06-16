import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {host} from "../shared/models/constants";
import {LicenseModel} from "../shared/models/license/license.model";
import {LicenseForCreationModel} from "../shared/models/license/licenseForCreation.model";
import {LicenseForEditingModel} from "../shared/models/license/licenseForEditing.model";

@Injectable()
export class LicenseService {

  constructor(private http: HttpClient) {
  }

  getAll(searchTerm: string = '', orderBy: string = '', pageNumber: number = 1, pageSize: number = 10,
         filterBy: string[] = [], filterByValue: string[] = [])
    : Observable<LicenseModel[]> {
    let params: HttpParams = new HttpParams()
    params = params.append('pageNumber', pageNumber)
    params = params.append('pageSize', pageSize)
    params = params.append('searchTerm', searchTerm)
    params = params.append('orderBy', orderBy)
    for (let i = 0; i < filterBy.length; i++) {
      params = params.append(filterBy[i], filterByValue[i])
    }

    return this.http.get<LicenseModel[]>(`${host}/licenses`, {
      params
    })
  }

  getById(id: string): Observable<LicenseModel> {
    return this.http.get<LicenseModel>(`${host}/licenses/${id}`)
  }

  create(license: LicenseForCreationModel): Observable<LicenseModel> {
    return this.http.post<LicenseModel>(`${host}/licenses`, license)
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${host}/licenses/${id}`);
  }

  update(id: string, license: LicenseForEditingModel): Observable<LicenseModel> {
    return this.http.put<LicenseModel>(`${host}/licenses/${id}`, license);
  }
}
