using System.Collections.Generic;
using System.Net.Http;
using EfConfigurationProvider.Core;
using EfConfigurationProvider.Sample;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Xunit;

namespace EfConfigurationProvider.Test
{
    public class ApiShould : BaseIntegrationTestClass
    {
        public ApiShould(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async void Have_Initial_Values()
        {
            HttpResponseMessage response = await client.GetAsync("api/__configuration/current");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Configuration config = JObject.Parse(content).ToObject<Configuration>();
            Dictionary<string, string> data = config.Values;
            Assert.NotNull(data);
            Assert.True(data.ContainsKey("key1"));
            Assert.Equal("value1", data["key1"]);
        }

        [Fact]
        public async void Insert_Values()
        {
            (var id1, var value1) = ("Hello:World123", "Hi123");
            (var id2, var value2) = ("ConnectionString123", "Server:Secret123");

            HttpResponseMessage response = await client.PostAsJsonAsync("api/__configuration/update",
                new Update
                {
                    Values = new[]
                    {
                        new ConfigurationValue { Name = id1, Value = value1 },
                        new ConfigurationValue { Name = id2, Value = value2 }
                    }
                });
            response.EnsureSuccessStatusCode();

            IConfiguration config = GetRequiredService<IConfiguration>();
            Assert.Equal(value1, config.GetValue<string>(id1));
            Assert.Equal(value2, config.GetValue<string>(id2));
        }
    }
}
