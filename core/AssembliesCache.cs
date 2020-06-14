using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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
}
