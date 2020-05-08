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
                    await x.RunAsync(assemblies.GetAddSoftwareQuery()).ConfigureAwait(false);
                    await x.RunAsync(assemblies.GetAddFullAssemblyQuery()).ConfigureAwait(false);
                    await x.RunAsync(assemblies.GetAddPartialAssemblyQuery()).ConfigureAwait(false);

                    await x.RunAsync(assemblies.GetAddReferenceQuery()).ConfigureAwait(false);
                }).ConfigureAwait(false);
            }
            finally
            {
                await session.CloseAsync().ConfigureAwait(false);
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
                    (var query, var resultExtractor) = name.GetSearchAssembliesQuery();
                    var restult = await x.RunAsync(query).ConfigureAwait(false);

                    var graphModels = await restult.ToListAsync(x => resultExtractor(x)).ConfigureAwait(false);

                    return graphModels.Select(x => x.ToAssembly()).ToList();
                }).ConfigureAwait(false);
            }
            finally
            {
                await session.CloseAsync().ConfigureAwait(false);
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
                    (var query, var resultExtractor) = assemblyName.GetFullAssemblyQuery();

                    var result = await x.RunAsync(query).ConfigureAwait(false);
                    var graphModels = await result.SingleAsync(x => resultExtractor(x)).ConfigureAwait(false);

                    return graphModels.ToAssembly().ToList();
                }).ConfigureAwait(false);
            }
            finally
            {
                await session.CloseAsync().ConfigureAwait(false);
            }
        }

    }
}
