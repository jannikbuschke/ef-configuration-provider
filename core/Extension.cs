using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace EfConfigurationProvider
{
    public static class Extension
    {
        public static PartialUpdate ToPartialUpdate<T>(this PartialUpdateRaw<T> value)
        {
            var json = JsonConvert.SerializeObject(value.Value);
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            return new PartialUpdate
            {
                Path = value.Path,
                Values = values.Select(v => new ConfigurationValue { Name = v.Key, Value = v.Value }).ToArray()
            };
        }
    }
}
