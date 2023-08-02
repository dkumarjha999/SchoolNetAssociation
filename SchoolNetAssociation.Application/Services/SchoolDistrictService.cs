using AutoMapper;
using SchoolNetAssociation.Application.Common;
using SchoolNetAssociation.Application.DTOs;
using SchoolNetAssociation.Domain.Entities;
using SchoolNetAssociation.Domain.Repositories;

namespace SchoolNetAssociation.Application.Services
{
	public class SchoolDistrictService : ISchoolDistrictService
	{

		private readonly ISchoolDistrictRepository _schoolDistrictRepository;
		private readonly IMapper _mapper;

		public SchoolDistrictService(ISchoolDistrictRepository schoolDistrictRepository, IMapper mapper)
		{
			_schoolDistrictRepository = schoolDistrictRepository;
			_mapper = mapper;
		}

		public async Task<IEnumerable<SchoolDistrictDto>> GetAllSchoolDistrictsAsync()
		{
			var schoolDistricts = await _schoolDistrictRepository.GetAllSchoolDistrictsAsync();
			return _mapper.Map<IEnumerable<SchoolDistrictDto>>(schoolDistricts);
		}

		public async Task<SchoolDistrictDto> GetSchoolDistrictByIdAsync(string id)
		{
			var schoolDistrict = await _schoolDistrictRepository.GetSchoolDistrictByIdAsync(id);
			if (schoolDistrict == null)
			{
				throw new InvalidOperationException(ResponseMessages.SchoolDistrictNotFound);
			}
			return _mapper.Map<SchoolDistrictDto>(schoolDistrict);
		}

		public async Task<SchoolDistrictDto> CreateSchoolDistrictAsync(SchoolDistrictDto schoolDistrictDto)
		{
			var schoolDistrictModel = _mapper.Map<SchoolDistrict>(schoolDistrictDto);
			var schoolDistrict = await _schoolDistrictRepository.CreateSchoolDistrictAsync(schoolDistrictModel);
			return _mapper.Map<SchoolDistrictDto>(schoolDistrict);
		}
	}
}
