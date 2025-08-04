using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Middleware;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentReadService _readService;
    private readonly IDepartmentWriteService _writeService;

    public DepartmentController(IDepartmentReadService readService, IDepartmentWriteService writeService)
    {
        _readService = readService;
        _writeService = writeService;
    }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await _readService.GetAllAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _readService.GetByIdAsync(id);
            if (department == null)
                return NotFound();
            return Ok(department);
        }

        [HttpPost]
        [ValidationActionFilter]
        public async Task<ActionResult<Department>> AddDepartment([FromBody] Department department)
        {
            var created = await _writeService.AddAsync(department);
            if (created == null)
            {
                return BadRequest(new { message = "Failed to create department." });
            }
            return CreatedAtAction(nameof(GetDepartment), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            if (id != department.Id)
                return BadRequest();
            var updated = await _writeService.UpdateAsync(department);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var deleted = await _writeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
