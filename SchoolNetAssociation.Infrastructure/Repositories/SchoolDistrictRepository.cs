using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SchoolNetAssociation.Domain.Entities;
using SchoolNetAssociation.Domain.Repositories;
using SchoolNetAssociation.Infrastructure.MongoData;

namespace SchoolNetAssociation.Infrastructure.Repositories
{
	public class SchoolDistrictRepository : ISchoolDistrictRepository
	{
		private readonly IMongoCollection<SchoolDistrict> _schoolDistricts;
		public SchoolDistrictRepository(IMongoDatabase mongoDatabase, IOptions<MongoDbSettings> mongoDbSettings)
		{
			_schoolDistricts = mongoDatabase.GetCollection<SchoolDistrict>(mongoDbSettings.Value.SchoolDistrictsCollectionName);
		}

		public async Task<IEnumerable<SchoolDistrict>> GetAllSchoolDistrictsAsync()
		{
			return await _schoolDistricts.Find(_ => true).ToListAsync();
		}

		public async Task<SchoolDistrict> GetSchoolDistrictByIdAsync(string id)
		{
			return await _schoolDistricts.Find(x => x.Id == id).FirstOrDefaultAsync();
		}

		public async Task<SchoolDistrict> CreateSchoolDistrictAsync(SchoolDistrict schoolDistrict)
		{
			await _schoolDistricts.InsertOneAsync(schoolDistrict);
			return schoolDistrict;
		}
	}
}
