// ...existing code...
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
  pageSize: number = 5; // Default page size is 5
  currentPage: number = 1;
  get pageSizeOptions(): number[] {
    // Always show 'All' (0) and fixed options: 5, 10, 20, 25
    return [0, 5, 10, 20, 25];
  }
  get pagedPersons(): any[] {
    // If pageSize equals total persons, show all filtered persons
    if (this.pageSize === this.persons.length) {
      console.log('pagedPersons (All):', this.filteredPersons); // Debug log
      return this.filteredPersons;
    }
    const start = (this.currentPage - 1) * this.pageSize;
    console.log('pagedPersons (Paged):', this.filteredPersons.slice(start, start + this.pageSize)); // Debug log
    return this.filteredPersons.slice(start, start + this.pageSize);
  }
  get pageNumbers(): number[] {
    // Prevent division by zero and invalid array length
    if (!this.pageSize || this.pageSize <= 0) return [1];
    const totalPages = Math.ceil(this.filteredPersons.length / this.pageSize);
    if (!isFinite(totalPages) || totalPages < 1) return [1];
    return Array.from({ length: totalPages }, (_, i) => i + 1);
  }
  goToPage(page: number) {
    if (page < 1 || page > this.pageNumbers.length) return;
    this.currentPage = page;
  }
  nameFilter: string = '';
  departmentFilter: string = '';
  get filteredPersons(): any[] {
    let filtered = this.persons;
    if (this.nameFilter && this.nameFilter.trim()) {
      const term = this.nameFilter.trim().toLowerCase();
      filtered = filtered.filter(p => {
        return (
          (p.firstName && p.firstName.toLowerCase().includes(term)) ||
          (p.lastName && p.lastName.toLowerCase().includes(term))
        );
      });
    }
    if (this.departmentFilter && this.departmentFilter !== '') {
      filtered = filtered.filter(p =>
        (p.departmentName || p.department?.name || p.department?.Name) === this.departmentFilter
      );
    }
    console.log('filteredPersons:', filtered); // Debug log
    return filtered;
  }
  // Helper to get error messages as array for template
  getErrorMessages(errors: any): string[] {
    if (!errors) return [];
    let msgs: string[] = [];
    if (Array.isArray(errors)) {
      msgs = errors;
    } else if (typeof errors === 'string') {
      if (errors.includes('.')) {
        msgs = errors.split('.').map(e => e.trim()).filter(e => e);
      } else {
        msgs = [errors];
      }
    }
    // Remove leading asterisk from each error message
    return msgs.map(e => e.replace(/^\*/, '').trim()).filter(e => e);
  }
  persons: any[] = [];
  selectedPerson: any = null;
  errorMessage: string = '';
  departments: any[] = [];
  fieldErrors: { [key: string]: string } = {}; // Store field-specific errors
  showDeleteModal: boolean = false;
  selectedPersonToDelete: any = null;

  constructor(
    private personService: PersonService,
    private departmentService: DepartmentService,
    private cdr: ChangeDetectorRef
  ) {}


  onPageSizeChange(newSize: number) {
    if (newSize === 0) {
      this.nameFilter = '';
      this.departmentFilter = '';
      this.pageSize = this.persons.length;
      this.currentPage = 1;
      this.getAllPersons();
      setTimeout(() => {
        this.cdr.detectChanges();
      }, 0);
    } else {
      this.pageSize = newSize || 5;
      this.currentPage = 1;
      this.cdr.detectChanges();
    }
  }

  ngOnInit() {
    // pageSize is already set to 5 by default above
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
        // Map DOB to JS Date for correct display in table
        this.persons = persons.map(p => ({
          ...p,
          dob: p.dob ? new Date(p.dob) : null
        }));
        this.cdr.detectChanges(); // ensure UI updates after async fetch
        setTimeout(() => {
          this.cdr.detectChanges();
        }, 0); // Extra change detection for stubborn UI
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
    return err?.message || 'Unknown error occurred';
  }

  onSave() {
    if (this.selectedPerson) {
      // Always clear field errors before save
      this.fieldErrors = {};
      const payload: any = {
        Id: this.selectedPerson?.id ?? this.selectedPerson?.Id ?? 0,
        FirstName: this.selectedPerson?.firstName ?? '',
        LastName: this.selectedPerson?.lastName ?? '',
        Description: this.selectedPerson?.description ?? '',
        DepartmentId: this.selectedPerson?.departmentId ?? 0,
        DOB: this.selectedPerson?.dob ? this.formatDateForBackend(this.selectedPerson?.dob) : null
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
          'DOB': 'dob'
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

  onInputChange(field: string) {
    // Clear field error for this field on input
    if (this.fieldErrors[field]) {
      this.fieldErrors[field] = '';
    }
  }

  onEdit(person: any) {
    // Clone to avoid editing the array object directly
    this.selectedPerson = { ...person };
    // Set date picker value to yyyy-MM-dd string for input type=date
    if (person.dob) {
      const d = new Date(person.dob);
      if (!isNaN(d.getTime())) {
        // Format as yyyy-MM-dd
        const day = String(d.getDate()).padStart(2, '0');
        const month = String(d.getMonth() + 1).padStart(2, '0');
        const year = d.getFullYear();
        this.selectedPerson.dob = `${year}-${month}-${day}`;
      } else {
        this.selectedPerson.dob = null;
      }
    } else {
      this.selectedPerson.dob = null;
    }
  }

  onDelete(person: any) {
    if (!person.id && person.Id) {
      person.id = person.Id; // Map Id to id if needed
    }
    if (!person.id) {
      console.error('Delete failed: person.id is undefined', person);
      this.errorMessage = 'Cannot delete: person id is missing.';
      return;
    }
    // Show custom confirmation popup
    this.showDeleteConfirm(person);
  }

  showDeleteConfirm(person: any) {
    this.selectedPersonToDelete = person;
    this.showDeleteModal = true;
  }

  confirmDelete() {
    if (this.selectedPersonToDelete && this.selectedPersonToDelete.id) {
      this.personService.delete(this.selectedPersonToDelete.id).subscribe({
        next: () => {
          this.getAllPersons();
          this.showDeleteModal = false;
          this.selectedPersonToDelete = null;
        },
        error: err => {
          console.error('Error deleting person:', err);
          this.errorMessage = this.formatError(err);
          this.showDeleteModal = false;
          this.selectedPersonToDelete = null;
        }
      });
    } else {
      this.showDeleteModal = false;
      this.selectedPersonToDelete = null;
    }
  }

  cancelDelete() {
    this.showDeleteModal = false;
    this.selectedPersonToDelete = null;
  }

  onAdd() {
    this.selectedPerson = {
      firstName: '',
      lastName: '',
      dob: null, // Ensure date picker is empty
      departmentId: 0,
      description: ''
    };
  }

  onCancelEdit() {
    this.selectedPerson = null;
    this.fieldErrors = {}; // clear field errors on cancel
  }
}
