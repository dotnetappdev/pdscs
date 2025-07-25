using FluentValidation;
using Microsoft.Extensions.Logging;
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
       

    public class DepartmentWriteRepository : IDepartmentWriteService<Department>
    {
        private readonly PersonManagerContext _context;
        private readonly ILogger<DepartmentWriteRepository> _logger;
        public DepartmentWriteRepository(PersonManagerContext context, ILogger<DepartmentWriteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return false;
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        public void SaveDepartment(Department dept)
        {
            try
            {
                _context.Departments.Add(dept);
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
                var department = _context.Departments.Find(id);
                if (department != null)
                {
                    _context.Departments.Remove(department);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department with id {DepartmentId}", id);
                throw;
            }
        }

        public async Task<ApiResponse<Department>> AddDepartment(Department department)
        {
            try
            {
                if (department == null)
                {
                    return ApiResponse<Department>.Failure(
                        "Person cannot be null.",
                        System.Net.HttpStatusCode.BadRequest
                    );
                }
                // Sanitize fields before validation and saving
                department.Name = Services.Validation.SanitizationHelper.Sanitize(department.Name);

                var validator = new Services.Validation.DepartmentValidator();
                var validationResult = validator.Validate(department);
                if (!validationResult.IsValid)
                {
                    var errorDict = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return new ApiResponse<Department>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = null,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Errors = errorDict
                    };
                }
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                return ApiResponse<Department>.SuccessResponse(
                    department,
                    "Person added successfully.",
                    System.Net.HttpStatusCode.Created // 201
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding person");
                return ApiResponse<Department>.Failure(
                    ex.Message.ToString(),
                    System.Net.HttpStatusCode.InternalServerError
                );
            }
        }

        public async Task<ApiResponse<Department>> UpdateDepartment(int id, Department updatedDepartment)
        {
            try
            {
                // Sanitize fields before validation and saving
                updatedDepartment.Name = Services.Validation.SanitizationHelper.Sanitize(updatedDepartment.Name);


                var validator = new Services.Validation.DepartmentValidator();
                var validationResult = validator.Validate(updatedDepartment);
                if (!validationResult.IsValid)
                {
                    var errorDict = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return new ApiResponse<Department>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Data = null,
                        StatusCode = HttpStatusCode.BadRequest,
                        Errors = errorDict
                    };
                }
                var recordToUpdate = _context.Departments.Find(id);
                if (recordToUpdate == null)
                {
                    return ApiResponse<Department>.Failure(
                        $"Person with ID {id} not found.",
                        HttpStatusCode.NotFound
                    );
                }

                // Safely copy values from updatedPerson to the tracked entity
                _context.Entry(recordToUpdate).CurrentValues.SetValues(updatedDepartment);

                await _context.SaveChangesAsync();

                return ApiResponse<Department>.SuccessResponse(
                    recordToUpdate,
                    "Person updated successfully.",
                    HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Department with id {DepartmentId}", id);
                return ApiResponse<Department>.Failure(
                    "An error occurred while updating the Department.",
                    HttpStatusCode.InternalServerError
                );
            }
        }


        public async Task<ApiResponse<Department>> DeleteDepartment(int id)
        {
            try
            {
                var personToDelete = await _context.Departments.FindAsync(id);
                if (personToDelete == null)
                {
                    return ApiResponse<Department>.Failure(
                        "Person not found.",
                        HttpStatusCode.NotFound
                    );
                }

                _context.Remove(personToDelete);
                await _context.SaveChangesAsync();

                return ApiResponse<Department>.SuccessResponse(
                    personToDelete,
                    "Person deleted successfully.",
                    HttpStatusCode.OK
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting department with id {DepartmentId}", id);
                return ApiResponse<Department>.Failure(
                    "An error occurred while deleting the person.",
                    HttpStatusCode.InternalServerError
                );
            }
        }


    }
}
