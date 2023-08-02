using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SchoolNetAssociation.Application.Common;
using SchoolNetAssociation.Application.DTOs;
using SchoolNetAssociation.Application.Services;
using SchoolNetAssociation.Application.Validators;
using SchoolNetAssociation.Domain.Entities;

namespace SchoolNetAssociation.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SchoolDistrictController : ControllerBase
	{
		private readonly ISchoolDistrictService _schoolDistrictService;

		public SchoolDistrictController(ISchoolDistrictService schoolDistrictService)
		{
			_schoolDistrictService = schoolDistrictService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllSchoolDistrictsAsync()
		{
			var schoolDistricts = await _schoolDistrictService.GetAllSchoolDistrictsAsync();
			return Ok(schoolDistricts);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetSchoolDistrictByIdAsync(string id)
		{
			if (string.IsNullOrEmpty(id) || !ObjectId.TryParse(id, out ObjectId objectId))
			{
				return BadRequest(ResponseMessages.InvalidSchoolDistrictId);
			}
			var schoolDistrict = await _schoolDistrictService.GetSchoolDistrictByIdAsync(id);
			return Ok(schoolDistrict);
		}

		[HttpPost]
		public async Task<IActionResult> CreateSchoolDistrictAsync([FromBody] SchoolDistrictDto schoolDistrict)
		{
			SchoolDistrictDtoValidator schoolDistrictValidator = new();
			var validatorResult = schoolDistrictValidator.Validate(schoolDistrict);

			if (!validatorResult.IsValid)
			{
				var errorMessages = validatorResult.Errors.Select(error => error.ErrorMessage).ToList();
				return BadRequest(errorMessages);
			}
			var createdSchoolDistrict = await _schoolDistrictService.CreateSchoolDistrictAsync(schoolDistrict);
			return Ok(createdSchoolDistrict);

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
