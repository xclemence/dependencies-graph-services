using System.Collections.Generic;

namespace Dependencies.Graph.Queries
{
    public class AssembliesTree
    {
        public IList<AssemblyGraph> Assemblies { get; init; }
        public IDictionary<string, List<string>> References { get; init; }
    }
}
