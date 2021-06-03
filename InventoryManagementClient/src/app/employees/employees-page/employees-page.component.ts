import { Component, OnInit } from '@angular/core';
import {EmployeeService} from "../employee.service";
import {EmployeeModel} from "../../shared/models/employee/employee.model";
import {delay} from "rxjs/operators";
import {Subscription} from "rxjs";
import {PageEvent} from "@angular/material/paginator";
import {Router} from "@angular/router";

@Component({
  selector: 'app-employees-page',
  templateUrl: './employees-page.component.html',
  styleUrls: ['./employees-page.component.scss']
})
export class EmployeesPageComponent implements OnInit {

  isLoading: boolean = false
  employees: EmployeeModel[] = []
  employeesCount: number = 0
  displayedColumns: string[] = ['fullName', 'job', 'department', 'employmentDate', 'address', 'country',
    'state', 'city', 'zipCode', 'actions']
  searchTerm: string = ''
  pageNumber: number = 1
  pageSize: number = 10

  getAllSub: Subscription = new Subscription()
  searchAllSub: Subscription = new Subscription()
  paginateAllSub: Subscription = new Subscription()
  deleteSub: Subscription = new Subscription()

  constructor(
    private employeeService: EmployeeService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.isLoading = true
    this.getAllSub =  this.employeeService.getAll(this.searchTerm, this.pageNumber, this.pageSize)
      .pipe(delay(1000))
      .subscribe((response: EmployeeModel[]) => {
        this.employees = response
        this.isLoading = false
      }, error => {
        console.log(error)
        if (error.status == 401){
          this.router.navigate(['/auth', 'login'])
        }
      })
    this.employeeService.getAll('', 1,10000000)
      .subscribe((response : EmployeeModel[]) => {
        this.employeesCount = response.length
      })
  }



  ngOnDestroy(): void {
    if (this.getAllSub){
      this.getAllSub.unsubscribe()
    }
    if (this.searchAllSub){
      this.searchAllSub.unsubscribe()
    }
    if (this.paginateAllSub){
      this.paginateAllSub.unsubscribe()
    }
  }

  remove(id: string) {
    this.deleteSub = this.employeeService.delete(id)
      .subscribe(() => {
        this.employees = this.employees.filter(x => x.id != id)
        this.employeesCount = this.employees.length
      }, error => {
        console.log(error);
      })
  }

  makeSearchTerm($event: InputEvent) {
    console.log($event)
    if ($event.inputType != 'deleteContentBackward' && $event.data != null)
      this.searchTerm += ($event.data)
    else
      this.searchTerm = this.searchTerm.slice(0,-1)

    console.log(this.searchTerm)
  }

  search() {
    this.searchAllSub = this.employeeService.getAll(this.searchTerm, this.pageNumber, this.pageSize)
      .subscribe((response) => {
        this.employees = response
        console.log(response)
      }, error => {
        console.log(error)
      })
  }

  paging($event: PageEvent) {
    this.pageSize = $event.pageSize
    this.pageNumber = ++$event.pageIndex
    this.paginateAllSub = this.employeeService.getAll(this.searchTerm,this.pageNumber, this.pageSize)
      .subscribe((response: EmployeeModel[]) => {
        this.employees = response
      })
  }


}
