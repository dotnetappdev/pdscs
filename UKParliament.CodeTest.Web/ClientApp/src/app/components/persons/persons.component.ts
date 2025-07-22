import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';
import { Department } from '../../models/department.model';
import { DepartmentService } from '../../services/department.service';

@Component({
  selector: 'app-persons',
  templateUrl: './persons.component.html',
  styleUrls: ['./persons.component.scss']
})
export class PersonsComponent implements OnInit {
  persons: any[] = [];
  selectedPerson: any = null;
  errorMessage: string = '';
  departments: any[] = [];

  constructor(
    private personService: PersonService,
    private departmentService: DepartmentService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.getAllPersons();
    this.loadDepartments();
  }

  loadDepartments() {
    this.departmentService.getDepartments().subscribe({
      next: (depts) => {
        this.departments = depts;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error loading departments:', err);
        this.errorMessage = 'Failed to load departments.';
      }
    });
  }

  getAllPersons() {
    this.personService.getAll().subscribe({
      next: (persons) => {
        this.persons = persons;
        this.cdr.detectChanges(); // ensure UI updates after async fetch
      },
      error: (err) => {
        console.error('Error fetching persons:', err);
        this.errorMessage = 'Failed to load persons.';
      }
    });
  }

  formatDateForBackend(date: any): string {
    // Implement your date formatting here, e.g.:
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().split('T')[0]; // 'YYYY-MM-DD'
  }

  formatError(err: any): string {
    // Format error messages as needed
    return err?.message || 'Unknown error occurred';
  }

  onSave() {
    if (this.selectedPerson) {
      const payload: any = {
        FirstName: this.selectedPerson?.firstName ?? '',
        LastName: this.selectedPerson?.lastName ?? '',
        Description: this.selectedPerson?.description ?? '',
        DepartmentId: this.selectedPerson?.departmentId ?? 0,
        DOB: this.formatDateForBackend(this.selectedPerson?.DOB)
      };
      console.log('Payload sent to backend:', payload);

      const isEdit = typeof this.selectedPerson.id === 'number' && this.selectedPerson.id > 0;

      const handleError = (err: any) => {
        console.error('Error saving person:', err);
        this.errorMessage = this.formatError(err);
      };

      const handleSuccess = () => {
        console.log('Save successful, refreshing persons list');
        this.selectedPerson = null;  // clear selection
        this.getAllPersons();        // reload fresh data
      };

      if (isEdit) {
        this.personService.update(this.selectedPerson.id!, payload).subscribe({
          next: handleSuccess,
          error: handleError
        });
      } else {
        this.personService.add(payload).subscribe({
          next: handleSuccess,
          error: handleError
        });
      }
    }
  }

  onEdit(person: any) {
    // Clone to avoid editing the array object directly
    this.selectedPerson = { ...person };
  }

  onDelete(person: any) {
    if (confirm(`Delete ${person.fullName}?`)) {
      this.personService.delete(person.id).subscribe({
        next: () => this.getAllPersons(),
        error: err => {
          console.error('Error deleting person:', err);
          this.errorMessage = this.formatError(err);
        }
      });
    }
  }

  onAdd() {
    this.selectedPerson = {
      firstName: '',
      lastName: '',
      DOB: null,
      departmentId: 0,
      description: ''
    };
  }

  onCancelEdit() {
    this.selectedPerson = null;
  }
}
