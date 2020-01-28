using System.ComponentModel.DataAnnotations;
using EfConfigurationProvider.Api;

namespace EfConfigurationProvider
{
    [GeneratedController("strongly-typed-options", Title = "Options One")]
    public class StronglyTypedOptions
    {
        [Required]
        public int Value1 { get; set; }
        public bool Flag { get; set; }
        [EmailAddress]
        public string Value3 { get; set; }
    }

    [GeneratedController("strongly-typed-options-2", Title="Options Two")]
    public class StronglyTypedOptions2
    {
        [Required]
        public int Value1 { get; set; }
        public bool Flag { get; set; }
        [Required, EmailAddress]
        public string Value3 { get; set; }
    }

    [GeneratedController("mails",Title ="Mails")]
    public class MailsOptions
    {
        public string Foo { get; set; }
        public int Number { get; set; }
        public bool HelloWorld { get; set; }

    }
}
