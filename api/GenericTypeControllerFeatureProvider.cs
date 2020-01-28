using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace EfConfigurationProvider.Api
{
    public class AssembliesCache : IEnumerable<Assembly>
    {
        public IEnumerable<Assembly> Assemblies { get; private set; }

        public AssembliesCache(Assembly[] assemblies)
        {
            Assemblies = assemblies;
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
        private readonly Assembly currentAssembly;

        public GenericTypeControllerFeatureProvider(Assembly assembly)
        {
            currentAssembly = assembly;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            IEnumerable<Type> candidates = currentAssembly
                .GetExportedTypes()
                .Where(x => x.GetCustomAttributes(typeof(GeneratedControllerAttribute), true).Any());

            foreach (Type candidate in candidates)
            {
                feature.Controllers.Add(
                    typeof(PartialUpdateController<>).MakeGenericType(candidate).GetTypeInfo()
                );
            }
        }
    }
}
