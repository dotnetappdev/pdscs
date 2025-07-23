using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web;

namespace UKParliament.CodeTest.Services.Repositories
{
    public class PersonWriteRepository : IPersonWriteService<Person>
    {
        private readonly PersonManagerContext _context;
        private readonly ILogger<PersonWriteRepository> _logger;
        public PersonWriteRepository(PersonManagerContext context, ILogger<PersonWriteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public void SavePerson(Person person)
        {
            try
            {
                _context.People.Add(person);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving person");
                throw;
            }
        }
        public void Delete(int id)
        {
            try
            {
                var person = _context.People.Find(id);
                if (person != null)
                {
                    _context.People.Remove(person);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting person with id {PersonId}", id);
                throw;
            }
        }

        public ApiResponse<Person> AddPerson(Person person)
        {
            try
            {
                // Sanitize fields before validation and saving
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
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = errorDict
                    };
                }
                _context.People.Add(person);
                _context.SaveChanges();

                return ApiResponse<Person>.SuccessResponse(
                    person,
                    "Person added successfully.",
                    HttpStatusCode.Created // 201
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding person");
                return ApiResponse<Person>.Failure(
                    ex.Message.ToString(),
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public ApiResponse<Person> UpdatePerson(int id, Person updatedPerson)
        {
            try
            {
                // Sanitize fields before validation and saving
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
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = errorDict
                    };
                }
                var recordToUpdate = _context.People.Find(id);
                if (recordToUpdate == null)
                {
                    return ApiResponse<Person>.Failure(
                        $"Person with ID {id} not found.",
                        HttpStatusCode.NotFound
                    );
                }

                // Safely copy values from updatedPerson to the tracked entity
                _context.Entry(recordToUpdate).CurrentValues.SetValues(updatedPerson);

                _context.SaveChanges();

                return ApiResponse<Person>.SuccessResponse(
                    recordToUpdate,
                    "Person updated successfully.",
                    HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating person with id {PersonId}", id);
                return ApiResponse<Person>.Failure(
                    "An error occurred while updating the person.",
                    HttpStatusCode.InternalServerError
                );
            }
        }


        public async Task<ApiResponse<Person>> DeletePerson(int id)
        {
            try
            {
                var personToDelete = await _context.People.FindAsync(id);
                if (personToDelete == null)
                {
                    return ApiResponse<Person>.Failure(
                        "Person not found.",
                        HttpStatusCode.NotFound
                    );
                }

                _context.Remove(personToDelete);
                await _context.SaveChangesAsync();

                return ApiResponse<Person>.SuccessResponse(
                    personToDelete,
                    "Person deleted successfully.",
                    HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting person with id {PersonId}", id);
                return ApiResponse<Person>.Failure(
                    "An error occurred while deleting the person.",
                    HttpStatusCode.InternalServerError
                );
            }
        }


    }
}
