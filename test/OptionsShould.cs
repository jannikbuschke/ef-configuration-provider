using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;

namespace EfConfigurationProvider.Test
{
    public class OptionsShould : BaseIntegrationTestClass
    {
        public OptionsShould(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async void Read_StronglyTypedOption()
        {
            IConfiguration configuration = GetRequiredService<IConfiguration>();
            var value1 = configuration.GetValue<int>("strongly-typed-options:value1");
            var flag = configuration.GetValue<bool>("strongly-typed-options:flag");
            var value3 = configuration.GetValue<string>("strongly-typed-options:value3");
            Assert.Equal(123, value1);
            Assert.True(flag);
            Assert.Equal("hello world", value3);

            IOptions<StronglyTypedOptions> options = GetRequiredService<IOptions<StronglyTypedOptions>>();
            StronglyTypedOptions value = options.Value;
            Assert.Equal(123, value.Value1);
            Assert.True(value.Flag);
            Assert.Equal("hello world", value.Value3);
        }

        [Fact]
        public async void Update_StronglyTypedOption()
        {
            HttpResponseMessage postResponse = await client.PostAsJsonAsync("/strongly-typed-options-2",
                new PartialUpdateRaw<StronglyTypedOptions2>
                {
                    Path = "strongly-typed-options-2",
                    Value = new StronglyTypedOptions2
                    {
                        Flag = true,
                        Value1 = 555,
                        Value3 = "foo@email.com"
                    }
                });
            postResponse.EnsureSuccessStatusCode();

            HttpResponseMessage getResponse = await client.GetAsync("/strongly-typed-options-2");
            StronglyTypedOptions2 data = await getResponse.Content.ReadAsAsync<StronglyTypedOptions2>();
            Assert.True(data.Flag);
            Assert.Equal(555, data.Value1);
            Assert.Equal("foo@email.com", data.Value3);
        }
    }
}
