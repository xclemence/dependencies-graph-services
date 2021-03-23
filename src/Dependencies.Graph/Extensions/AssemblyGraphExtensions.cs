using Dependencies.Graph.Models;
using Dependencies.Graph.Queries;
using Neo4j.Driver;

namespace Dependencies.Graph.Extensions
{
    internal static class AssemblyGraphExtensions
    {
        internal static AssemblyGraph ToAssemblyGraph(this Assembly assembly)
        {
            return new AssemblyGraph
            {
                name = assembly.Name,
                shortName = assembly.ShortName,
                isNative = assembly.IsNative,
                version = assembly.Version,
                creationDate = new LocalDateTime(assembly.CreationDate),
                creator = assembly.Creator,
                isDebug = assembly.IsDebug,
                isILOnly = assembly.IsILOnly,
                targetFramework = assembly.TargetFramework,
                targetProcessor = assembly.TargetProcessor
            };
        }

        internal static Assembly ToAssembly(this AssemblyGraph assembly)
        {
            return new Assembly
            {
                Name = assembly.name,
                ShortName = assembly.shortName,
                IsNative = assembly.isNative,
                Version = assembly.version,
                CreationDate = assembly.creationDate.ToDateTime(),
                Creator = assembly.creator,
                IsDebug = assembly.isDebug,
                IsILOnly = assembly.isILOnly,
                TargetFramework = assembly.targetFramework,
                TargetProcessor = assembly.targetProcessor,
                IsPartial = assembly.isPartial ?? false,
                HasEntryPoint = assembly.hasEntryPoint ?? false
            };
        }
    }
}
