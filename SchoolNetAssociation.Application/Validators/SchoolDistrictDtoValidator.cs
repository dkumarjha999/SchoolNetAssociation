using FluentValidation;
using SchoolNetAssociation.Application.DTOs;

namespace SchoolNetAssociation.Application.Validators
{
	public class SchoolDistrictDtoValidator : AbstractValidator<SchoolDistrictDto>
	{
		public SchoolDistrictDtoValidator()
		{
			RuleFor(sd => sd.Name).NotNull().NotEmpty().WithMessage("{PropertyName} should be not empty.");
			RuleFor(sd => sd.City).NotNull().NotEmpty().WithMessage("{PropertyName} should be not empty.");
			RuleFor(sd => sd.NumberOfSchools).NotNull().GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.");
		}
	}
}
