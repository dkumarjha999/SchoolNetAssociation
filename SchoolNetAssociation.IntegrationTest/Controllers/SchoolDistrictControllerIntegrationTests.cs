using System.Net;
using System.Net.Http.Json;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolNetAssociation.Domain.Entities;

namespace SchoolNetAssociation.IntegrationTest
{
    public class SchoolDistrictControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private readonly Faker<SchoolDistrict> _schoolDistrictFaker;
        private readonly Faker _faker;

        const string ClientBaseUrl = "api/SchoolDistrict";

        public SchoolDistrictControllerTests(WebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
            _faker=new Faker();
            _schoolDistrictFaker = new Faker<SchoolDistrict>()
                .RuleFor(sd => sd.Name, f => f.Company.CompanyName())
                .RuleFor(sd => sd.Description, f => f.Lorem.Sentence())
                .RuleFor(sd => sd.City, f => f.Address.City())
                .RuleFor(sd => sd.Superintendent, f => f.Name.FullName())
                .RuleFor(sd => sd.IsPublic, f => f.Random.Bool())
                .RuleFor(sd => sd.NumberOfSchools, f => f.Random.Int(1, 1000));
        }

        [Fact]
        public async Task GetAllSchoolDistrictsAsync_ShouldReturnOkWithSchoolDistricts()
        {
            // Act
            var response = await _httpClient.GetAsync(ClientBaseUrl);
            var result = await response.Content.ReadFromJsonAsync<SchoolDistrict[]>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetSchoolDistrictByIdAsync_ExistingId_ShouldReturnOkWithSchoolDistrict()
        {
            // Arrange
            var existingId = _faker.Random.Guid().ToString();
            var expectedSchoolDistrict = _schoolDistrictFaker.Generate();
            expectedSchoolDistrict.Id=existingId;
            // Act
            var response = await _httpClient.GetAsync($"{ClientBaseUrl}/{expectedSchoolDistrict.Id}");
            var result = await response.Content.ReadFromJsonAsync<SchoolDistrict>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
            result.Should().Be(expectedSchoolDistrict);

        }

        [Fact]
        public async Task GetSchoolDistrictByIdAsync_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistingId = _faker.Random.Guid().ToString();

            // Act
            var response = await _httpClient.GetAsync($"{ClientBaseUrl}/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateSchoolDistrictAsync_ValidData_ShouldReturnCreatedAndCorrectData()
        {
            // Arrange
            var newSchoolDistrict = _schoolDistrictFaker.Generate();

            // Act
            var response = await _httpClient.PostAsJsonAsync(ClientBaseUrl, newSchoolDistrict);
            var createdSchoolDistrict = await response.Content.ReadFromJsonAsync<SchoolDistrict>();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            createdSchoolDistrict.Should().BeEquivalentTo(newSchoolDistrict, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task UpdateSchoolDistrictAsync_ExistingIdAndValidData_ShouldReturnOkAndUpdatedData()
        {
            // Arrange
            var existingId = _faker.Random.Guid().ToString();
            var updatedSchoolDistrict = _schoolDistrictFaker.Generate();
            updatedSchoolDistrict.Id=existingId;
            // Act
            var response = await _httpClient.PutAsJsonAsync($"{ClientBaseUrl}/{existingId}", updatedSchoolDistrict);
            var updatedResult = await response.Content.ReadFromJsonAsync<SchoolDistrict>();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedResult.Should().Be(updatedSchoolDistrict);

        }

        [Fact]
        public async Task UpdateSchoolDistrictAsync_NonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistingId = _faker.Random.Guid().ToString();
            var updatedSchoolDistrict = _schoolDistrictFaker.Generate();

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
