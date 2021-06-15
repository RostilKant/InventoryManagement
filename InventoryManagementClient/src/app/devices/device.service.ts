import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {Observable} from "rxjs";
import {host} from "../shared/models/constants";
import {DeviceModel} from "../shared/models/device/device.model";
import {DeviceForEditingModel} from "../shared/models/device/deviceForEditing.model";
import {DeviceForCreationModel} from "../shared/models/device/deviceForCreation.model";

@Injectable()
export class DeviceService {

  constructor(
    private http: HttpClient
  ) {
  }

  getAll(searchTerm: string = '', orderBy: string = '', pageNumber: number = 1, pageSize: number = 10,
         filterBy: string[] = [], filterByValue: string[] = [])
    : Observable<DeviceModel[]> {
    let params: HttpParams = new HttpParams()
    params = params.append('pageNumber', pageNumber)
    params = params.append('pageSize', pageSize)
    params = params.append('searchTerm', searchTerm)
    params = params.append('orderBy', orderBy)
    for (let i = 0; i < filterBy.length; i++){
      params = params.append(filterBy[i], filterByValue[i])
    }

    return this.http.get<DeviceModel[]>(`${host}/devices`, {
      params
    })
  }

  getById(id: string): Observable<DeviceModel> {
    return this.http.get<DeviceModel>(`${host}/devices/${id}`)
  }

  create(device: DeviceForCreationModel): Observable<DeviceModel> {
    return this.http.post<DeviceModel>(`${host}/devices`, device)
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${host}/devices/${id}`);
  }

  update(id: string, device: DeviceForEditingModel): Observable<DeviceModel> {
    return this.http.put<DeviceModel>(`${host}/devices/${id}`, device);
  }
}
