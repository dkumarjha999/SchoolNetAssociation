using SchoolNetAssociation.Domain.Entities;

namespace SchoolNetAssociation.Domain.Repositories
{
	public interface ISchoolDistrictRepository
	{
		Task<IEnumerable<SchoolDistrict>> GetAllSchoolDistrictsAsync();
		Task<SchoolDistrict> GetSchoolDistrictByIdAsync(string id);
		Task<SchoolDistrict> CreateSchoolDistrictAsync(SchoolDistrict schoolDistrict);
	}
}
