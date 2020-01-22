using System.Linq;
using FluentValidation;

namespace EfConfigurationProvider.Core
{
    public class UpdateValidator : AbstractValidator<Update>
    {
        public UpdateValidator()
        {
            RuleFor(v => v.Values).NotNull();
            RuleFor(v => v.Values).Must(BeUnique);
        }

        public bool BeUnique(ConfigurationValue[] values)
        {
            return values.Distinct().Count() == values.Length;
        }
    }
}
