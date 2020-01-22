using MediatR;

namespace EfConfigurationProvider.Core
{
    public class Update : IRequest
    {
        public ConfigurationValue[] Values { get; set; }
    }
}
