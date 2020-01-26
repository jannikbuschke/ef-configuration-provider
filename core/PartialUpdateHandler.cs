using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EfConfigurationProvider.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EfConfigurationProvider
{
    public class PartialUpdateHandler : IRequestHandler<PartialUpdate>
    {
        public async Task<Unit> Handle(PartialUpdate request, CancellationToken cancellationToken)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            EntityFrameworkExtensions.optionsAction(builder);
            using var ctx = new DataContext(builder.Options);

            Configuration current = await ctx.Configurations.OrderByDescending(v => v.Created).FirstOrDefaultAsync();

            if (current == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                Dictionary<string, string> values = current.Values;
                var @new = new Dictionary<string, string>(values);
                foreach (ConfigurationValue value in request.Values)
                {
                    @new[request.Path + ":" + value.Name] = value.Value;
                }

                ctx.Configurations.Add(new Configuration
                {
                    Values = @new,
                    Created = DateTime.UtcNow,
                });

                await ctx.SaveChangesAsync();
                ConfigurationProvider.Value.Reload();
            }

            return Unit.Value;
        }
    }
}
