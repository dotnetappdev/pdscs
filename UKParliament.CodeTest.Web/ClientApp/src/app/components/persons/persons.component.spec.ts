import { TestBed } from '@angular/core/testing';
import { PersonsComponent } from './persons.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
describe('PersonsComponent', () => {
  it('updates a person and reflects the change in the table', async () => {
    // Arrange
    const fixture = TestBed.createComponent(PersonsComponent);
    const component = fixture.componentInstance;
    const person = {
      Id: 1,
      FirstName: 'Matt',
      LastName: 'Smith',
      DOB: '2001-01-26',
      DepartmentId: 1,
      DepartmentName: 'HR',
      FullName: 'Matt Smith',
      Description: 'HR Specialist'
    };
    component.persons = [person];
    fixture.detectChanges();

    // Act: update the person
    const updatedPerson = {
      ...person,
      FirstName: 'Matthew',
      Description: 'HR Manager'
    };
    component.persons = [updatedPerson];
    fixture.detectChanges();

    // Assert
    const compiled = fixture.nativeElement as HTMLElement;
    const row = compiled.querySelector('table tbody tr');
    expect(row).toBeTruthy();
    expect(row?.textContent).toContain('Matthew');
    expect(row?.textContent).toContain('Smith');
    expect(row?.textContent).toContain('26/01/2001');
    expect(row?.textContent).toContain('HR');
  });
  it('deletes a person and removes their row from the table', async () => {
    // Arrange
    const fixture = TestBed.createComponent(PersonsComponent);
    const component = fixture.componentInstance;
    const person1 = {
      Id: 1,
      FirstName: 'Matt',
      LastName: 'Smith',
      DOB: '2001-01-26',
      DepartmentId: 1,
      DepartmentName: 'HR',
      FullName: 'Matt Smith',
      Description: 'HR Specialist'
    };
    const person2 = {
      Id: 2,
      FirstName: 'David',
      LastName: 'Brown',
      DOB: '1977-06-26',
      DepartmentId: 9,
      DepartmentName: 'Software Development',
      FullName: 'David Brown',
      Description: 'Software Developer'
    };
    component.persons = [person1, person2];
    fixture.detectChanges();

    // Act: simulate delete by manually removing from array
    component.persons = component.persons.filter(p => p.Id !== person1.Id);
    fixture.detectChanges();

    // Assert
    const compiled = fixture.nativeElement as HTMLElement;
    const rows = compiled.querySelectorAll('table tbody tr');
    expect(rows.length).toBe(1);
    expect(rows[0].textContent).toContain('David');
    expect(rows[0].textContent).not.toContain('Matt');
  });

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
      Id: 1,
      FirstName: 'Matt',
      LastName: 'Smith',
      DOB: '2001-01-26',
      DepartmentId: 1,
      DepartmentName: 'HR',
      FullName: 'Matt Smith',
      Description: 'HR Specialist'
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
      Id: 1,
      FirstName: 'Matt',
      LastName: 'Smith',
      DOB: '2001-01-26',
      DepartmentId: 1,
      DepartmentName: 'HR',
      FullName: 'Matt Smith',
      Description: 'HR Specialist'
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
