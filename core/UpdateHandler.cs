using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EfConfigurationProvider
{
    public class UpdateHandler : IRequestHandler<Update>
    {
        public async Task<Unit> Handle(Update request, CancellationToken cancellationToken)
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            EntityFrameworkExtensions.optionsAction(builder);
            using var ctx = new DataContext(builder.Options);
            //ctx.BulkInsertOrUpdate(request.UpsertValues);
            ctx.Values
                .UpsertRange(request.UpsertValues)
                .On(v => v.Name)
                //.WhenMatched(v => new ConfigurationValue { Value = v.Value })
                .Run();

            await ctx.SaveChangesAsync();
            ConfigurationProvider.Value.Reload();
            return Unit.Value;
        }
    }
}
