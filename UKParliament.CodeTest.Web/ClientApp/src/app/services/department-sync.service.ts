import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class DepartmentSyncService {
  private departmentChangedSource = new Subject<void>();
  departmentChanged$ = this.departmentChangedSource.asObservable();

  notifyDepartmentChanged() {
    this.departmentChangedSource.next();
  }
}
