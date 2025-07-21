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

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });


        modelBuilder.Entity<Person>().HasData(
        new Person { Id = 1, FirstName = "Gaberial", LastName = "Smith", DepartmentId = 1, DOB = new DateOnly(2001, 1, 26) },
        new Person { Id = 2, FirstName = "David", LastName = "Brown", DepartmentId = 3, DOB = new DateOnly(1977, 6, 26) },
        new Person { Id = 3, FirstName = "Mike", LastName = "Jones", DepartmentId = 4, DOB = new DateOnly(1985, 3, 20) },
        new Person { Id = 4, FirstName = "Ricky", LastName = "White", DepartmentId = 1, DOB = new DateOnly(1999, 7, 10) }
    );
    }

    public DbSet<Person> People { get; set; }

    public DbSet<Department> Departments { get; set; }



}