import {Component, OnInit} from '@angular/core';
import {
  consumablesColumns, consumablesDisplayedColumns
} from "../../shared/models/constants";
import {Subscription} from "rxjs";
import {Router} from "@angular/router";
import {delay} from "rxjs/operators";
import {PageEvent} from "@angular/material/paginator";
import {Sort} from "@angular/material/sort";
import {MatCheckboxChange} from "@angular/material/checkbox";
import {ConsumableService} from "../consumable.service";
import {ConsumableModel} from "../../shared/models/consumable/consumable.model";

@Component({
  selector: 'app-consumables-page',
  templateUrl: './consumables-page.component.html',
  styleUrls: ['./consumables-page.component.scss']
})
export class ConsumablesPageComponent implements OnInit {

  isLoading: boolean = false
  consumables: ConsumableModel[] = []
  consumablesCount: number = 0

  // changing fields feature
  displayedColumns = consumablesDisplayedColumns
  defaultColumns = consumablesColumns
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
    private consumableService: ConsumableService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.initColumnValues()

    this.isLoading = true
    this.getAllSub = this.consumableService.getAll(this.searchTerm, this.orderBy, this.pageNumber, this.pageSize)
      .pipe(delay(888))
      .subscribe((response: ConsumableModel[]) => {
        this.consumables = response
        this.isLoading = false
      }, error => {
        console.log(error)
        if (error.status == 401) {
          this.router.navigate(['/auth', 'login'])
        }
      })
    this.consumableService.getAll(this.searchTerm, this.orderBy, 1, 10000000)
      .subscribe((response: ConsumableModel[]) => {
        this.consumablesCount = response.length
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
    this.deleteSub = this.consumableService.delete(id)
      .subscribe(() => {
        this.consumables = this.consumables.filter(x => x.id != id)
        this.consumablesCount = this.consumables.length
      }, error => {
        console.log(error);
      })
  }

  search() {
    this.searchAllSub = this.consumableService.getAll(this.searchTerm, this.orderBy, this.pageNumber,
      this.pageSize, this.filterBy, this.filterByValue)
      .subscribe((response: ConsumableModel[]) => {
        this.consumablesCount = response.length
        this.consumables = response
        console.log(response)
      }, error => {
        console.log(error)
      })
  }

  paging($event: PageEvent) {
    this.pageSize = $event.pageSize
    this.pageNumber = ++$event.pageIndex
    this.paginateAllSub = this.consumableService.getAll(this.searchTerm, this.orderBy, this.pageNumber,
      this.pageSize, this.filterBy, this.filterByValue)
      .subscribe((response: ConsumableModel[]) => {
        this.consumables = response
      })
  }

  sortData($event: Sort) {
    if ($event.active != null && $event.direction)
      this.orderBy = `${$event.active} ${$event.direction}`
    else
      this.orderBy = ''

    this.consumableService.getAll(this.searchTerm, this.orderBy, this.pageNumber,
      this.pageSize, this.filterBy, this.filterByValue)
      .subscribe((response: ConsumableModel[]) => {
        this.consumables = response
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

    this.getAllSub = this.consumableService.getAll(this.searchTerm, this.orderBy, this.pageNumber,
      this.pageSize, this.filterBy, this.filterByValue)
      .subscribe((response: ConsumableModel[]) => {
        this.consumablesCount = response.length
        this.consumables = response
      }, error => console.log(error))
  }

  removeChip(chip: string) {
    let chipField = this.filterBy[this.filterByValue.indexOf(chip)]
    this.filterByValue = this.filterByValue.filter(x => x != chip)
    this.filterBy = this.filterBy.filter(x => x != chipField)
    this.filtering('', '')
  }
}
