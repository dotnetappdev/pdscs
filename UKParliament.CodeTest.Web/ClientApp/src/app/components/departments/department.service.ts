import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Department } from './department.model';

@Injectable({ providedIn: 'root' })
export class DepartmentService {
  private apiUrl = '/api/department';

  constructor(private http: HttpClient) { }

  getDepartments(): Observable<Department[]> {
    console.log("API URL " + this.apiUrl);
    return this.http.get<Department[]>(this.apiUrl);
  }

  addDepartment(dept: Department): Observable<Department> {
    return this.http.post<Department>(this.apiUrl, dept);
  }

  updateDepartment(dept: Department): Observable<Department> {
    return this.http.put<Department>(`${this.apiUrl}/${dept.id}`, dept);
  }

  deleteDepartment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
