using System.Collections.Generic;
using System.Linq;
using Dependencies.Graph.Models;
using Dependencies.Graph.Queries;

namespace Dependencies.Graph.Extensions
{
    internal static class AssembliesTreeExtensions
    {
        internal static IEnumerable<Assembly> ToAssembly(this AssembliesTree assembliesTree)
        {
            return assembliesTree.Assemblies.Select(x =>
            {
                var assembly = x.ToAssembly();

                if (assembliesTree.References.ContainsKey(x.name))
                    assembly.AssembliesReferenced.AddRange(assembliesTree.References[x.name]);

                return assembly;
            });
        }
    }
}
