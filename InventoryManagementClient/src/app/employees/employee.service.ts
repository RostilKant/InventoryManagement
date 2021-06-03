import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {EmployeeForCreationModel} from "../shared/models/employee/employeeForCreation.model";
import {Observable} from "rxjs";
import {EmployeeModel} from "../shared/models/employee/employee.model";
import {host} from "../shared/models/constants";

@Injectable()
export class EmployeeService {

  constructor(
    private http: HttpClient
  ) {
  }

  getAll(searchTerm: string = '', pageNumber: number = 1, pageSize: number = 10)
    : Observable<EmployeeModel[]>{
    return this.http.get<EmployeeModel[]>(`${host}/employees`, {
      params: {
        pageNumber: pageNumber,
        pageSize: pageSize,
        searchTerm: searchTerm
      }
    })
  }

  create(employee: EmployeeForCreationModel): Observable<EmployeeModel>{
    return this.http.post<EmployeeModel>(`${host}/employees`, employee)
  }

  delete(id: string): Observable<void>{
    return this.http.delete<void>(`${host}/employees/${id}`);
  }

}
