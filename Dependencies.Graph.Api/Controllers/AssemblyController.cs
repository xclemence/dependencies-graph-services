using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dependencies.Graph.Dtos;
using Dependencies.Graph.Models;
using Dependencies.Graph.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dependencies.Graph.Api.Controllers
{
    [Route("api/assembly")]
    [ApiController]
    public class AssemblyController : ControllerBase
    {
        private readonly AssemblyService assemblyService;
        private readonly ILogger<AssemblyController> logger;
        private readonly MapperConfiguration mapperConfig;

        public AssemblyController(AssemblyService assemblyService, MapperConfiguration config, ILogger<AssemblyController> logger)
        {
            this.assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            mapperConfig = config;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddAsyc([FromBody] IList<AssemblyDto> dto)
        {
            var mapper = mapperConfig.CreateMapper();
            var models = dto.Select(x => mapper.Map<Assembly>(x));

            await assemblyService.AddAsync(models);

            return Ok();
        }

        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchAsync(string name)
        {
            var results = await assemblyService.SearchAsync(name);
            var mapper = mapperConfig.CreateMapper();

            var dtos = results.Select(x => mapper.Map<AssemblyDto>(x));

            return Ok(dtos);
        }

        [HttpGet("{assemblyName}")]
        public async Task<IActionResult> GetAsync(string assemblyName)
        {
            var results = await assemblyService.GetAsync(assemblyName);
            var mapper = mapperConfig.CreateMapper();

            var dtos = results.Select(x => mapper.Map<AssemblyDto>(x));

            return Ok(dtos);
        }
    }
}
