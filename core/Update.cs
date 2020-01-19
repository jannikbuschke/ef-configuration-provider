using MediatR;

namespace EfConfigurationProvider
{
    public class Update : IRequest
    {
        public ConfigurationValue[] Values { get; set; }
    }
}
