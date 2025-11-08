using Dapper_Crud.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Dapper_Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepo _countryRepo;

        public CountryController(IDbConnection dbConnection)
        {
            _countryRepo = new CountryRepository(dbConnection);//countryRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _countryRepo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _countryRepo.GetByIdAsync(id);
            if (country == null) return NotFound();
            return Ok(country);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Country country)
        {
            var id = await _countryRepo.CreateAsync(country);
            return Ok(new { CountryId = id });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Country country)
        {
            var success = await _countryRepo.UpdateAsync(country);
            return success ? Ok("Updated successfully") : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _countryRepo.DeleteAsync(id);
            return success ? Ok("Deleted successfully") : NotFound();
        }
    }
}
