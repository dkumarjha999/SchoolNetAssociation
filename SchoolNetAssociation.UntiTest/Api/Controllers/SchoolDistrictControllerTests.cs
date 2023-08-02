using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using SchoolNetAssociation.API.Controllers;
using SchoolNetAssociation.Application.Common;
using SchoolNetAssociation.Application.DTOs;
using SchoolNetAssociation.Application.Mappings;
using SchoolNetAssociation.Application.Services;
using SchoolNetAssociation.Application.Validators;
using SchoolNetAssociation.Domain.Entities;

namespace SchoolNetAssociation.UntiTest.Api.Controllers
{
	public class SchoolDistrictControllerTests
	{
		private readonly Mock<ISchoolDistrictService> _mockSchoolDistrictService;
		private readonly SchoolDistrictController _controller;
		private readonly IMapper _mapper;
		private readonly Faker _faker;

		public SchoolDistrictControllerTests()
		{
			_mockSchoolDistrictService = new Mock<ISchoolDistrictService>();
			_mapper = new MapperConfiguration(cfg => cfg.AddProfile<SchoolNetAssociationProfile>()).CreateMapper();
			_controller = new SchoolDistrictController(_mockSchoolDistrictService.Object);
			_faker = new Faker();
		}

		private List<SchoolDistrictDto> GetFakeSchoolDistrictDtos(int count)
		{
			var fakeSchoolDistricts = new Faker<SchoolDistrict>()
				.RuleFor(sd => sd.Name, f => f.Company.CompanyName())
				.RuleFor(sd => sd.Description, f => f.Lorem.Sentence())
				.RuleFor(sd => sd.City, f => f.Address.City())
				.RuleFor(sd => sd.Superintendent, f => f.Name.FullName())
				.RuleFor(sd => sd.IsPublic, f => f.Random.Bool())
				.RuleFor(sd => sd.NumberOfSchools, f => f.Random.Int(1, 10000))
				.Generate(count);

			return _mapper.Map<List<SchoolDistrictDto>>(fakeSchoolDistricts);
		}

		[Fact]
		public async Task GetAllSchoolDistrictsAsync_ShouldReturnOkWithSchoolDistricts()
		{
			// Arrange
			var fakeSchoolDistricts = GetFakeSchoolDistrictDtos(5);
			_mockSchoolDistrictService.Setup(x => x.GetAllSchoolDistrictsAsync()).ReturnsAsync(fakeSchoolDistricts);

			// Act
			var result = await _controller.GetAllSchoolDistrictsAsync();

			// Assert
			_mockSchoolDistrictService.Verify(x => x.GetAllSchoolDistrictsAsync(), Times.Once);
			result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(fakeSchoolDistricts);

		}
		[Fact]

		public async Task GetSchoolDistrictByIdAsync_WithValidId_ReturnsOkResultWithSchoolDistrict()
		{
			// Arrange
			var fakeSchoolDistrict = GetFakeSchoolDistrictDtos(1)[0];
			var schoolDistrictId = ObjectId.GenerateNewId().ToString();  //_faker.Random.Guid() ->not of type ObjectId 
			fakeSchoolDistrict.Id=schoolDistrictId;
			_mockSchoolDistrictService.Setup(x => x.GetSchoolDistrictByIdAsync(schoolDistrictId)).ReturnsAsync(fakeSchoolDistrict);

			// Act
			var result = await _controller.GetSchoolDistrictByIdAsync(schoolDistrictId);

			// Assert
			_mockSchoolDistrictService.Verify(x => x.GetSchoolDistrictByIdAsync(schoolDistrictId), Times.Once);
			result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(fakeSchoolDistrict);
		}

		[Fact]
		public async Task GetSchoolDistrictByIdAsync_WithInvalidId_ReturnsBadRequest()
		{
			// Arrange
			var invalidId = _faker.Name.FullName();

			// Act
			var result = await _controller.GetSchoolDistrictByIdAsync(invalidId);

			// Assert
			result.Should().BeOfType<BadRequestObjectResult>().Which.Value.Should().Be(ResponseMessages.InvalidSchoolDistrictId);
		}

		[Fact]
		public async Task CreateSchoolDistrictAsync_WithValidData_ShouldReturnOkWithCreatedData()
		{
			// Arrange
			var newSchoolDistrictDto = GetFakeSchoolDistrictDtos(1)[0];
			_mockSchoolDistrictService.Setup(x => x.CreateSchoolDistrictAsync(newSchoolDistrictDto)).ReturnsAsync(newSchoolDistrictDto);

			// Act
			var result = await _controller.CreateSchoolDistrictAsync(newSchoolDistrictDto);

			// Assert
			_mockSchoolDistrictService.Verify(x => x.CreateSchoolDistrictAsync(newSchoolDistrictDto), Times.Once);
			result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(newSchoolDistrictDto);
		}

		[Fact]
		public async Task CreateSchoolDistrictAsync_WithInvalidData_ShouldReturnBadRequestWithErrors()
		{
			// Arrange
			var invalidSchoolDistrictDto = new SchoolDistrictDto();
			var validator = new SchoolDistrictDtoValidator();
			var validationResults = validator.Validate(invalidSchoolDistrictDto).Errors.Select(error => error.ErrorMessage).ToList();

			// Act
			var result = await _controller.CreateSchoolDistrictAsync(invalidSchoolDistrictDto);

			// Assert
			result.Should().BeOfType<BadRequestObjectResult>().Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
			var errorMessages = result.As<BadRequestObjectResult>().Value.Should().BeAssignableTo<System.Collections.IEnumerable>().Subject;
			errorMessages.Should().BeEquivalentTo(validationResults);
		}

	}
}
