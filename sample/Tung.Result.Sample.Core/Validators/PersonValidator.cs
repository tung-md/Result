using Tung.Result.Sample.Core.Model;
using FluentValidation;

namespace Tung.Result.Sample.Core.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Surname)
                .NotEmpty()
                .MaximumLength(10)
                .NotEqual("SomeLongName");
            RuleFor(x => x.Forename).NotEmpty().WithMessage("Please specify a first name");
        }
    }
}
