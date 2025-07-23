import { TestBed } from '@angular/core/testing';
import { DepartmentComponent } from './department.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('DepartmentComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [DepartmentComponent],
    }).compileComponents();
  });

  it('renders rows for departments from sample data', async () => {
    // Arrange: Use a few real departments from SampleData.cs
    const fixture = TestBed.createComponent(DepartmentComponent);
    const component = fixture.componentInstance as DepartmentComponent;
    // Example departments from SampleData.cs
    const testDepartments = [
      { Id: 1, Name: 'HR' },
      { Id: 2, Name: 'IT' },
      { Id: 3, Name: 'Finance' },
      { Id: 9, Name: 'Software Development' }
    ];
    component.departments = testDepartments;
    fixture.detectChanges();

    // Act: query the DOM for the table rows
    const compiled = fixture.nativeElement as HTMLElement;
    const rows = compiled.querySelectorAll('table tbody tr');

    // Log the rows' HTML for debugging
    rows.forEach((row, i) => console.log(`Row ${i} HTML:`, row.outerHTML));

    // Assert: Check that each department name appears in the table
    expect(rows.length).toBe(testDepartments.length);
    expect(rows[0].textContent).toContain('HR');
    expect(rows[1].textContent).toContain('IT');
    expect(rows[2].textContent).toContain('Finance');
    expect(rows[3].textContent).toContain('Software Development');
  });
});
