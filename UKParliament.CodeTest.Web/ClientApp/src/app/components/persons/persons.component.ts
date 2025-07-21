import { Component, OnInit } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-persons',
  templateUrl: './persons.component.html',
  styleUrls: ['./persons.component.scss']
})
export class PersonsComponent implements OnInit {
  persons: PersonViewModel[] = [];
  errorMessage: string = '';

  constructor(private personService: PersonService) { }

  ngOnInit(): void {
    this.getPersonById(1);
    this.getAllPersons();
  }

  getPersonById(id: number): void {
    this.personService.getById(id).subscribe({
      next: (result) => console.info(`User returned: ${JSON.stringify(result)}`),
      error: (e) => {
        console.error('Error fetching person by ID:', e);
        this.errorMessage = this.formatError(e);
      }
    });
  }

  getAllPersons(): void {
    this.personService.getAll().subscribe({
      next: (data) => this.persons = data,
      error: (err) => {
        console.error('Error fetching all persons:', err);
        this.errorMessage = this.formatError(err);
      }
    });
  }

  private formatError(error: any): string {
    if (error.error && error.error.message) {
      // Backend returns an error object with message
      return `Error: ${error.error.message}`;
    } else if (error.message) {
      // Generic error message property
      return `Error: ${error.message}`;
    } else if (typeof error === 'string') {
      return `Error: ${error}`;
    } else {
      // Fallback stringify entire error
      return `Error: ${JSON.stringify(error)}`;
    }
  }
}
