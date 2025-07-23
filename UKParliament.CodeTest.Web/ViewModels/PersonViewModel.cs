using System.ComponentModel.DataAnnotations;

namespace UKParliament.CodeTest.Web.ViewModels;

public class PersonViewModel
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? FullName { get; set; }

        // Show only the date part as string (yyyy-MM-dd)
        public string? DOB { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }



}