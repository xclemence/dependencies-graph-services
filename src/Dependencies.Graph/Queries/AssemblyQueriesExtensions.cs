using System;
using System.Collections.Generic;
using System.Linq;
using Dependencies.Graph.Extensions;
using Dependencies.Graph.Models;
using Neo4j.Driver;

namespace Dependencies.Graph.Queries
{
    internal static class AssemblyQueriesExtensions
    {
        public static Query GetAddFullAssemblyQuery(this IEnumerable<Assembly> assemblies)
        {
            var formattedAssemblies = assemblies.Where(x => !x.IsPartial && !x.HasEntryPoint)
                                                .Select(x => x.ToAssemblyGraph());

            var query = @"UNWIND $assemblies AS assembly
                          MERGE (a:Assembly { name: assembly.name })
                            ON CREATE SET a = assembly
                            On MATCH SET a.version = assembly.version REMOVE a:Partial";

            return new Query(query, new { assemblies = formattedAssemblies.ToArray() });
        }

        public static Query GetAddSoftwareQuery(this IEnumerable<Assembly> assemblies)
        {
            var formattedAssemblies = assemblies.Where(x => x.HasEntryPoint)
                                                .Select(x => x.ToAssemblyGraph());

            var query = @"UNWIND $assemblies AS assembly
                          MERGE (a:Assembly:Software { name: assembly.name })
                            ON CREATE SET a = assembly";

            return new Query(query, new { assemblies = formattedAssemblies.ToArray() });
        }

        public static Query GetAddPartialAssemblyQuery(this IEnumerable<Assembly> assemblies)
        {
            var formattedAssemblies = assemblies.Where(x => x.IsPartial)
                                                .Select(x => x.ToAssemblyGraph());

            var query = @"UNWIND $assemblies AS assembly
                          MERGE (a:Assembly { name: assembly.name })
                            ON CREATE SET a = assembly SET a:Partial";

            return new Query(query, new { assemblies = formattedAssemblies.ToArray() });
        }

        public static (Query query, Func<IRecord, AssemblyGraph> resultExtractor) GetSearchAssembliesQuery(this string assemblyName)
        {
            var query = @$"MATCH (a:Assembly)
                           WHERE a.name =~ $filter AND NOT 'Partial' IN labels(a)
                           RETURN a";

            return (query: new Query(query, new { filter = $"(?i).*{ assemblyName }.*" }),
                    resultExtractor: x => x["a"].As<INode>().To<AssemblyGraph>());
        }

        public static Query GetAddReferenceQuery(this IEnumerable<Assembly> assemblies)
        {
            var references = assemblies.SelectMany(x => x.AssembliesReferenced, (a, r) => new { name = a.Name, referenceName = r }).ToArray();

            var query = @"UNWIND $references AS ref
                          MATCH (a:Assembly {name: ref.name}), (r: Assembly {name: ref.referenceName})
                          MERGE (a)-[:REFERENCE]->(r)";

            return new Query(query, new { references });
        }

        public static (Query query, Func<IRecord, AssembliesTree> resultExtractor) GetFullAssemblyQuery(this string assemblyName)
        {
            var query = @"MATCH p = (:Assembly {name: $assemblyName})-[:REFERENCE*0..]->(x)
                          WITH *, relationships(p) AS r
                          WITH collect(DISTINCT x) as nodes, [r in collect(distinct last(r)) | [startNode(r).name,endNode(r).name]] as references
                          RETURN nodes, references";

            return (query: new Query(query, new { assemblyName }),
                    resultExtractor: x => new AssembliesTree
                    {
                        Assemblies = x["nodes"].As<IList<INode>>().Select(x =>
                        {
                            var item = x.To<AssemblyGraph>();
                            item.isPartial = x.Labels.Contains("Partial");
                            item.hasEntryPoint = x.Labels.Contains("Software");
                            return item;
                        }).ToList(),
                        References = x["references"].As<IList<IList<string>>>().GroupBy(x => x[0], x => x[1]).ToDictionary(x => x.Key, x => x.ToList())
                    });
        }

    }
}
