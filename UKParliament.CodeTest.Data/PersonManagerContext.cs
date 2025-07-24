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

        modelBuilder.Entity<Department>().HasData(SampleData.GetDepartments());
        modelBuilder.Entity<Person>().HasData(SampleData.GetPeople());
    }

public DbSet<Person> People { get; set; } = default!;

public DbSet<Department> Departments { get; set; } = default!;



}