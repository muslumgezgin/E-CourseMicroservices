using System;
using FluentValidation;
using FreeCourse.Web.Models.Catalog;

namespace FreeCourse.Web.Validators
{
    public class CourseCreateInputValidator : AbstractValidator<CourseCreateInput>
    {
        public CourseCreateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description cannot be empty");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1,int.MaxValue).WithMessage("Duration cannot be empty");
            RuleFor(x => x.Price).NotEmpty().WithMessage("Price Cannot be empty")
                .ScalePrecision(2,6).WithMessage("Wrong format");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("select a category");

        }
    }
}

