import { Component, Input } from '@angular/core';
import { Person } from '../persons/person.model';

@Component({
  selector: 'app-department-persons-dialog',
  template: `
    <div *ngIf="show" class="modal-overlay">
      <div class="modal-content">
        <h3>Persons in Department</h3>
        <ul *ngIf="persons && persons.length > 0; else noPersons">
          <li *ngFor="let person of persons">{{ person.firstName }} {{ person.lastName }}</li>
        </ul>
        <ng-template #noPersons>
          <p>No persons in this department.</p>
        </ng-template>
        <div class="modal-actions">
          <button (click)="close.emit()" class="ukp-btn ukp-btn-cancel">Close</button>
        </div>
      </div>
    </div>
  `,
  styleUrls: ['../shared/confirm-delete-modal.component.scss']
})
export class DepartmentPersonsDialogComponent {
  @Input() show: boolean = false;
  @Input() persons: Person[] = [];
  @Input() close = { emit: () => {} };
}
