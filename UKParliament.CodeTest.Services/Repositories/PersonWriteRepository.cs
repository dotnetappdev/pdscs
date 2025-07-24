using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web;

namespace UKParliament.CodeTest.Services.Repositories
{
    public class PersonWriteRepository : IPersonWriteService
    {
        private readonly PersonManagerContext _context;
        private readonly ILogger<PersonWriteRepository> _logger;

        public PersonWriteRepository(PersonManagerContext context, ILogger<PersonWriteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<Person>> AddPersonAsync(Person person)
        {
            try
            {
                if (person == null)
                {
                    return ApiResponse<Person>.Failure(
                        "Person cannot be null.",
                        System.Net.HttpStatusCode.BadRequest
                    );
                }
                person.FirstName = Services.Validation.SanitizationHelper.Sanitize(person.FirstName);
                person.LastName = Services.Validation.SanitizationHelper.Sanitize(person.LastName);
                person.Description = Services.Validation.SanitizationHelper.Sanitize(person.Description);

                var validator = new Services.Validation.PersonValidator();
                var validationResult = validator.Validate(person);
                if (!validationResult.IsValid)
                {
                    var errorDict = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return new ApiResponse<Person>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = null,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Errors = errorDict
                    };
                }
                await _context.People.AddAsync(person);
                await _context.SaveChangesAsync();

                return ApiResponse<Person>.SuccessResponse(
                    person,
                    "Person added successfully.",
                    System.Net.HttpStatusCode.Created
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding person");
                return ApiResponse<Person>.Failure(
                    ex.Message.ToString(),
                    System.Net.HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ApiResponse<Person>> UpdatePersonAsync(int id, Person updatedPerson)
        {
            try
            {
                updatedPerson.FirstName = Services.Validation.SanitizationHelper.Sanitize(updatedPerson.FirstName);
                updatedPerson.LastName = Services.Validation.SanitizationHelper.Sanitize(updatedPerson.LastName);
                updatedPerson.Description = Services.Validation.SanitizationHelper.Sanitize(updatedPerson.Description);

                var validator = new Services.Validation.PersonValidator();
                var validationResult = validator.Validate(updatedPerson);
                if (!validationResult.IsValid)
                {
                    var errorDict = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return new ApiResponse<Person>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = null,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Errors = errorDict
                    };
                }
                var recordToUpdate = await _context.People.FindAsync(id);
                if (recordToUpdate == null)
                {
                    return ApiResponse<Person>.Failure(
                        $"Person with ID {id} not found.",
                        System.Net.HttpStatusCode.NotFound
                    );
                }
                _context.Entry(recordToUpdate).CurrentValues.SetValues(updatedPerson);
                await _context.SaveChangesAsync();

                return ApiResponse<Person>.SuccessResponse(
                    recordToUpdate,
                    "Person updated successfully.",
                    System.Net.HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating person with id {PersonId}", id);
                return ApiResponse<Person>.Failure(
                    "An error occurred while updating the person.",
                    System.Net.HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ApiResponse<Person>> DeletePersonAsync(int id)
        {
            try
            {
                var personToDelete = await _context.People.FindAsync(id);
                if (personToDelete == null)
                {
                    return ApiResponse<Person>.Failure(
                        "Person not found.",
                        System.Net.HttpStatusCode.NotFound
                    );
                }

                _context.Remove(personToDelete);
                await _context.SaveChangesAsync();

                return ApiResponse<Person>.SuccessResponse(
                    personToDelete,
                    "Person deleted successfully.",
                    System.Net.HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting person with id {PersonId}", id);
                return ApiResponse<Person>.Failure(
                    "An error occurred while deleting the person.",
                    System.Net.HttpStatusCode.InternalServerError
                );
            }
        }
    }
}