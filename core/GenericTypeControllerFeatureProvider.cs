using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace EfConfigurationProvider.Core
{
    public class AssembliesCache : IEnumerable<Assembly>
    {
        public IEnumerable<Assembly> Assemblies { get; private set; }

        public AssembliesCache(IEnumerable<Assembly> assemblies)
        {
            Assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
        }

        public IEnumerator<Assembly> GetEnumerator()
        {
            return Assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Assemblies.GetEnumerator();
        }
    }

    public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IEnumerable<Assembly> assemblies;

        public GenericTypeControllerFeatureProvider(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            IEnumerable<Type> candidates = assemblies
                .SelectMany(v => v.GetExportedTypes()
                .Where(x => x.GetCustomAttributes(typeof(GeneratedControllerAttribute), true).Any()));

            foreach (Type candidate in candidates)
            {
                feature.Controllers.Add(
                    typeof(PartialUpdateController<>).MakeGenericType(candidate).GetTypeInfo()
                );
            }
        }
    }
}
