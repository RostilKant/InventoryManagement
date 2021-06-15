import { Injectable } from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {host} from "../shared/models/constants";
import {AccessoryModel} from "../shared/models/accessory/accessory.model";
import {AccessoryForCreationModel} from "../shared/models/accessory/accessoryForCreation.model";
import {AccessoryForEditingModel} from "../shared/models/accessory/accessoryForEditing.model";

@Injectable()
export class AccessoryService {

  constructor(private http: HttpClient) { }

  getAll(searchTerm: string = '', orderBy: string = '', pageNumber: number = 1, pageSize: number = 10,
         filterBy: string[] = [], filterByValue: string[] = [])
    : Observable<AccessoryModel[]> {
    let params: HttpParams = new HttpParams()
    params = params.append('pageNumber', pageNumber)
    params = params.append('pageSize', pageSize)
    params = params.append('searchTerm', searchTerm)
    params = params.append('orderBy', orderBy)
    for (let i = 0; i < filterBy.length; i++){
      params = params.append(filterBy[i], filterByValue[i])
    }

    return this.http.get<AccessoryModel[]>(`${host}/accessories`, {
      params
    })
  }

  getById(id: string): Observable<AccessoryModel> {
    return this.http.get<AccessoryModel>(`${host}/accessories/${id}`)
  }

  create(accessory: AccessoryForCreationModel): Observable<AccessoryModel> {
    return this.http.post<AccessoryModel>(`${host}/accessories`, accessory)
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${host}/accessories/${id}`);
  }

  update(id: string, accessory: AccessoryForEditingModel): Observable<AccessoryModel> {
    return this.http.put<AccessoryModel>(`${host}/accessories/${id}`, accessory);
  }
}
