using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Services.Mapper;
using UKParliament.CodeTest.Web.Mapper;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonController : ControllerBase
{
    private readonly PersonReadService _readService;
    private readonly PersonWriteService _writeService;
    private readonly PersonMapperBase _mapper;

    public PersonController(PersonReadService readService, PersonWriteService writeService, PersonMapperBase mapper)
    {
        _readService = readService;
        _writeService = writeService;
        _mapper = mapper;

    }

    [Route("{id:int}")]
    [HttpGet]
    public ActionResult<PersonViewModel> GetById(int id)
    {
        var person = _readService.GetPerson(id);

        var personVm = _mapper.Map(person);
        return Ok(personVm);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var people = _readService.GetAllPersons();
        var vmList = _mapper.MapList(people);
        return Ok(vmList);
    }
}