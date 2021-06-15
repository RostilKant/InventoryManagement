import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {host} from "../shared/models/constants";
import {ComponentModel} from "../shared/models/component/component.model";
import {ComponentForCreationModel} from "../shared/models/component/componentForCreation.model";
import {ComponentForEditingModel} from "../shared/models/component/componentForEditing.model";

@Injectable({
  providedIn: 'root'
})
export class ComponentService {

  constructor(private http: HttpClient) {
  }

  getAll(searchTerm: string = '', orderBy: string = '', pageNumber: number = 1, pageSize: number = 10,
         filterBy: string[] = [], filterByValue: string[] = [])
    : Observable<ComponentModel[]> {
    let params: HttpParams = new HttpParams()
    params = params.append('pageNumber', pageNumber)
    params = params.append('pageSize', pageSize)
    params = params.append('searchTerm', searchTerm)
    params = params.append('orderBy', orderBy)
    for (let i = 0; i < filterBy.length; i++) {
      params = params.append(filterBy[i], filterByValue[i])
    }

    return this.http.get<ComponentModel[]>(`${host}/components`, {
      params
    })
  }

  getById(id: string): Observable<ComponentModel> {
    return this.http.get<ComponentModel>(`${host}/components/${id}`)
  }

  create(component: ComponentForCreationModel): Observable<ComponentModel> {
    return this.http.post<ComponentModel>(`${host}/components`, component)
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${host}/components/${id}`);
  }

  update(id: string, component: ComponentForEditingModel): Observable<ComponentModel> {
    return this.http.put<ComponentModel>(`${host}/components/${id}`, component);
  }
}
