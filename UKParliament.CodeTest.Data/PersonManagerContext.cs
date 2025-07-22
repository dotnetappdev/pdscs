using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data;

public class PersonManagerContext : DbContext
{
    public PersonManagerContext(DbContextOptions<PersonManagerContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>().HasKey(p => p.Id);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });

        modelBuilder.Entity<Person>().HasData(
            new Person { Id = 1, FirstName = "Gaberial", LastName = "Smith", Description = "Test1", DepartmentId = 1, DOB = new DateOnly(2001, 1, 26) },
            new Person { Id = 2, FirstName = "David", LastName = "Brown", Description = "Test2", DepartmentId = 3, DOB = new DateOnly(1977, 6, 26) },
            new Person { Id = 3, FirstName = "Mike", LastName = "Jones", Description = "Test3", DepartmentId = 4, DOB = new DateOnly(1985, 3, 20) },
            new Person { Id = 4, FirstName = "Ricky", LastName = "White", Description = "Test4", DepartmentId = 1, DOB = new DateOnly(1999, 7, 10) },
            new Person { Id = 5, FirstName = "Sarah", LastName = "Johnson", Description = "Civil Service Analyst", DepartmentId = 2, DOB = new DateOnly(1995, 2, 15) },
            new Person { Id = 6, FirstName = "Emily", LastName = "Clark", Description = "Policy Advisor", DepartmentId = 3, DOB = new DateOnly(1990, 5, 30) },
            new Person { Id = 7, FirstName = "James", LastName = "Miller", Description = "HR Specialist", DepartmentId = 4, DOB = new DateOnly(1982, 8, 12) },
            new Person { Id = 8, FirstName = "Jessica", LastName = "Taylor", Description = "Finance Officer", DepartmentId = 3, DOB = new DateOnly(1998, 11, 22) },
            new Person { Id = 9, FirstName = "Matthew", LastName = "Wilson", Description = "IT Support", DepartmentId = 1, DOB = new DateOnly(1996, 4, 18) },
            new Person { Id = 10, FirstName = "Ashley", LastName = "Moore", Description = "Communications Lead", DepartmentId = 2, DOB = new DateOnly(1987, 9, 5) },
            new Person { Id = 11, FirstName = "Joshua", LastName = "Martin", Description = "Legal Advisor", DepartmentId = 4, DOB = new DateOnly(1993, 6, 27) },
            new Person { Id = 12, FirstName = "Amanda", LastName = "Lee", Description = "Project Manager", DepartmentId = 1, DOB = new DateOnly(1980, 3, 14) },
            new Person { Id = 13, FirstName = "Andrew", LastName = "Walker", Description = "Civil Service Trainee", DepartmentId = 2, DOB = new DateOnly(2002, 7, 19) },
            new Person { Id = 14, FirstName = "Lauren", LastName = "Hall", Description = "Finance Analyst", DepartmentId = 3, DOB = new DateOnly(1997, 12, 3) },
            new Person { Id = 15, FirstName = "Brandon", LastName = "Allen", Description = "HR Assistant", DepartmentId = 4, DOB = new DateOnly(1994, 10, 8) },
            new Person { Id = 16, FirstName = "Megan", LastName = "Young", Description = "Policy Researcher", DepartmentId = 2, DOB = new DateOnly(1991, 1, 21) },
            new Person { Id = 17, FirstName = "Ryan", LastName = "King", Description = "IT Analyst", DepartmentId = 1, DOB = new DateOnly(1999, 5, 17) },
            new Person { Id = 18, FirstName = "Brittany", LastName = "Wright", Description = "Communications Officer", DepartmentId = 2, DOB = new DateOnly(1996, 8, 29) },
            new Person { Id = 19, FirstName = "Tyler", LastName = "Lopez", Description = "Legal Assistant", DepartmentId = 4, DOB = new DateOnly(1989, 2, 11) },
            new Person { Id = 20, FirstName = "Rachel", LastName = "Hill", Description = "Project Coordinator", DepartmentId = 1, DOB = new DateOnly(1992, 6, 6) },
            new Person { Id = 21, FirstName = "Justin", LastName = "Scott", Description = "Finance Trainee", DepartmentId = 3, DOB = new DateOnly(2003, 9, 15) },
            new Person { Id = 22, FirstName = "Stephanie", LastName = "Green", Description = "HR Manager", DepartmentId = 4, DOB = new DateOnly(1984, 12, 25) },
            new Person { Id = 23, FirstName = "Kevin", LastName = "Adams", Description = "Policy Lead", DepartmentId = 2, DOB = new DateOnly(1995, 7, 2) },
            new Person { Id = 24, FirstName = "Nicole", LastName = "Nelson", Description = "IT Manager", DepartmentId = 1, DOB = new DateOnly(1986, 11, 13) },
            new Person { Id = 25, FirstName = "Eric", LastName = "Baker", Description = "Communications Trainee", DepartmentId = 2, DOB = new DateOnly(2004, 2, 28) },
            new Person { Id = 26, FirstName = "Kimberly", LastName = "Carter", Description = "Legal Trainee", DepartmentId = 4, DOB = new DateOnly(2002, 4, 9) },
            new Person { Id = 27, FirstName = "Steven", LastName = "Mitchell", Description = "Project Analyst", DepartmentId = 1, DOB = new DateOnly(1998, 7, 23) },
            new Person { Id = 28, FirstName = "Amber", LastName = "Perez", Description = "Finance Manager", DepartmentId = 3, DOB = new DateOnly(1983, 10, 30) },
            new Person { Id = 29, FirstName = "Gregory", LastName = "Roberts", Description = "HR Lead", DepartmentId = 4, DOB = new DateOnly(1990, 5, 2) },
            new Person { Id = 30, FirstName = "Victoria", LastName = "Turner", Description = "Policy Analyst", DepartmentId = 2, DOB = new DateOnly(1997, 8, 16) },
            new Person { Id = 31, FirstName = "Dylan", LastName = "Phillips", Description = "IT Trainee", DepartmentId = 1, DOB = new DateOnly(2005, 3, 7) },
            new Person { Id = 32, FirstName = "Morgan", LastName = "Campbell", Description = "Communications Analyst", DepartmentId = 2, DOB = new DateOnly(1999, 12, 19) },
            new Person { Id = 33, FirstName = "Jordan", LastName = "Parker", Description = "Legal Officer", DepartmentId = 4, DOB = new DateOnly(1993, 6, 1) },
            new Person { Id = 34, FirstName = "Courtney", LastName = "Evans", Description = "Project Lead", DepartmentId = 1, DOB = new DateOnly(1988, 9, 25) },
            new Person { Id = 35, FirstName = "Alex", LastName = "Edwards", Description = "Finance Officer", DepartmentId = 3, DOB = new DateOnly(1996, 2, 14) },
            new Person { Id = 36, FirstName = "Haley", LastName = "Collins", Description = "HR Trainee", DepartmentId = 4, DOB = new DateOnly(2002, 5, 20) },
            new Person { Id = 37, FirstName = "Ethan", LastName = "Stewart", Description = "Policy Trainee", DepartmentId = 2, DOB = new DateOnly(2003, 8, 11) },
            new Person { Id = 38, FirstName = "Kaitlyn", LastName = "Sanchez", Description = "IT Analyst", DepartmentId = 1, DOB = new DateOnly(1998, 1, 3) },
            new Person { Id = 39, FirstName = "Jared", LastName = "Morris", Description = "Communications Manager", DepartmentId = 2, DOB = new DateOnly(1985, 4, 27) },
            new Person { Id = 40, FirstName = "Brooke", LastName = "Rogers", Description = "Legal Manager", DepartmentId = 4, DOB = new DateOnly(1981, 7, 15) }
        );
    }

    public DbSet<Person> People { get; set; }

    public DbSet<Department> Departments { get; set; }



}