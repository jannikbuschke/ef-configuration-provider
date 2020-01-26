using EfConfigurationProvider.Core;
using EfConfigurationProvider.Sample;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EfConfigurationProvider.Test
{
    public class CoreShould : BaseIntegrationTestClass
    {
        public CoreShould(CustomWebApplicationFactory<Startup> factory) : base(factory) { }

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
                Values = new[]
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
            (var id1, var value1) = ("key6", "foobar");
            IConfiguration config = GetRequiredService<IConfiguration>();
            Assert.Equal("value6", config.GetValue<string>(id1));

            await Send(new Update
            {
                Values = new[]
                {
                    new ConfigurationValue { Name = id1, Value = value1 }
                }
            });

            config = GetRequiredService<IConfiguration>();
            Assert.Equal(value1, config.GetValue<string>(id1));
        }

        [Fact]
        public async void Throw_ValidationException_On_Duplicate_Upserts()
        {
            (var id1, var value1) = ("key10", "value10");

            await Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await Send(new Update
                {
                    Values = new[]
                    {
                        new ConfigurationValue { Name = id1, Value = value1 },
                        new ConfigurationValue { Name = id1, Value = value1 },
                    }
                });
            });
        }

        [Fact]
        public async void Partial_Update()
        {
            (var id1, var value1) = ("key11", "value11");

            await Send(new PartialUpdate
            {
                Path = "paath",
                Values = new[]
                    {
                        new ConfigurationValue { Name = id1, Value = value1 },
                    }
            });

            IConfiguration config = GetRequiredService<IConfiguration>();
            Assert.Equal(value1, config.GetValue<string>("paath:" + id1));

        }
    }
}
