using MediatR;

namespace EfConfigurationProvider
{
    public class PartialUpdateRaw<T> : IRequest
    {
        public string Path { get; set; }
        public T Value { get; set; }
    }
}
