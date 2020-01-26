using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EfConfigurationProvider.Core
{
    public class UpdateHandler : IRequestHandler<Update>
    {
        public async Task<Unit> Handle(Update request, CancellationToken cancellationToken)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            EntityFrameworkExtensions.optionsAction(builder);
            using var ctx = new DataContext(builder.Options);

            var validator = new UpdateValidator();
            FluentValidation.Results.ValidationResult result = validator.Validate(request);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }

            ctx.Configurations.Add(new Configuration
            {
                Values = request.Values.ToDictionary(value => value.Name, v => v.Value),
                Created = DateTime.UtcNow,
            });

            await ctx.SaveChangesAsync();
            ConfigurationProvider.Value.Reload();
            return Unit.Value;
        }
    }
}
