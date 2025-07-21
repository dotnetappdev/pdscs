namespace UKParliament.CodeTest.Data;

public class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public DateOnly DOB { get; set; }


    public int DepartmentId { get; set; }  // Foreign key
    public Department Department { get; set; }

}