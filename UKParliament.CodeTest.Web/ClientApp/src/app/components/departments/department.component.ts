import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../services/department.service';
import { DepartmentSyncService } from '../../services/department-sync.service';
import { PersonService } from '../../services/person.service';
import { Department } from '../../models/department.model';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrls: ['./department.component.scss']
})
export class DepartmentComponent implements OnInit {
  onDepartmentNameFilterChange(): void {
    this.filteredDepartments = this.filterDepartments();
    this.currentPage = 1;
  }
  isMobileView: boolean = false;
  pageSize: number = 5; // Default page size
  currentPage: number = 1;
  get pageSizeOptions(): number[] {
    return [0, 5, 10, 20, 25];
  }
  get pagedDepartments(): Department[] {
    if (this.pageSize === this.filteredDepartments.length) {
      return this.filteredDepartments;
    }
    const start = (this.currentPage - 1) * this.pageSize;
    return this.filteredDepartments.slice(start, start + this.pageSize);
  }
  get pageNumbers(): number[] {
    if (!this.pageSize || this.pageSize <= 0) return [1];
    const totalPages = Math.ceil(this.filteredDepartments.length / this.pageSize);
    if (!isFinite(totalPages) || totalPages < 1) return [1];
    return Array.from({ length: totalPages }, (_, i) => i + 1);
  }
  goToPage(page: number) {
    if (page < 1 || page > this.pageNumbers.length) return;
    this.currentPage = page;
  }
  onPageSizeChange(newSize: number) {
    if (newSize === 0) {
      this.pageSize = this.filteredDepartments.length;
      this.currentPage = 1;
    } else {
      this.pageSize = newSize || 5;
      this.currentPage = 1;
    }
  }
  departmentNameFilter: string = '';
  filteredDepartments: Department[] = [];
  fieldErrors: { [key: string]: string } = {};
  toastMessage: string = '';
  showToast: boolean = false;




  showPersonsPanel: boolean = false;

  showToastMessage(message: string): void {
    this.toastMessage = message;
    this.showToast = true;
    setTimeout(() => {
      this.showToast = false;
    }, 2500);
  }
  hasPersons(dept: Department): boolean {
    return this.persons.some(p => p.DepartmentId === dept.Id);
  }
  departments: Department[] = [];
  persons: PersonViewModel[] = [];
  selectedDepartment: Department | null = null;
  deleteDepartmentTarget: Department | null = null;
  showDeleteModal = false;
  showPersonsModal = false;
  personsInDepartment: PersonViewModel[] = [];
  deleteError: string = '';

  errorMessage: string = '';
  isEditMode: boolean = false;
  sortDirection: 'asc' | 'desc' = 'asc';

  constructor(
    private departmentService: DepartmentService,
    private personService: PersonService,
    private departmentSyncService: DepartmentSyncService
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
    this.loadPersons();
    this.filteredDepartments = this.departments;
    this.onResize();
    window.addEventListener('resize', this.onResize.bind(this));
  }

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe((data: Department[]) => {
      this.departments = data;
      this.filteredDepartments = this.filterDepartments();
    });
  }

  filterDepartments(): Department[] {
    if (!this.departmentNameFilter.trim()) {
      return [...this.departments];
    }
    const filter = this.departmentNameFilter.trim().toLowerCase();
    return this.departments.filter(d => d.Name.toLowerCase().includes(filter));
  }

  onAdd(): void {
    this.selectedDepartment = { Id: 0, Name: '' };
    this.isEditMode = false;
    this.errorMessage = '';
  }

  onEdit(dept: Department): void {
    this.selectedDepartment = { ...dept };
    this.isEditMode = true;
    this.errorMessage = '';
  }

  onSave(): void {
    this.fieldErrors = {};
    if (!this.selectedDepartment || !this.selectedDepartment.Name || this.selectedDepartment.Name.trim() === '') {
      this.fieldErrors['Name'] = 'Department name cannot be blank.';
      this.errorMessage = 'Department name cannot be blank.';
      return;
    }
    if (this.isEditMode) {
      this.departmentService.updateDepartment(this.selectedDepartment).subscribe({
        next: (updated) => {
          this.loadDepartments();
          this.departmentSyncService.notifyDepartmentChanged();
          this.selectedDepartment = null;
          this.isEditMode = false;
          this.errorMessage = '';
          this.showToastMessage('Department updated successfully!');
        },
        error: (err) => {
          if (err?.error?.fieldErrors) {
            this.fieldErrors = err.error.fieldErrors;
            this.errorMessage = Object.values(err.error.fieldErrors).join(' ');
          } else {
            this.errorMessage = 'Failed to update department.';
          }
        }
      });
    } else {
      this.departmentService.addDepartment(this.selectedDepartment).subscribe({
        next: (created) => {
          this.loadDepartments();
          this.departmentSyncService.notifyDepartmentChanged();
          this.selectedDepartment = null;
          this.isEditMode = false;
          this.errorMessage = '';
          this.showToastMessage('Department added successfully!');
        },
        error: (err) => {
          if (err?.error?.fieldErrors) {
            this.fieldErrors = err.error.fieldErrors;
            this.errorMessage = Object.values(err.error.fieldErrors).join(' ');
          } else {
            this.errorMessage = 'Failed to add department.';
          }
        }
      });
    }
  }

  onCancelEdit(): void {
    this.selectedDepartment = null;
    this.isEditMode = false;
    this.errorMessage = '';
  }

  deleteDepartment(): void {
    if (!this.deleteDepartmentTarget) {
      console.error('deleteDepartment called but no deleteDepartmentTarget is set!');
      this.deleteError = 'No department selected for deletion.';
      return;
    }
    this.departmentService.deleteDepartment(this.deleteDepartmentTarget.Id).subscribe({
      next: () => {
        this.loadDepartments();
        this.showDeleteModal = false;
        this.deleteDepartmentTarget = null;
        this.deleteError = '';
        this.showToastMessage('Department deleted successfully!');
        this.currentPage = 1;
      },
      error: (err: any) => {
        this.deleteError = 'Failed to delete department.';
      }
    });
  }

  toggleSortDirection(): void {
    this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    this.departments = [...this.departments].sort((a, b) => {
      if (this.sortDirection === 'asc') {
        return a.Name.localeCompare(b.Name);
      } else {
        return b.Name.localeCompare(a.Name);
      }
    });
  }

  loadPersons(): void {
    this.personService.getAllAsync().subscribe((data: PersonViewModel[]) => {
      this.persons = data;
    });
  }

  confirmDelete(department?: Department): void {
    if (department) {
      this.deleteDepartmentTarget = department;
    }
    this.deleteError = '';
    this.showDeleteModal = true;
  }

  cancelDelete(): void {
    this.deleteDepartmentTarget = null;
    this.showDeleteModal = false;
    this.deleteError = '';
  }

  // No deleteDepartment logic since not implemented in DepartmentService

  viewPersons(department: Department): void {
    this.personsInDepartment = this.persons.filter(p => p.DepartmentId === department.Id);
    this.showPersonsPanel = true;
    this.selectedDepartment = department;
  }

  closePersonsPanel(): void {
    this.showPersonsPanel = false;
    this.personsInDepartment = [];
    this.selectedDepartment = null;
  }
  ngOnDestroy() {
    window.removeEventListener('resize', this.onResize.bind(this));
  }
  onResize() {
    this.isMobileView = window.innerWidth <= 600;
  }
}

