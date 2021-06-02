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

  create(employee: EmployeeForCreationModel): Observable<EmployeeModel>{
    return this.http.post<EmployeeModel>(`${host}/employees`, employee)
  }
}
