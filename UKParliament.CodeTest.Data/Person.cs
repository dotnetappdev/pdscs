using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Data;

public class Person
{
    public int Id { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public DateOnly? DOB { get; set; }

    [MaxLength(1000)]
    public required string Description { get; set; }

    public int DepartmentId { get; set; }  // Foreign key

   
}