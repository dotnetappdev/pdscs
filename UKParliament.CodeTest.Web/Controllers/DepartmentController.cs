using Microsoft.AspNetCore.Mvc;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentReadRepository _readRepository;
        private readonly IDepartmentWriteRepository _writeRepository;

        public DepartmentController(IDepartmentReadRepository readRepository, IDepartmentWriteRepository writeRepository)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await _readRepository.GetAllAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _readRepository.GetByIdAsync(id);
            if (department == null)
                return NotFound();
            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult<Department>> AddDepartment([FromBody] Department department)
        {
            var created = await _writeRepository.AddAsync(department);
            return CreatedAtAction(nameof(GetDepartment), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] Department department)
        {
            if (id != department.Id)
                return BadRequest();
            var updated = await _writeRepository.UpdateAsync(department);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var deleted = await _writeRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
