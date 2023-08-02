using AutoFixture;
using FluentValidation.TestHelper;
using SchoolNetAssociation.Application.DTOs;
using SchoolNetAssociation.Application.Validators;

namespace SchoolNetAssociation.UntiTest.Application.Validators
{
	public class SchoolDistrictDtoValidatorTests
	{
		private SchoolDistrictDtoValidator _validator;
		private Fixture _fixture;
		public SchoolDistrictDtoValidatorTests()
		{
			_validator = new SchoolDistrictDtoValidator();
			_fixture = new Fixture();
		}

		[Fact]
		public void ValidatorShouldReturnErrorWhenNameIsNull()
		{
			//Arrange
			var schoolDistrictDto = _fixture.Create<SchoolDistrictDto>();
			schoolDistrictDto.Name =null;
			//Act
			var result = _validator.TestValidate(schoolDistrictDto);
			//Assert
			result.ShouldHaveValidationErrorFor(sd => sd.Name);
		}

		[Fact]
		public void ValidatorShouldReturnErrorWhenNameIsEmpty()
		{
			//Arrange
			var schoolDistrictDto = _fixture.Create<SchoolDistrictDto>();
			schoolDistrictDto.Name ="";
			//Act
			var result = _validator.TestValidate(schoolDistrictDto);
			//Assert
			result.ShouldHaveValidationErrorFor(sd => sd.Name);
		}

		[Fact]
		public void ValidatorShouldReturnErrorCityIsNull()
		{
			//Arrange
			var schoolDistrictDto = _fixture.Create<SchoolDistrictDto>();
			schoolDistrictDto.City=null;
			//Act
			var result = _validator.TestValidate(schoolDistrictDto);
			//Assert
			result.ShouldHaveValidationErrorFor(sd => sd.City);
		}

		[Fact]
		public void ValidatorShouldReturnErrorCityIsEmpty()
		{
			//Arrange
			var schoolDistrictDto = _fixture.Create<SchoolDistrictDto>();
			schoolDistrictDto.City="";
			//Act
			var result = _validator.TestValidate(schoolDistrictDto);
			//Assert
			result.ShouldHaveValidationErrorFor(sd => sd.City);
		}

		[Fact]
		public void ValidatorShouldReturnErrorWhenNumberOfSchoolsIsNotGreaterThanZero()
		{
			//Arrange
			var schoolDistrictDto = _fixture.Create<SchoolDistrictDto>();
			schoolDistrictDto.NumberOfSchools = 0;
			//Act
			var result = _validator.TestValidate(schoolDistrictDto);
			//Assert
			result.ShouldHaveValidationErrorFor(sd => sd.NumberOfSchools);
		}

		[Fact]
		public void ShouldNotHaveAnyErrorsWhenDtoIsValid()
		{
			//Arrange
			var schoolDistrictDto = _fixture.Create<SchoolDistrictDto>();
			//Act
			var result = _validator.TestValidate(schoolDistrictDto);
			//Assert
			result.ShouldNotHaveAnyValidationErrors();
		}
	}
}
