// ...existing code...
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { PersonsService } from './persons.service';
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
  showToast = false;
  toastMessage = '';
  showToastMessage(message: string): void {
    this.toastMessage = message;
    this.showToast = true;
    setTimeout(() => {
      this.showToast = false;
    }, 2500);
  }
  isMobileView: boolean = window.innerWidth <= 600;
  // Validate person input for SQL keywords and dangerous characters, return error object
  private validatePersonInput(person: any): { [key: string]: string } {
    const errors: { [key: string]: string } = {};
    const check = (val: string, field: string) => {
      if (!val) return;
      const lowered = val.toLowerCase();
      this.sqlBlacklist.forEach(bad => {
        if (lowered.includes(bad)) {
          errors[field] = `Input contains forbidden keyword or character: '${bad}'`;
        }
      });
    };
    check(person.FirstName, 'FirstName');
    check(person.LastName, 'LastName');
    check(person.Description, 'Description');
    return errors;
  }
  // SQL and dangerous character blacklist for input sanitization
  private sqlBlacklist: string[] = [
    "select", "insert", "update", "delete", "drop", "truncate", "union", "--", "/*", "*/", "xp_", "exec", "script", "<", ">", "'", '"', ";"
  ];

  private isInputSafe(input: string): boolean {
    if (!input) return true;
    const lowered = input.toLowerCase();
    return !this.sqlBlacklist.some(bad => lowered.includes(bad));
  }

  private sanitizePersonInput(person: any): any {
    // Remove dangerous SQL and script characters from person fields
    const sanitize = (val: string) => {
      if (!val) return val;
      let result = val;
      this.sqlBlacklist.forEach(bad => {
        const regex = new RegExp(bad.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'), 'gi');
        result = result.replace(regex, '');
      });
      return result;
    };
    return {
      ...person,
      FirstName: sanitize(person.FirstName),
      LastName: sanitize(person.LastName),
      Description: sanitize(person.Description)
    };
  }
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
    if (!Array.isArray(this.persons) || this.persons.length === 0) return [];
    let filtered = this.persons;
    if (this.nameFilter && this.nameFilter.trim()) {
      const term = this.nameFilter.trim().toLowerCase();
      filtered = filtered.filter(p => {
        return (
          (p.FirstName && p.FirstName.toLowerCase().includes(term)) ||
          (p.LastName && p.LastName.toLowerCase().includes(term))
        );
      });
    }
    if (this.departmentFilter && this.departmentFilter !== '') {
      filtered = filtered.filter(p => {
        const deptName = p.DepartmentName || p.department?.Name || '';
        return deptName === this.departmentFilter;
      });
    }
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
    private personsService: PersonsService,
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
    window.addEventListener('resize', this.onResize.bind(this));
    this.onResize();
  }

  ngOnDestroy() {
    window.removeEventListener('resize', this.onResize.bind(this));
  }

  onResize() {
    this.isMobileView = window.innerWidth <= 600;
    this.cdr.detectChanges();
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
    this.personsService.getPersons().subscribe({
      next: (persons) => {
        console.log('Received persons:', persons); // Debug log added
        // Map DOB to JS Date for correct display in table
        this.persons = persons.map(p => ({
          ...p,
          DOB: p.DOB ? new Date(p.DOB) : null
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
      // Validate for SQL keywords and dangerous characters
      const validationErrors = this.validatePersonInput(this.selectedPerson);
      if (Object.keys(validationErrors).length > 0) {
        this.fieldErrors = validationErrors;
        this.errorMessage = 'Please remove forbidden keywords or characters from the highlighted fields.';
        return;
      }
      // Sanitize input fields before sending to backend
      const sanitizedPerson = this.sanitizePersonInput(this.selectedPerson);
      const payload: any = {
        Id: sanitizedPerson?.id ?? sanitizedPerson?.Id ?? 0,
        FirstName: sanitizedPerson?.firstName ?? '',
        LastName: sanitizedPerson?.lastName ?? '',
        Description: sanitizedPerson?.description ?? '',
        DepartmentId: sanitizedPerson?.departmentId ?? 0,
        DOB: sanitizedPerson?.dob ? this.formatDateForBackend(sanitizedPerson?.dob) : null
      };
      console.log('Payload sent to backend:', payload);

      const isEdit = typeof sanitizedPerson.id === 'number' && sanitizedPerson.id > 0;

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
        if (isEdit) {
          this.showToastMessage('Person updated successfully!');
        } else {
          this.showToastMessage('Person added successfully!');
        }
      };

      if (isEdit) {
        this.personsService.updatePerson(sanitizedPerson.id!, payload).subscribe({
          next: handleSuccess,
          error: handleError
        });
      } else {
        this.personsService.addPerson(payload).subscribe({
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
    // Find department by name (case-insensitive)
    let deptName = person.DepartmentName ?? person.departmentName ?? person.department ?? '';
    let deptId = person.DepartmentId ?? person.departmentId ?? person.departmentID ?? person.DepartmentID ?? 0;
    if (!deptId && deptName && Array.isArray(this.departments)) {
      const found = this.departments.find(
        d => (d.Name ?? d.name ?? '').toLowerCase() === deptName.toLowerCase()
      );
      if (found) deptId = found.Id ?? found.id ?? 0;
    }
    // Get description/notes
    let desc = person.Description ?? person.description ?? person.Notes ?? person.notes ?? '';
    // Set DOB as yyyy-MM-dd string for input type=date
    let dobVal = person.DOB ?? person.dob ?? person.Dob ?? '';
    let dobStr = '';
    if (dobVal) {
      const d = new Date(dobVal);
      if (!isNaN(d.getTime())) {
        const month = String(d.getMonth() + 1).padStart(2, '0');
        const day = String(d.getDate()).padStart(2, '0');
        dobStr = `${d.getFullYear()}-${month}-${day}`;
      }
    }
    this.selectedPerson = {
      Id: person.Id ?? person.id ?? 0,
      FirstName: person.FirstName ?? person.firstName ?? '',
      LastName: person.LastName ?? person.lastName ?? '',
      DOB: dobStr,
      DepartmentId: deptId,
      DepartmentName: deptName,
      Description: desc
    };
    console.log('Selected person for edit:', this.selectedPerson);
  }

  formatDateForInput(date: any): string {
    if (!date) return '';
    const d = new Date(date);
    if (isNaN(d.getTime())) return '';
    const day = String(d.getDate()).padStart(2, '0');
    const month = String(d.getMonth() + 1).padStart(2, '0');
    const year = d.getFullYear();
    return `${year}-${month}-${day}`;
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
      this.personsService.deletePerson(this.selectedPersonToDelete.id).subscribe({
        next: () => {
          this.getAllPersons();
          this.showDeleteModal = false;
          this.selectedPersonToDelete = null;
          this.showToastMessage('Person deleted successfully!');
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
      FirstName: '',
      LastName: '',
      DOB: '',
      DepartmentId: 0,
      Description: ''
    };
  }

  onCancelEdit() {
    this.selectedPerson = null;
    this.fieldErrors = {}; // clear field errors on cancel
  }
}
