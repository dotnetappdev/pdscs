import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Person } from './person.model';

@Injectable({ providedIn: 'root' })
export class PersonsService {
  private apiUrl = '/api/person';

  constructor(private http: HttpClient) {}

  getPersons(): Observable<Person[]> {
    return this.http.get<Person[]>(this.apiUrl);
  }

  addPerson(person: Person): Observable<Person> {
    return this.http.post<Person>(this.apiUrl, person);
  }

  updatePerson(id: number, person: any): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, person);
  }

  deletePerson(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
