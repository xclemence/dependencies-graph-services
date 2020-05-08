using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies.Graph.Queries;
using Dependencies.Graph.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Dependencies.Graph.UnitTests.Extensions;

namespace Dependencies.Graph.UnitTests
{
    [TestClass]
    public class AssemblyQueryTests
    {
        [TestMethod]
        public void TestSearchAssembliesQuery()
        {
            var (query, _) = "test".GetSearchAssembliesQuery();
            var error = query.ValidateQuery();

            Assert.IsNull(error, error);
        }

        [TestMethod]
        public void TestGetFullAssemblyQuery()
        {
            var (query, _) = "test".GetFullAssemblyQuery();
            var error = query.ValidateQuery();

            Assert.IsNull(error, error);
        }

        [TestMethod]
        public void TestGetAddFullAssemblyQuery()
        {
            var assemblies = new[]
            {
                new Assembly { Name = "test", HasEntryPoint = true },
                new Assembly { Name = "test2", IsPartial = true },
                new Assembly { Name = "test3" },
            };

            var query = assemblies.GetAddFullAssemblyQuery();
            var error = query.ValidateQuery();

            Assert.IsNull(error, error);
            Assert.AreEqual(1, ((IList)query.Parameters.First().Value).Count);
        }

        [TestMethod]
        public void TestGetAddSoftwareQuery()
        {
            var assemblies = new[]
            {
                new Assembly { Name = "test", HasEntryPoint = true },
                new Assembly { Name = "test2", HasEntryPoint = true },
                new Assembly { Name = "test3" },
            };

            var query = assemblies.GetAddSoftwareQuery();
            var error = query.ValidateQuery();

            Assert.IsNull(error, error);
            Assert.AreEqual(2, ((IList)query.Parameters.First().Value).Count);
        }

        [TestMethod]
        public void TestGetAddPartialAssemblyQuery()
        {
            var assemblies = new[]
            {
                new Assembly { Name = "test", HasEntryPoint = true },
                new Assembly { Name = "test2", IsPartial = true },
                new Assembly { Name = "test2", IsPartial = true },
                new Assembly { Name = "test2", IsPartial = true },
                new Assembly { Name = "test3" },
            };

            var query = assemblies.GetAddPartialAssemblyQuery();
            var error = query.ValidateQuery();

            Assert.IsNull(error, error);
            Assert.AreEqual(3, ((IList)query.Parameters.First().Value).Count);
        }

        [TestMethod]
        public void TestGetAddReferenceQuery()
        {
            var assemblies = new[]
            {
                new Assembly { Name = "test", AssembliesReferenced = new List<string> { "test2", "test3" } },
                new Assembly { Name = "test2", AssembliesReferenced = new List<string> { "test2", "test3" } }
            };

            var query = assemblies.GetAddReferenceQuery();
            var error = query.ValidateQuery();

            Assert.IsNull(error, error);
            Assert.AreEqual(4, ((IList)query.Parameters.First().Value).Count);
        }
    }
}
