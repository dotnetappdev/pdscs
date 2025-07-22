import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';
import { Department } from '../../models/department.model';
import { DepartmentService } from '../../services/department.service';
import { HttpHeaders } from '@angular/common/http';

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
  fieldErrors: { [key: string]: string } = {}; // Store field-specific errors

  constructor(
    private personService: PersonService,
    private departmentService: DepartmentService,
    private cdr: ChangeDetectorRef
  ) {

  }

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
        console.log('Received persons:', persons); // Debug log added
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
        DOB: this.selectedPerson?.DOB ? this.formatDateForBackend(this.selectedPerson?.DOB) : null
      };
      console.log('Payload sent to backend:', payload);

      const isEdit = typeof this.selectedPerson.id === 'number' && this.selectedPerson.id > 0;

      const handleError = (err: any) => {
        console.error('Error saving person:', err);
        this.errorMessage = this.formatError(err);
        this.fieldErrors = {};
        // Map backend PascalCase error keys to frontend camelCase
        const keyMap: { [key: string]: string } = {
          'FirstName': 'firstName',
          'LastName': 'lastName',
          'Description': 'description',
          'DepartmentId': 'departmentId',
          'DOB': 'DOB'
        };
        if (err?.error) {
          const errors = err.error.errors || err.error.ModelState || err.error;
          if (errors && typeof errors === 'object') {
            Object.keys(errors).forEach(key => {
              const val = Array.isArray(errors[key]) ? errors[key].join(' ') : errors[key];
              const mappedKey = keyMap[key] || key;
              this.fieldErrors[mappedKey] = val;
            });
          }
        }
      };

      const handleSuccess = () => {
        console.log('Save successful, refreshing persons list');
        this.selectedPerson = null;  // clear selection
        this.getAllPersons();        // reload fresh data
        this.fieldErrors = {};      // clear field errors on success
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
    if (!person.DOB) {
      this.selectedPerson.DOB = null; // Ensure date picker is empty if DOB is missing
    }
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
      DOB: null, // Ensure date picker is empty
      departmentId: 0,
      description: ''
    };
  }

  onCancelEdit() {
    this.selectedPerson = null;
    this.fieldErrors = {}; // clear field errors on cancel
  }
}
