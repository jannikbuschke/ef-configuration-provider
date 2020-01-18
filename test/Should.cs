using Microsoft.Extensions.Configuration;
using Xunit;

namespace EfConfigurationProvider.Test
{
    public class Should : BaseIntegrationTestClass
    {
        public Should(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public void Have_Initial_Values()
        {
            IConfiguration config = GetRequiredService<IConfiguration>();
            Assert.Equal("value123", config.GetValue<string>("cs"));
        }

        [Fact]
        public async void Insert_Values()
        {
            (var id1, var value1) = ("Hello:World", "Hi");
            (var id2, var value2) = ("ConnectionString", "Server:Secret");

            await Send(new Update
            {
                UpsertValues = new System.Collections.Generic.List<ConfigurationValue>
                {
                    new ConfigurationValue { Name = id1, Value = value1 },
                    new ConfigurationValue { Name = id2, Value = value2 }
                }
            });

            IConfiguration config = GetRequiredService<IConfiguration>();
            Assert.Equal(value1, config.GetValue<string>(id1));
            Assert.Equal(value2, config.GetValue<string>(id2));
        }

        [Fact]
        public async void Update_Value()
        {
            (var id1, var value1) = ("key5", "foobar");
            IConfiguration config = GetRequiredService<IConfiguration>();
            Assert.Equal("value5", config.GetValue<string>(id1));

            await Send(new Update
            {
                UpsertValues = new System.Collections.Generic.List<ConfigurationValue>
                {
                    new ConfigurationValue { Name = id1, Value = value1 },
                }
            });

            config = GetRequiredService<IConfiguration>();
            Assert.Equal(value1, config.GetValue<string>(id1));
        }
    }
}
