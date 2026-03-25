using System.Threading.Tasks;
using ConstructionProject.Models;
using ConstructionProject.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorController : ControllerBase
    {
        private readonly ContractorService _service;

        public ContractorController(ContractorService service)
        {
            _service = service;
        }

        [HttpPost("addContractor")]
        public async Task<IActionResult> AddContractor([FromBody] Contractor contractor)
        {
            if (contractor == null) return BadRequest();

            var created = await _service.AddContractorAsync(contractor);
            return CreatedAtAction(nameof(GetContractorDetails), new { id = created.ContractorId }, created);
        }

        [HttpPost("assignContractor/{contractorId}")]
        public async Task<IActionResult> AssignContractor(int contractorId, [FromBody] Workforce worker)
        {
            if (worker == null) return BadRequest();

            var ok = await _service.AssignContractorAsync(contractorId, worker);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpGet("getContractorDetails/{id}")]
        public async Task<IActionResult> GetContractorDetails(int id)
        {
            var contractor = await _service.GetContractorDetailsAsync(id);
            if (contractor == null) return NotFound();
            return Ok(contractor);
        }
    }
}
