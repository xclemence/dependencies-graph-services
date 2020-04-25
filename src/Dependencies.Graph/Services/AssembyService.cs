using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dependencies.Graph.Extensions;
using Dependencies.Graph.Models;
using Dependencies.Graph.Queries;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;

namespace Dependencies.Graph.Services
{
    public class AssemblyService
    {
        private readonly GraphDriverService service;
        private readonly ILogger<AssemblyService> logger;

        public AssemblyService(GraphDriverService service, ILogger<AssemblyService> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        public async Task AddAsync(IEnumerable<Assembly> assemblies)
        {
            var driver = service.GetDriver();
            var session = driver.AsyncSession();
            
            try
            {
                await session.WriteTransactionAsync(async x =>
                {
                    await x.RunAsync(assemblies.GetAddSoftwareQuery());
                    await x.RunAsync(assemblies.GetAddFullAssemblyQuery());
                    await x.RunAsync(assemblies.GetAddPartialAssemblyQuery());

                    await x.RunAsync(assemblies.GetAddReferenceQuery());
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<IList<Assembly>> SearchAsync(string name)
        {
            var driver = service.GetDriver();
            var session = driver.AsyncSession();
            try
            {
                return await session.WriteTransactionAsync(async x =>
                {
                    var (query, resultExtractor) = name.GetSearchAssembliesQuery();
                    var restult = await x.RunAsync(query);
                    
                    var graphModels = await restult.ToListAsync(x => resultExtractor(x));

                    return graphModels.Select(x => x.ToAssembly()).ToList();
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<IList<Assembly>> GetAsync(string assemblyName)
        {
            var driver = service.GetDriver();
            var session = driver.AsyncSession();

            try
            {
                return await session.WriteTransactionAsync(async x =>
                {
                    var (query, resultExtractor) = assemblyName.GetFullAssemblyQuery();

                    var result = await x.RunAsync(query);
                    var graphModels = await result.SingleAsync(x => resultExtractor(x));

                    return graphModels.ToAssembly().ToList();
                });
            }
            finally
            {
                await session.CloseAsync();
            }
        }

    }
}
