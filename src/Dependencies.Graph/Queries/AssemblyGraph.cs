using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Dependencies.Graph.JsonConverters;
using Neo4j.Driver;

namespace Dependencies.Graph.Queries
{
    [SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Neo4j naming style for properties (for auto mapping)")]
    public class AssemblyGraph
    {
        public string name { get; set; }

        public string shortName { get; set; }

        public bool isNative { get; set; }

        public string version { get; set; }

        public string creator { get; set; }

        public string targetFramework { get; set; }

        public string targetProcessor { get; set; }

        public bool? isDebug { get; set; }

        public bool isILOnly { get; set; }

        public bool? isPartial { get; set; }

        public bool? hasEntryPoint { get; set; }

        [JsonConverter(typeof(LocalDateTimeJsonConverter))]
        public LocalDateTime creationDate { get; set; }
    }
}
