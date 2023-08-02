using SchoolNetAssociation.Application.DTOs;

namespace SchoolNetAssociation.Application.Services
{
	public interface ISchoolDistrictService
	{
		Task<IEnumerable<SchoolDistrictDto>> GetAllSchoolDistrictsAsync();
		Task<SchoolDistrictDto> GetSchoolDistrictByIdAsync(string id);
		Task<SchoolDistrictDto> CreateSchoolDistrictAsync(SchoolDistrictDto schoolDistrictDto);
	}
}
