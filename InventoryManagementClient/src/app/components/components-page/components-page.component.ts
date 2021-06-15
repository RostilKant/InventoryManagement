import {Component, OnInit} from '@angular/core';
import {
  componentsColumns,
  componentsDisplayedColumns
} from "../../shared/models/constants";
import {Subscription} from "rxjs";
import {Router} from "@angular/router";
import {delay} from "rxjs/operators";
import {PageEvent} from "@angular/material/paginator";
import {Sort} from "@angular/material/sort";
import {MatCheckboxChange} from "@angular/material/checkbox";
import {ComponentModel} from "../../shared/models/component/component.model";
import {ComponentService} from "../component.service";

@Component({
  selector: 'app-components-page',
  templateUrl: './components-page.component.html',
  styleUrls: ['./components-page.component.scss']
})
export class ComponentsPageComponent implements OnInit {

  isLoading: boolean = false
  components: ComponentModel[] = []
  componentsCount: number = 0

  // changing fields feature
  displayedColumns = componentsDisplayedColumns
  defaultColumns = componentsColumns
  displayedColumnsValues: Array<string> = new Array<string>(this.defaultColumns.length)
  removable: boolean = true

  searchTerm: string = ''
  orderBy: string = ''
  pageNumber: number = 1
  pageSize: number = 10
  filterBy: string[] = []
  filterByValue: string[] = []

  getAllSub: Subscription = new Subscription()
  searchAllSub: Subscription = new Subscription()
  paginateAllSub: Subscription = new Subscription()
  deleteSub: Subscription = new Subscription()

  constructor(
    private componentService: ComponentService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.initColumnValues()

    this.isLoading = true
    this.getAllSub = this.componentService.getAll(this.searchTerm, this.orderBy, this.pageNumber, this.pageSize)
      .pipe(delay(888))
      .subscribe((response: ComponentModel[]) => {
        this.components = response
        this.isLoading = false
      }, error => {
        console.log(error)
        if (error.status == 401) {
          this.router.navigate(['/auth', 'login'])
        }
      })
    this.componentService.getAll(this.searchTerm, this.orderBy, 1, 10000000)
      .subscribe((response: ComponentModel[]) => {
        this.componentsCount = response.length
      })
  }

  ngOnDestroy(): void {
    if (this.getAllSub) {
      this.getAllSub.unsubscribe()
    }
    if (this.searchAllSub) {
      this.searchAllSub.unsubscribe()
    }
    if (this.paginateAllSub) {
      this.paginateAllSub.unsubscribe()
    }
    if (this.deleteSub) {
      this.deleteSub.unsubscribe()
    }
  }

  remove(id: string) {
    this.deleteSub = this.componentService.delete(id)
      .subscribe(() => {
        this.components = this.components.filter(x => x.id != id)
        this.componentsCount = this.components.length
      }, error => {
        console.log(error);
      })
  }

  search() {
    this.searchAllSub = this.componentService.getAll(this.searchTerm, this.orderBy, this.pageNumber,
      this.pageSize, this.filterBy, this.filterByValue)
      .subscribe((response: ComponentModel[]) => {
        this.componentsCount = response.length
        this.components = response
        console.log(response)
      }, error => {
        console.log(error)
      })
  }

  paging($event: PageEvent) {
    this.pageSize = $event.pageSize
    this.pageNumber = ++$event.pageIndex
    this.paginateAllSub = this.componentService.getAll(this.searchTerm, this.orderBy, this.pageNumber,
      this.pageSize, this.filterBy, this.filterByValue)
      .subscribe((response: ComponentModel[]) => {
        this.components = response
      })
  }

  sortData($event: Sort) {
    if ($event.active != null && $event.direction)
      this.orderBy = `${$event.active} ${$event.direction}`
    else
      this.orderBy = ''

    this.componentService.getAll(this.searchTerm, this.orderBy, this.pageNumber,
      this.pageSize, this.filterBy, this.filterByValue)
      .subscribe((response: ComponentModel[]) => {
        this.components = response
      })
  }

  ifExists(column: string): boolean {
    return this.displayedColumnsValues.includes(column)
  }

  manipulateFields($event: MatCheckboxChange, idx: number) {
    if ($event.checked) {
      this.displayedColumns.set(idx, this.defaultColumns[idx])
      this.initColumnValues()
    } else {
      this.displayedColumns.delete(idx)
      this.initColumnValues()
    }

    console.log(this.displayedColumnsValues);
  }

  private initColumnValues() {
    this.displayedColumnsValues = []
    this.displayedColumns = new Map<number, string>([...this.displayedColumns.entries()]
      .sort((a, b) => a[0] - b[0]))

    for (const i of this.displayedColumns.keys()) {
      console.log(i)
      this.displayedColumnsValues.push(this.defaultColumns[i])
    }
    this.displayedColumnsValues.push('actions')
  }

  filtering(field: string, filterBy: string) {
    if (!this.filterBy.includes(field) && filterBy != '') {
      this.filterBy.push(field)
      this.filterByValue.push(filterBy)
    }

    this.getAllSub = this.componentService.getAll(this.searchTerm, this.orderBy, this.pageNumber,
      this.pageSize, this.filterBy, this.filterByValue)
      .subscribe((response: ComponentModel[]) => {
        this.componentsCount = response.length
        this.components = response
      }, error => console.log(error))
  }

  removeChip(chip: string) {
    let chipField = this.filterBy[this.filterByValue.indexOf(chip)]
    this.filterByValue = this.filterByValue.filter(x => x != chip)
    this.filterBy = this.filterBy.filter(x => x != chipField)
    this.filtering('', '')
  }
}
