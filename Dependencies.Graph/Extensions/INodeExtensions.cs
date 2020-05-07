using Neo4j.Driver;
using Newtonsoft.Json;

namespace Dependencies.Graph.Extensions
{
    internal static class INodeExtensions
    {
        internal static T To<T>(this INode node)
        {
            var nodeProps = JsonConvert.SerializeObject(node.Properties);
            return JsonConvert.DeserializeObject<T>(nodeProps);
        }
    }
}
