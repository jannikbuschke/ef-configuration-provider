using System;

namespace EfConfigurationProvider.Core
{
    public class ConfigurationValue : IEquatable<ConfigurationValue>
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public bool Equals(ConfigurationValue other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
