import { TestBed } from '@angular/core/testing';
import { PersonsComponent } from './persons.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
describe('PersonsComponent', () => {

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [PersonsComponent],
    }).compileComponents();
  });

  it('does not render a row for a person (Julian Bashir) who is not present in the persons array', async () => {
    // Arrange: Only Matt Smith is in the list, Julian Bashir should not be found
    const fixture = TestBed.createComponent(PersonsComponent);
    const component = fixture.componentInstance;
    const testPerson = {
      id: 1,
      firstName: 'Matt',
      lastName: 'Smith',
      dob: '2001-01-26',
      departmentId: 1,
      departmentName: 'HR',
      fullName: 'Matt Smith',
      description: 'HR Specialist'
    };
    component.persons = [testPerson];
    fixture.detectChanges();

    // Act: query the DOM for the data grid row
    const compiled = fixture.nativeElement as HTMLElement;
    const row = compiled.querySelector('table tbody tr');

    // Log the row's HTML and text content for debugging
    console.log('Row HTML:', row?.outerHTML);
    console.log('Row text:', row?.textContent);

    // Assert: Should NOT find a person named "Julian Bashir"
    expect(row?.textContent).not.toContain('Julian');
    expect(row?.textContent).not.toContain('Bashir');
  });

  it('it does render a row for a person Matt Smith but not Julian Bashir who is not present in the persons array', async () => {
    // Arrange: Use the first real person from SampleData.cs
    const fixture = TestBed.createComponent(PersonsComponent);
    const component = fixture.componentInstance;
    // Matt Smith, Department HR (id: 1)
    const testPerson = {
      id: 1,
      firstName: 'Matt',
      lastName: 'Smith',
      dob: '2001-01-26',
      departmentId: 1,
      departmentName: 'HR',
      fullName: 'Matt Smith',
      description: 'HR Specialist'
    };
    component.persons = [testPerson];
    fixture.detectChanges();

    // Act: query the DOM for the data grid row
    const compiled = fixture.nativeElement as HTMLElement;
    const row = compiled.querySelector('table tbody tr');

    // Log the row's HTML and text content for debugging
    console.log('Row HTML:', row?.outerHTML);
    console.log('Row text:', row?.textContent);

    // Assert
    expect(row).toBeTruthy();
    expect(row?.textContent).toContain('Matt');
    expect(row?.textContent).toContain('Smith');

    // Find the department column by header name for readability
    const headerCells = compiled.querySelectorAll('table thead th');
    let deptColIndex = -1;
    headerCells.forEach((th, idx) => {
      if (th.textContent?.trim() === 'Department') {
        deptColIndex = idx;
      }
    });
    const cells = row?.querySelectorAll('td');
    expect(deptColIndex).toBeGreaterThan(-1);
    expect(cells?.[deptColIndex].textContent).toContain('HR');
  });
});
