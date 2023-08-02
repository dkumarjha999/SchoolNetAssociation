using AutoMapper;
using Bogus;
using FluentAssertions;
using MongoDB.Bson;
using Moq;
using SchoolNetAssociation.Application.Common;
using SchoolNetAssociation.Application.DTOs;
using SchoolNetAssociation.Application.Services;
using SchoolNetAssociation.Domain.Entities;
using SchoolNetAssociation.Domain.Repositories;

namespace SchoolNetAssociation.UntiTest.Application.Services
{
	public class SchoolDistrictServiceTests
	{
		private readonly SchoolDistrictService _schoolDistrictService;
		private readonly Mock<ISchoolDistrictRepository> _mockSchoolDistrictRepository;
		private readonly IMapper _mapper;
		private readonly Faker _faker;
		public SchoolDistrictServiceTests()
		{
			var configuration = new MapperConfiguration(cfg =>
			{
				cfg.CreateMap<SchoolDistrict, SchoolDistrictDto>();
				cfg.CreateMap<SchoolDistrictDto, SchoolDistrict>();
			});

			_mapper = configuration.CreateMapper();
			_mockSchoolDistrictRepository = new Mock<ISchoolDistrictRepository>();
			_schoolDistrictService = new SchoolDistrictService(_mockSchoolDistrictRepository.Object, _mapper);
			_faker = new Faker();
		}

		private List<SchoolDistrict> GenerateFakeSchoolDistricts(int count)
		{
			var fakeSchoolDistricts = new Faker<SchoolDistrict>()
				.RuleFor(sd => sd.Name, f => f.Company.CompanyName())
				.RuleFor(sd => sd.Description, f => f.Lorem.Sentence())
				.RuleFor(sd => sd.City, f => f.Address.City())
				.RuleFor(sd => sd.Superintendent, f => f.Name.FullName())
				.RuleFor(sd => sd.IsPublic, f => f.Random.Bool())
				.RuleFor(sd => sd.NumberOfSchools, f => f.Random.Int(1, 1000))
				.Generate(count);

			return fakeSchoolDistricts;
		}

		[Fact]
		public async Task GetAllSchoolDistrictsAsync_ShouldReturnAllSchoolDistrictDtos()
		{
			// Arrange
			var fakeSchoolDistricts = GenerateFakeSchoolDistricts(5);
			_mockSchoolDistrictRepository.Setup(x => x.GetAllSchoolDistrictsAsync()).ReturnsAsync(fakeSchoolDistricts);

			// Act
			var result = await _schoolDistrictService.GetAllSchoolDistrictsAsync();

			// Assert
			result.Should().BeAssignableTo<IEnumerable<SchoolDistrictDto>>().Subject.Should().BeEquivalentTo(_mapper.Map<IEnumerable<SchoolDistrictDto>>(fakeSchoolDistricts));
		}

		[Fact]
		public async Task GetSchoolDistrictByIdAsync_WithExistingId_ShouldReturnSchoolDistrictDto()
		{
			// Arrange
			var existingId = ObjectId.GenerateNewId().ToString();
			var existingSchoolDistrict = GenerateFakeSchoolDistricts(1)[0];
			existingSchoolDistrict.Id = existingId;
			_mockSchoolDistrictRepository.Setup(x => x.GetSchoolDistrictByIdAsync(existingId)).ReturnsAsync(existingSchoolDistrict);

			// Act
			var result = await _schoolDistrictService.GetSchoolDistrictByIdAsync(existingId);

			// Assert
			result.Should().BeEquivalentTo(existingSchoolDistrict);
		}

		[Fact]
		public async Task GetSchoolDistrictByIdAsync_WithInvalidId_ShouldThrowInvalidOperationException()
		{
			// Arrange
			var invalidId = _faker.Name.FullName();
			_mockSchoolDistrictRepository.Setup(x => x.GetSchoolDistrictByIdAsync(invalidId)).ReturnsAsync((SchoolDistrict)null);

			//Act
			Func<Task> action = async () => await _schoolDistrictService.GetSchoolDistrictByIdAsync(invalidId);

			// Assert
			await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(ResponseMessages.SchoolDistrictNotFound);
		}
		[Fact]
		public async Task CreateSchoolDistrictAsync_WithValidData_ShouldReturnCreatedSchoolDistrictDto()
		{
			// Arrange
			var newSchoolDistrictModel = GenerateFakeSchoolDistricts(1)[0];
			_mockSchoolDistrictRepository.Setup(x => x.CreateSchoolDistrictAsync(It.IsAny<SchoolDistrict>())).ReturnsAsync(newSchoolDistrictModel);

			// Act
			var result = await _schoolDistrictService.CreateSchoolDistrictAsync(_mapper.Map<SchoolDistrictDto>(newSchoolDistrictModel));

			// Assert
			result.Should().BeEquivalentTo(_mapper.Map<SchoolDistrictDto>(newSchoolDistrictModel));
		}

	}
}
