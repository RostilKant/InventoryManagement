import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {EmployeeForCreationModel} from "../shared/models/employee/employeeForCreation.model";
import {Observable} from "rxjs";
import {EmployeeModel} from "../shared/models/employee/employee.model";
import {host} from "../shared/models/constants";
import {EmployeeForEditingModel} from "../shared/models/employee/employeeForEditing.model";

@Injectable()
export class DeviceService {

  constructor(
    private http: HttpClient
  ) {
  }

  getAll(searchTerm: string = '', pageNumber: number = 1, pageSize: number = 10)
    : Observable<EmployeeModel[]> {
    return this.http.get<EmployeeModel[]>(`${host}/employees`, {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize,
        searchTerm: searchTerm
      }
    })
  }
  getById(id: string): Observable<EmployeeModel>{
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