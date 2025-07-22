import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Department } from '../models/department.model';

@Injectable({ providedIn: 'root' })
export class DepartmentService {
  // In-memory departments based on PersonManagerContext
  private departments: Department[] = [
    { Id: 1, Name: 'Sales' },
    { Id: 2, Name: 'Marketing' },
    { Id: 3, Name: 'Finance' },
    { Id: 4, Name: 'HR' }
  ];

  getDepartments(): Observable<Department[]> {
    return of(this.departments);
  }
}
