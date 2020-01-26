using System.ComponentModel.DataAnnotations;
using EfConfigurationProvider.Api;

namespace EfConfigurationProvider
{
    [GeneratedController("strongly-typed-options")]
    public class StronglyTypedOptions
    {
        [Required]
        public int Value1 { get; set; }
        public bool Flag { get; set; }
        [EmailAddress]
        public string Value3 { get; set; }
    }

    [GeneratedController("strongly-typed-options-2")]
    public class StronglyTypedOptions2
    {
        [Required]
        public int Value1 { get; set; }
        public bool Flag { get; set; }
        [EmailAddress]
        public string Value3 { get; set; }
    }
}
