using System.Collections.Generic;
using MediatR;

namespace EfConfigurationProvider
{
    public class Update : IRequest
    {
        public List<ConfigurationValue> UpsertValues { get; set; }
        public List<string> DeleteValues { get; set; }
    }
}
