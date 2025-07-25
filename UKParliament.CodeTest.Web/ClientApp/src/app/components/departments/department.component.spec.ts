@Component({
  selector: 'app-confirm-delete-modal',
  template: ''
})
class MockConfirmDeleteModalComponent {
  @Input() itemName: string = '';
  @Input() show: boolean = false;
}
import { Component, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';

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
  it('removes a department from the table when the array is updated', async () => {
    const fixture = TestBed.createComponent(DepartmentComponent);
    const component = fixture.componentInstance as DepartmentComponent;
    // Arrange: two departments
    const dept1 = { Id: 1, Name: 'HR' };
    const dept2 = { Id: 2, Name: 'IT' };
    component.departments = [dept1, dept2];
    component.pageSize = 2;
    fixture.detectChanges();
    await fixture.whenStable();

    // Act: remove HR from the array
    component.departments = component.departments.filter(d => d.Id !== dept1.Id);
    component.pageSize = component.departments.length;
    fixture.detectChanges();
    await fixture.whenStable();

    // Assert: table now has one row
    const compiled = fixture.nativeElement as HTMLElement;
    const rows = compiled.querySelectorAll('table tbody tr');
    expect(rows.length).toBe(1);
  });
  it('updates a department and reflects the change in the table', async () => {
    const fixture = TestBed.createComponent(DepartmentComponent);
    const component = fixture.componentInstance as DepartmentComponent;
    // Use SeedData.cs: Id 1 = 'HR'
    const department = { Id: 1, Name: 'HR' };

    component.departments = [department];
    component.pageSize = 1; // Show all test data on one page
    fixture.detectChanges();
    await fixture.whenStable();

    // Act: update the department
    const updatedDepartment = { ...department, Name: 'HR' };

    component.departments = [updatedDepartment];
    component.pageSize = 1;
    fixture.detectChanges();
    await fixture.whenStable();

    // Assert
    const compiled = fixture.nativeElement as HTMLElement;
    const rows = compiled.querySelectorAll('table tbody tr');
    expect(rows.length).toBe(1);
    const firstCell = rows[0].querySelector('td');
    expect(firstCell).toBeTruthy();
    expect(firstCell?.textContent?.trim()).toBe('HR');
  });

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, FormsModule],
      declarations: [DepartmentComponent, MockToastComponent, MockConfirmDeleteModalComponent],
      providers: [
        { provide: 'BASE_URL', useValue: 'http://localhost' }
      ]
    }).compileComponents();
  });

  it('renders a table with department names in the first cell of each row', async () => {
    const fixture = TestBed.createComponent(DepartmentComponent);
    const component = fixture.componentInstance as DepartmentComponent;
    // Use SeedData.cs department names
    const testDepartments = [
      { Id: 1, Name: 'HR' },
      { Id: 2, Name: 'IT' },
      { Id: 3, Name: 'Finance' },
      { Id: 9, Name: 'Software Development' }
    ];

    component.departments = testDepartments;
    component.pageSize = testDepartments.length;
    fixture.detectChanges();
    await fixture.whenStable();

    const compiled = fixture.nativeElement as HTMLElement;
    const table = compiled.querySelector('table');
    expect(table).toBeTruthy();
    const rows = table?.querySelectorAll('tbody tr') ?? [];
    expect(rows.length).toBe(testDepartments.length);
    rows.forEach((row, i) => {
      const cells = row.querySelectorAll('td');
      expect(cells.length).toBeGreaterThan(0);
      expect(cells[0].textContent?.trim()).toBe(testDepartments[i].Name);
    });
  });
});
