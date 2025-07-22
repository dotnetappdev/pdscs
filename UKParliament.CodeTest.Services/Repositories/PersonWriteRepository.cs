using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web;

namespace UKParliament.CodeTest.Services.Repositories
{
    public class PersonWriteRepository : IPersonWriteService<Person>
    {
        private readonly PersonManagerContext _context;
        public PersonWriteRepository(PersonManagerContext context) => _context = context;


        public void SavePerson(Person person)
        {
            _context.People.Add(person);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var person = _context.People.Find(id);
            if (person != null)
            {
                _context.People.Remove(person);
                _context.SaveChanges();
            }
        }

        public ApiResponse<Person> AddPerson(Person person)
        {
            try
            {
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
            catch (Exception)
            {
                return ApiResponse<Person>.Failure(
                    "An error occurred while updating the person.",
                    HttpStatusCode.InternalServerError
                );
            }
        }

        public Task DeletePerson(int id)
        {
            throw new NotImplementedException();
        }


    }
}
