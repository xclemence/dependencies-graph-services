using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies.Graph.Queries;
using Neo4j.Driver;
using Dependencies.Graph.UnitTests.Models;
using System.Linq;

namespace Dependencies.Graph.UnitTests
{

    [TestClass]
    public class QueryExtractorTests
    {
        [TestMethod]
        public void SearchAssemblyExtractorTest()
        {
            var (_, extractor) = "test".GetSearchAssembliesQuery();

            var node = new FakeNode
            {
                Properties = new Dictionary<string, object>
                {
                    ["name"] = "test",
                    ["isNative"] = true,
                    ["creationDate"] = new LocalDateTime(2020, 1, 2, 3, 4, 5)
                }
            };

            var record = new FakeRecord(new Dictionary<string, object> { ["a"] = node });

            var result = extractor.Invoke(record);

            Assert.AreEqual("test", result.name);
            Assert.AreEqual(true, result.isNative);
            Assert.AreEqual(new DateTime(2020, 1, 2, 3, 4, 5), result.creationDate);
        }
    
        [TestMethod]
        public void SearchAssemblyExtractorErrorTest()
        {
            var (_, extractor) = "test".GetFullAssemblyQuery();

            var node = new FakeNode();
            var record = new FakeRecord(new Dictionary<string, object> { ["badName"] = node });

            Assert.ThrowsException<KeyNotFoundException>(() => _ = extractor.Invoke(record));
        }

        [TestMethod]
        public void GetAssemblyExtractorTest() 
        {
            var (_, extractor) = "test".GetFullAssemblyQuery();

            var nodes = new INode[]
            {
                new FakeNode { Properties = new Dictionary<string, object> { ["name"] = "test", }, },
                new FakeNode { Properties = new Dictionary<string, object> { ["name"] = "test2", } },
            };

            var references = new List<IList<string>>
            {
                new List<string> { "1", "2" },
                new List<string> { "3", "4" },
                new List<string> { "1", "3" },
                new List<string> { "1", "4" },
            };

            var record = new FakeRecord(new Dictionary<string, object> 
            { 
                ["nodes"] = nodes,
                ["references"] = references
            });

            var result = extractor.Invoke(record);

            Assert.AreEqual(2, result.Assemblies.Count);
            Assert.AreEqual("test", result.Assemblies[0].name);

            Assert.AreEqual(2, result.References.Count);
            Assert.AreEqual(3, result.References["1"].Count);
        }

        [TestMethod]
        public void GetAssemblyExtractorPartialTest()
        {
            var (_, extractor) = "test".GetFullAssemblyQuery();

            var nodes = new INode[]
            {
                new FakeNode 
                { 
                    Properties = new Dictionary<string, object> { ["name"] = "test" }, 
                    Labels = new List<string> { "Partial" }
                },
                new FakeNode
                {
                    Properties = new Dictionary<string, object> { ["name"] = "test2" },
                },
            };

            var record = new FakeRecord(new Dictionary<string, object>
            {
                ["nodes"] = nodes,
                ["references"] = new List<IList<string>>()
            });

            var result = extractor.Invoke(record);

            Assert.AreEqual(2, result.Assemblies.Count);
            Assert.AreEqual(true, result.Assemblies[0].isPartial);
            Assert.AreEqual(false, result.Assemblies[1].isPartial);
        }

        [TestMethod]
        public void GetAssemblyExtractorSoftwareTest()
        {
            var (_, extractor) = "test".GetFullAssemblyQuery();

            var nodes = new INode[]
            {
                new FakeNode
                {
                    Properties = new Dictionary<string, object> { ["name"] = "test" },
                    Labels = new List<string> { "Software" }
                },
                new FakeNode
                {
                    Properties = new Dictionary<string, object> { ["name"] = "test2" },
                },
            };

            var record = new FakeRecord(new Dictionary<string, object>
            {
                ["nodes"] = nodes,
                ["references"] = new List<IList<string>>()
            });

            var result = extractor.Invoke(record);

            Assert.AreEqual(2, result.Assemblies.Count);
            Assert.AreEqual(true, result.Assemblies[0].hasEntryPoint);
            Assert.AreEqual(false, result.Assemblies[1].hasEntryPoint);
        }

        [TestMethod]
        public void GetAssemblyExtractorBadNodesNameTest()
        {
            var (_, extractor) = "test".GetFullAssemblyQuery();

            var record = new FakeRecord(new Dictionary<string, object>
            {
                ["BadName"] = Array.Empty<INode>(),
                ["references"] = Enumerable.Empty<IList<string>>()
            });

            Assert.ThrowsException<KeyNotFoundException>(() => _ = extractor.Invoke(record));
        }

        [TestMethod]
        public void GetAssemblyExtractorBadReferenceNameTest()
        {
            var (_, extractor) = "test".GetFullAssemblyQuery();

            var record = new FakeRecord(new Dictionary<string, object>
            {
                ["nodes"] = Array.Empty<INode>(),
                ["BadName"] = Enumerable.Empty<IList<string>>()
            });

            Assert.ThrowsException<KeyNotFoundException>(() => _ = extractor.Invoke(record));
        }
    }
}
