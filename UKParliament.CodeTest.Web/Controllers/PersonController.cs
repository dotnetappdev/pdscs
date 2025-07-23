using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Repositories;
using UKParliament.CodeTest.Web.Mapper;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;
using Serilog;

[ApiController]
// Use explicit route for clarity
[Route("api/person")]
public class PersonController : ControllerBase
{
    private readonly IPersonReadService<Person> _readService;
    private readonly IPersonWriteService<Person> _writeService;
    private readonly PersonMapperBase _mapper;
    private readonly ILogger _logger;

    public PersonController(IPersonReadService<Person> readService, IPersonWriteService<Person> writeService, PersonMapperBase mapper)
    {
        _readService = readService;
        _writeService = writeService;
        _mapper = mapper;
        _logger = Log.ForContext<PersonController>();
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] Person person)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors != null && x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors?.Select(e => e.ErrorMessage).ToArray() ?? new string[0]
                );
            // If errors only contain keys like "person" or "$", it's a binding error, not FluentValidation
            var onlyBindingErrors = errors.Keys.All(k => k == "person" || k.StartsWith("$"));
            if (onlyBindingErrors)
            {
                return BadRequest(new { message = "Model binding failed", errors });
            }
            // Otherwise, return field-level errors (FluentValidation)
            return BadRequest(new { message = "Validation failed", errors });
        }
        var api = _writeService.UpdatePerson(id, person);
        if (api.StatusCode == HttpStatusCode.OK)
        {
            return Ok();
        }
        else
        {
            // Always return errors as { errors: { field: [messages] } }
            var errors = api.GetType().GetProperty("Errors")?.GetValue(api);
            if (errors is IDictionary<string, string[]> fieldErrors && fieldErrors.Count > 0)
            {
                // Ensure all errors have field names
                return BadRequest(new { message = api.Message, errors = fieldErrors });
            }
            // If error is a string, treat as general error
            if (errors is string msg && !string.IsNullOrWhiteSpace(msg))
            {
                var generalErrors = new Dictionary<string, string[]> { { "General", new[] { msg } } };
                return BadRequest(new { message = api.Message, errors = generalErrors });
            }
            return BadRequest(new { message = api.Message });
        }

    }

    [HttpPost]
    public ActionResult Post([FromBody] Person person)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors != null && x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors?.Select(e => e.ErrorMessage).ToArray() ?? new string[0]
                );
            // If errors only contain keys like "person" or "$", it's a binding error, not FluentValidation
            var onlyBindingErrors = errors.Keys.All(k => k == "person" || k.StartsWith("$"));
            if (onlyBindingErrors)
            {
                return BadRequest(new { message = "Model binding failed", errors });
            }
            // Otherwise, return field-level errors (FluentValidation)
            return BadRequest(new { message = "Validation failed", errors });
        }
        var api = _writeService.AddPerson(person);
        if (api.StatusCode == HttpStatusCode.OK)
        {
            _logger.Information("Person added successfully: {@Person}", person);
            return Ok();
        }
        else if (api.StatusCode == HttpStatusCode.Created)
        {
            _logger.Information("Person created successfully: {@Person}", person);
            return Ok();
        }
        else
        {
            // Always return errors as { errors: { field: [messages] } }
            var errors = api.GetType().GetProperty("Errors")?.GetValue(api);
            if (errors is IDictionary<string, string[]> fieldErrors && fieldErrors.Count > 0)
            {
                _logger.Warning("Validation errors when adding person: {@Errors}", fieldErrors);
                return BadRequest(new { message = api.Message, errors = fieldErrors });
            }
            // If error is a string, treat as general error
            if (errors is string msg && !string.IsNullOrWhiteSpace(msg))
            {
                var generalErrors = new Dictionary<string, string[]> { { "General", new[] { msg } } };
                _logger.Warning("General error when adding person: {Error}", msg);
                return BadRequest(new { message = api.Message, errors = generalErrors });
            }
            _logger.Error("Unknown error when adding person: {Message}", api.Message);
            return BadRequest(new { message = api.Message });
        }

    }





    [HttpGet("{id:int}")]
    public ActionResult<PersonViewModel> GetById(int id)
    {
        var person = _readService.GetById(id);
        if (person == null)
        {
            // Return 404 if not found, avoid null reference
            return NotFound(new { message = $"Person with id {id} not found." });
        }
        var personVm = _mapper.Map(person);
        if (personVm == null)
        {
            // Defensive: if mapping fails, return 500
            return StatusCode(500, new { message = "Failed to map person to view model." });
        }
        return Ok(personVm);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var people = _readService.GetAll();
        var vmList = _mapper.MapList(people);
        return Ok(vmList);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var person = _readService.GetById(id);
        if (person == null)
        {
            // Return 404 if not found, avoid null reference
            return NotFound(new { message = $"Person with id {id} not found." });
        }
        _writeService.DeletePerson(id);
        return Ok(new { message = $"Person with id {id} deleted successfully." });
    }

}