using EfConfigurationProvider.Core;
using MediatR;

namespace EfConfigurationProvider
{
    public class PartialUpdate : IRequest
    {
        public string Path { get; set; }
        public ConfigurationValue[] Values { get; set; }
    }
}
