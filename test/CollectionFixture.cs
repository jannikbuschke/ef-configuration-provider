using EfConfigurationProvider.Sample;
using Xunit;

namespace EfConfigurationProvider.Test
{
    [CollectionDefinition("Database collection")]
    public class CollectionFixture : ICollectionFixture<CustomWebApplicationFactory<Startup>>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
