import { Component, OnInit } from '@angular/core';
import { DepartmentService } from '../../services/department.service';
import { PersonService } from '../../services/person.service';
import { Department } from '../../models/department.model';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrls: ['./department.component.scss']
})
export class DepartmentComponent implements OnInit {
  fieldErrors: { [key: string]: string } = {};
  toastMessage: string = '';
  showToast: boolean = false;

  deleteDepartment(): void {
    if (!this.selectedDepartment) {
      console.error('deleteDepartment called but no selectedDepartment is set!');
      this.deleteError = 'No department selected for deletion.';
      return;
    }
    this.departmentService.deleteDepartment(this.selectedDepartment.Id).subscribe({
      next: () => {
        this.departments = this.departments.filter(d => d.Id !== this.selectedDepartment!.Id);
        this.showDeleteModal = false;
        this.selectedDepartment = null;
        this.deleteError = '';
        this.showToastMessage('Department deleted successfully!');
      },
      error: (err: any) => {
        this.deleteError = 'Failed to delete department.';
      }
    });
  }

  // ...existing code...

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
  showDeleteModal = false;
  showPersonsModal = false;
  personsInDepartment: PersonViewModel[] = [];
  deleteError: string = '';

  errorMessage: string = '';
  isEditMode: boolean = false;

  constructor(
    private departmentService: DepartmentService,
    private personService: PersonService
  ) {}

  ngOnInit(): void {
    this.loadDepartments();
    this.loadPersons();
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

  loadDepartments(): void {
    this.departmentService.getDepartments().subscribe((data: Department[]) => {
      this.departments = data;
    });
  }

  loadPersons(): void {
    this.personService.getAll().subscribe((data: PersonViewModel[]) => {
      this.persons = data;
    });
  }

  confirmDelete(department?: Department): void {
    if (department) {
      this.selectedDepartment = department;
    }
    this.deleteError = '';
    this.showDeleteModal = true;
  }

  cancelDelete(): void {
    this.selectedDepartment = null;
    this.showDeleteModal = false;
    this.deleteError = '';
  }

  // No deleteDepartment logic since not implemented in DepartmentService

  viewPersons(department: Department): void {
    this.personsInDepartment = this.persons.filter(p => p.DepartmentId === department.Id);
    this.showPersonsModal = true;
  }

  closePersonsModal(): void {
    this.showPersonsModal = false;
    this.personsInDepartment = [];
  }
}

