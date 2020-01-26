using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace EfConfigurationProvider.Api
{
    public class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly Assembly currentAssembly;

        public GenericTypeControllerFeatureProvider(Assembly assembly)
        {
            currentAssembly = assembly;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            //Assembly currentAssembly = typeof(GenericTypeControllerFeatureProvider).Assembly;
            //currentAssembly.GetExportedTypes().Where(v=>v.GetCustomAttributes()
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
