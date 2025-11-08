using Dapper_Crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.Common;

namespace Dapper_Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IStateRepo _stateRepo;

        public StateController(IDbConnection dbConnection)
        {
            _stateRepo = new StateRepository(dbConnection);//countryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _stateRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var state = await _stateRepo.GetByIdAsync(id);
            if (state == null) return NotFound();
            return Ok(state);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] State state)
        {
            var id = await _stateRepo.CreateAsync(state);
            return Ok(new { StateId = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] State state)
        {
            var success = await _stateRepo.UpdateAsync(state);
            return success ? Ok("Updated successfully") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _stateRepo.DeleteAsync(id);
            return success ? Ok("Deleted successfully") : NotFound();
        }
    }
}
