using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Net;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Repositories;
using UKParliament.CodeTest.Web.Mapper;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
// Use explicit route for clarity
[Route("api/person")]
public class PersonController : ControllerBase
{
    private readonly IPersonReadService<Person> _readService;
    private readonly IPersonWriteService<Person> _writeService;
    private readonly PersonMapperBase _mapper;

    public PersonController(IPersonReadService<Person> readService, IPersonWriteService<Person> writeService, PersonMapperBase mapper)
    {
        _readService = readService;
        _writeService = writeService;
        _mapper = mapper;
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, [FromBody] Person person)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { message = "Model binding failed", errors });
        }
        var api = _writeService.UpdatePerson(id, person);
        if (api.StatusCode == HttpStatusCode.OK)
        {
            return Ok();
        }
        else
        {
            return BadRequest(new { message = api.Message });
        }

    }

    [HttpPost]
    public ActionResult Post([FromBody] Person person)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            return BadRequest(new { message = "Model binding failed", errors });
        }
        var api = _writeService.AddPerson(person);
        if (api.StatusCode == HttpStatusCode.OK)
        {
            return Ok();
        }
        else if (api.StatusCode == HttpStatusCode.Created)
        {
            return Ok();
        }
        else
        {
            return BadRequest(new { message = api.Message });
        }

    }





    [HttpGet("{id:int}")]
    public ActionResult<PersonViewModel> GetById(int id)
    {
        var person = _readService.GetById(id);
        if (person == null)
        {
            return NotFound(new { message = $"Person with id {id} not found." });
        }
        var personVm = _mapper.Map(person);
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
            return NotFound(new { message = $"Person with id {id} not found." });
        }
        _writeService.DeletePerson(id);
        return Ok(new { message = $"Person with id {id} deleted successfully." });
    }

}