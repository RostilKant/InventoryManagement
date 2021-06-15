import {Injectable} from "@angular/core";
import {HttpClient, HttpParams} from "@angular/common/http";
import {EmployeeForCreationModel} from "../shared/models/employee/employeeForCreation.model";
import {Observable} from "rxjs";
import {EmployeeModel} from "../shared/models/employee/employee.model";
import {host} from "../shared/models/constants";
import {EmployeeForEditingModel} from "../shared/models/employee/employeeForEditing.model";

@Injectable()
export class EmployeeService {

  constructor(
    private http: HttpClient
  ) {
  }

  getAll(searchTerm: string = '', orderBy: string = '', pageNumber: number = 1, pageSize: number = 10,
         filterBy: string[] = [], filterByValue: string[] = [])
    : Observable<EmployeeModel[]> {
    let params: HttpParams = new HttpParams()
    params = params.append('pageNumber', pageNumber)
    params = params.append('pageSize', pageSize)
    params = params.append('searchTerm', searchTerm)
    params = params.append('orderBy', orderBy)
    for (let i = 0; i < filterBy.length; i++){
      params = params.append(filterBy[i], filterByValue[i])
    }

    return this.http.get<EmployeeModel[]>(`${host}/employees`, {
      params
    })
  }

  getById(id: string): Observable<EmployeeModel> {
    return this.http.get<EmployeeModel>(`${host}/employees/${id}`)
  }

  create(employee: EmployeeForCreationModel): Observable<EmployeeModel> {
    return this.http.post<EmployeeModel>(`${host}/employees`, employee)
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${host}/employees/${id}`);
  }

  update(id: string, employee: EmployeeForEditingModel): Observable<EmployeeModel> {
    return this.http.put<EmployeeModel>(`${host}/employees/${id}`, employee);
  }
}
