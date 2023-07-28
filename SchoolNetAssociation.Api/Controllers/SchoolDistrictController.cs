using Microsoft.AspNetCore.Mvc;
using SchoolNetAssociation.Domain.Entities;

namespace SchoolNetAssociation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolDistrictController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolDistrict>>> GetAllSchoolDistrictsAsync()
        {
            return Ok();
        }

        [HttpGet("{id}", Name = "GetSchoolDistrictById")]
        public async Task<IActionResult> GetSchoolDistrictByIdAsync(string id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchoolDistrictAsync(SchoolDistrict schoolDistrict)
        {
            if (schoolDistrict == null)
            {
                return BadRequest();
            }
            return CreatedAtRoute("GetSchoolDistrictById", new { id = schoolDistrict.Id }, schoolDistrict);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolDistrictAsync(string id, SchoolDistrict schoolDistrict)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchoolDistrictAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}
