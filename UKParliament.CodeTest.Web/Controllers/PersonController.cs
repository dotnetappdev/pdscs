using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Repositories;
using UKParliament.CodeTest.Web.Mapper;
using UKParliament.CodeTest.Web.ViewModels;
using UKParliament.CodeTest.Web.Middleware;

namespace UKParliament.CodeTest.Web.Controllers;
using Serilog;

[ApiController]
// Use explicit route for clarity
[Route("api/person")]
public class PersonController : ControllerBase
{
    private readonly IPersonReadService _readService;
    private readonly IPersonWriteService _writeService;
    private readonly PersonMapperBase _mapper;
    private readonly ILogger _logger;

    public PersonController(IPersonReadService readService, IPersonWriteService writeService, PersonMapperBase mapper)
    {
        _readService = readService;
        _writeService = writeService;
        _mapper = mapper;
        _logger = Log.ForContext<PersonController>();
    }

    [HttpPut("{id:int}")]
    [ValidationActionFilter]
    public async Task<ActionResult> Put(int id, [FromBody] Person person)
    {
        var api = await _writeService.UpdatePersonAsync(id, person);
        if (api.StatusCode == HttpStatusCode.OK)
        {
            return Ok();
        }
        if (api.StatusCode == HttpStatusCode.Accepted)
        {
            return Accepted();
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
    [ValidationActionFilter]
    public async Task<ActionResult> Post([FromBody] Person person)
    {
        var api = await _writeService.AddPersonAsync(person);
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
    public async Task<ActionResult<PersonViewModel>> GetById(int id)
    {
        var person = await _readService.GetByIdAsync(id);
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
    public async Task<IActionResult> GetAll()
    {
        var people = await _readService.GetAllAsync();
        var vmList = _mapper.MapList(people);
        return Ok(vmList);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var person = await _readService.GetByIdAsync(id);
        if (person == null)
        {
            // Return 404 if not found, avoid null reference
            return NotFound(new { message = $"Person with id {id} not found." });
        }
        await _writeService.DeletePersonAsync(id);
        return Ok(new { message = $"Person with id {id} deleted successfully." });
    }

}