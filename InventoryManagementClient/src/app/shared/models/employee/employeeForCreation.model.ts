import {EmployeeDepartment} from "../../enums/employee-department.enum";

export interface EmployeeForCreationModel {
  firstName: string,
  lastName: string,
  job: string,
  phone: string,
  department: EmployeeDepartment,
  officeAddress: string,
  employmentDate: Date,
  address: string,
  city: string,
  state: string,
  country: string,
  zipCode: string
}
