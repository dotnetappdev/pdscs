using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    private readonly PersonManagerContext dbContext;

    public PersonService(PersonManagerContext context)
    {
        // Constructor logic if needed
        dbContext = context;
    }

    public async Task<Person> GetPeron(int id)
    {

        var person = await dbContext.People.FindAsync(id);
        return person;
    }

    public async Task<List<Person>> GetPersons()
    {
        return await dbContext.People.ToListAsync();
    }

    public async Task<Person> AddPerson(Person person)
    {
        dbContext.People.Add(person);
        await dbContext.SaveChangesAsync();
        return person;
    }

    public async Task<Person> UpdatePerson(Person person)
    {
        dbContext.People.Update(person);
        await dbContext.SaveChangesAsync();
        return person;
    }

    public async Task DeletePerson(int id)
    {
        var person = await dbContext.People.FindAsync(id);
        if (person != null)
        {
            dbContext.People.Remove(person);
            await dbContext.SaveChangesAsync();
        }
    }
}