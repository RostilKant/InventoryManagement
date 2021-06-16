import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {host} from "../shared/models/constants";
import {ConsumableModel} from "../shared/models/consumable/consumable.model";
import {ConsumableForEditingModel} from "../shared/models/consumable/consumableForEditing.model";
import {ConsumableForCreationModel} from "../shared/models/consumable/consumableForCreation.model";

@Injectable()
export class ConsumableService {

  constructor(private http: HttpClient) {
  }

  getAll(searchTerm: string = '', orderBy: string = '', pageNumber: number = 1, pageSize: number = 10,
         filterBy: string[] = [], filterByValue: string[] = [])
    : Observable<ConsumableModel[]> {
    let params: HttpParams = new HttpParams()
    params = params.append('pageNumber', pageNumber)
    params = params.append('pageSize', pageSize)
    params = params.append('searchTerm', searchTerm)
    params = params.append('orderBy', orderBy)
    for (let i = 0; i < filterBy.length; i++) {
      params = params.append(filterBy[i], filterByValue[i])
    }

    return this.http.get<ConsumableModel[]>(`${host}/consumables`, {
      params
    })
  }

  getById(id: string): Observable<ConsumableModel> {
    return this.http.get<ConsumableModel>(`${host}/consumables/${id}`)
  }

  create(consumable: ConsumableForCreationModel): Observable<ConsumableModel> {
    return this.http.post<ConsumableModel>(`${host}/consumables`, consumable)
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${host}/consumables/${id}`);
  }

  update(id: string, consumable: ConsumableForEditingModel): Observable<ConsumableModel> {
    return this.http.put<ConsumableModel>(`${host}/consumables/${id}`, consumable);
  }
}
