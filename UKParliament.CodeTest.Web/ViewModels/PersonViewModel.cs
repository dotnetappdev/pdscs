namespace UKParliament.CodeTest.Web.ViewModels;

public class PersonViewModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public DateOnly DOB { get; set; }

    public int DepartmentId { get; set; }  // Foreign key
    public string DepartmentName { get; set; } // Assuming you want to include the department name
    public string FullName { get; set; }
}