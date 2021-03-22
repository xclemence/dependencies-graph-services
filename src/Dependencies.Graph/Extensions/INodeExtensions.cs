using Neo4j.Driver;
using System.Text.Json;

namespace Dependencies.Graph.Extensions
{
    internal static class INodeExtensions
    {
        internal static T To<T>(this INode node)
        {
            var nodeProps = JsonSerializer.Serialize(node.Properties);
            return JsonSerializer.Deserialize<T>(nodeProps);
        }
    }
}
