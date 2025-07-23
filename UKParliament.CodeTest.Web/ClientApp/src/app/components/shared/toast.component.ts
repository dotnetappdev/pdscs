import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-toast',
  template: `
    <div *ngIf="show" class="toast-message">
      {{ message }}
    </div>
  `,
  styleUrls: ['./toast.component.scss']
})
export class ToastComponent {
  @Input() show: boolean = false;
  @Input() message: string = '';
}
