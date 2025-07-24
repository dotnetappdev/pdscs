@Component({
  selector: 'app-confirm-delete-modal',
  template: ''
})
class MockConfirmDeleteModalComponent {
  @Input() itemName: string = '';
  @Input() show: boolean = false;
}
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-toast',
  template: ''
})
class MockToastComponent {
  @Input() message: string = '';
  @Input() show: boolean = false;
}
import { TestBed } from '@angular/core/testing';
import { DepartmentComponent } from './department.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('DepartmentComponent', () => {
  it('updates a department and reflects the change in the table', async () => {
    // Arrange
    const fixture = TestBed.createComponent(DepartmentComponent);
    const component = fixture.componentInstance as DepartmentComponent;
    const department = { Id: 1, Name: 'HR' };
    component.departments = [department];
    fixture.detectChanges();

    // Act: update the department
    const updatedDepartment = { ...department, Name: 'Human Resources' };
    component.departments = [updatedDepartment];
    fixture.detectChanges();

    // Assert
    const compiled = fixture.nativeElement as HTMLElement;
    const row = compiled.querySelector('table tbody tr');
    expect(row).toBeTruthy();
    expect(row?.textContent).toContain('Human Resources');
    expect(row?.textContent).not.toContain('HR');
  });

  it('deletes a department and removes its row from the table', async () => {
    // Arrange
    const fixture = TestBed.createComponent(DepartmentComponent);
    const component = fixture.componentInstance as DepartmentComponent;
    const dept1 = { Id: 1, Name: 'HR' };
    const dept2 = { Id: 2, Name: 'IT' };
    component.departments = [dept1, dept2];
    fixture.detectChanges();

    // Act: simulate delete by manually removing from array
    component.departments = component.departments.filter(d => d.Id !== dept1.Id);
    fixture.detectChanges();

    // Assert
    const compiled = fixture.nativeElement as HTMLElement;
    const rows = compiled.querySelectorAll('table tbody tr');
    expect(rows.length).toBe(1);
    expect(rows[0].textContent).toContain('IT');
    expect(rows[0].textContent).not.toContain('HR');
  });
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [DepartmentComponent, MockToastComponent, MockConfirmDeleteModalComponent],
      providers: [
        { provide: 'BASE_URL', useValue: 'http://localhost' }
      ]
    }).compileComponents();
  });

  it('renders a table with department names in the first cell of each row', async () => {
    // Arrange
    const fixture = TestBed.createComponent(DepartmentComponent);
    const component = fixture.componentInstance as DepartmentComponent;
    const testDepartments = [
      { Id: 1, Name: 'HR' },
      { Id: 2, Name: 'IT' },
      { Id: 3, Name: 'Finance' },
      { Id: 9, Name: 'Software Development' }
    ];
    component.departments = testDepartments;
    fixture.detectChanges();

    // Act
    const compiled = fixture.nativeElement as HTMLElement;
    const table = compiled.querySelector('table');
    expect(table).toBeTruthy();
    const rows = table?.querySelectorAll('tbody tr') ?? [];
    expect(rows.length).toBe(testDepartments.length);
    rows.forEach((row, i) => {
      const cells = row.querySelectorAll('td');
      expect(cells.length).toBeGreaterThan(0);
      expect(cells[0].textContent).toContain(testDepartments[i].Name);
    });
  });
});
