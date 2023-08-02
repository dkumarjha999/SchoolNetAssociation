using System.Net;
using System.Net.Http.Json;
using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Bson;
using SchoolNetAssociation.Application.Common;
using SchoolNetAssociation.Application.DTOs;
using SchoolNetAssociation.Application.Mappings;
using SchoolNetAssociation.Domain.Entities;

namespace SchoolNetAssociation.IntegrationTest
{
	public class SchoolDistrictControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
	{
		private readonly Faker _faker;
		private readonly IMapper _mapper;
		private readonly HttpClient _httpClient;

		const string ClientBaseUrl = "api/SchoolDistrict";

		public SchoolDistrictControllerIntegrationTests(WebApplicationFactory<Program> factory)
		{
			_httpClient = factory.CreateClient();
			_faker=new Faker();
			_mapper = new MapperConfiguration(cfg => cfg.AddProfile<SchoolNetAssociationProfile>()).CreateMapper();
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
			// Act
			var response = await _httpClient.GetAsync(ClientBaseUrl);
			var result = await response.Content.ReadFromJsonAsync<SchoolDistrictDto[]>();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			result.Should().NotBeEmpty().And.BeAssignableTo<SchoolDistrictDto[]>();
		}

		[Fact]
		public async Task GetSchoolDistrictByIdAsync_ExistingId_ShouldReturnOkWithSchoolDistrict()
		{
			// Arrange
			var existingId = ObjectId.GenerateNewId().ToString();
			var expectedSchoolDistrict = GetFakeSchoolDistrictDtos(1);
			expectedSchoolDistrict[0].Id=existingId;


			// Act 
			var response = await _httpClient.GetAsync($"{ClientBaseUrl}/{existingId}");
			var result = await response.Content.ReadFromJsonAsync<SchoolDistrictDto>();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			result.Id.Should().NotBeNull().And.Be(existingId);
		}

		[Fact]
		public async Task GetSchoolDistrictByIdAsync_NonExistingId_ShouldReturnNotFound()
		{
			// Arrange
			var nonExistingId = ObjectId.GenerateNewId().ToString();

			// Act
			var response = await _httpClient.GetAsync($"{ClientBaseUrl}/{nonExistingId}");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			var message = await response.Content.ReadAsStringAsync();
			message.Should().Contain(ResponseMessages.SchoolDistrictNotFound);
		}

		[Fact]
		public async Task CreateSchoolDistrictAsync_ValidData_ShouldReturnCreatedAndCorrectData()
		{
			// Arrange
			var newSchoolDistrict = GetFakeSchoolDistrictDtos(1)[0];

			// Act
			var response = await _httpClient.PostAsJsonAsync(ClientBaseUrl, newSchoolDistrict);
			var createdSchoolDistrict = await response.Content.ReadFromJsonAsync<SchoolDistrictDto>();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			createdSchoolDistrict.Should().BeEquivalentTo(newSchoolDistrict, options => options.Excluding(x => x.Id));
		}

		[Fact]
		public async Task UpdateSchoolDistrictAsync_ExistingIdAndValidData_ShouldReturnOkAndUpdatedData()
		{
			// Arrange
			var existingId = ObjectId.GenerateNewId().ToString();
			var updatedSchoolDistrict = GetFakeSchoolDistrictDtos(1)[0];
			updatedSchoolDistrict.Id=existingId;
			// Act
			var response = await _httpClient.PutAsJsonAsync($"{ClientBaseUrl}/{existingId}", updatedSchoolDistrict);
			var updatedResult = await response.Content.ReadFromJsonAsync<SchoolDistrictDto>();
			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			updatedResult.Should().Be(updatedSchoolDistrict);

		}

		[Fact]
		public async Task UpdateSchoolDistrictAsync_NonExistingId_ShouldReturnNotFound()
		{
			// Arrange
			var nonExistingId = _faker.Random.Guid().ToString();
			var updatedSchoolDistrict = GetFakeSchoolDistrictDtos(1)[0];

			// Act
			var response = await _httpClient.PutAsJsonAsync($"{ClientBaseUrl}/{nonExistingId}", updatedSchoolDistrict);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task DeleteSchoolDistrictAsync_ExistingId_ShouldReturnNoContent()
		{
			// Arrange
			var existingId = _faker.Random.Guid().ToString();

			// Act
			var response = await _httpClient.DeleteAsync($"{ClientBaseUrl}/{existingId}");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}

		[Fact]
		public async Task DeleteSchoolDistrictAsync_NonExistingId_ShouldReturnNotFound()
		{
			// Arrange
			var nonExistingId = _faker.Random.Guid().ToString();

			// Act
			var response = await _httpClient.DeleteAsync($"{ClientBaseUrl}/{nonExistingId}");

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}
	}
}
