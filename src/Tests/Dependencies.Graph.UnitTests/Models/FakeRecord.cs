using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver;

namespace Dependencies.Graph.UnitTests.Models
{
    public class FakeRecord : IRecord
    {
        public FakeRecord(IReadOnlyDictionary<string, object> values) => Values = values;

        public object this[int index] => Values[Keys[index]];

        public object this[string key] => Values[key];

        public IReadOnlyDictionary<string, object> Values { get; }

        public IReadOnlyList<string> Keys => Values.Keys.ToArray();

    }
}
