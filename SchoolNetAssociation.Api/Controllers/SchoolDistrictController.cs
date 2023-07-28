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
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolDistrictByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSchoolDistrictAsync([FromBody] SchoolDistrict schoolDistrict)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSchoolDistrictAsync(string id, [FromBody] SchoolDistrict schoolDistrict)
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
