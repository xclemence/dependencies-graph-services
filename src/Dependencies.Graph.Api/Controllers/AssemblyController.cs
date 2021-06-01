using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dependencies.Graph.Dtos;
using Dependencies.Graph.Models;
using Dependencies.Graph.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dependencies.Graph.Api.Controllers
{
    [Route("api/assembly")]
    [ApiController]
    public class AssemblyController : ControllerBase
    {
        private readonly AssemblyService assemblyService;
        private readonly MapperConfiguration mapperConfig;

        public AssemblyController(AssemblyService assemblyService, MapperConfiguration config)
        {
            this.assemblyService = assemblyService ?? throw new ArgumentNullException(nameof(assemblyService));
            mapperConfig = config;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddAsyc([FromBody] IList<AssemblyDto> dto)
        {
            var mapper = mapperConfig.CreateMapper();
            var models = dto.Select(x => mapper.Map<Assembly>(x));

            await assemblyService.AddAsync(models).ConfigureAwait(false);

            return Ok();
        }

        [Authorize]
        [HttpGet("search/{name}")]
        public async Task<IActionResult> SearchAsync(string name)
        {
            var results = await assemblyService.SearchAsync(name).ConfigureAwait(false);
            var mapper = mapperConfig.CreateMapper();

            var dtos = results.Select(x => mapper.Map<AssemblyDto>(x));

            return Ok(dtos);
        }

        [HttpGet("{assemblyName}")]
        [Authorize]
        public async Task<IActionResult> GetAsync(string assemblyName)
        {
            var results = await assemblyService.GetAsync(assemblyName).ConfigureAwait(false);
            var mapper = mapperConfig.CreateMapper();

            var dtos = results.Select(x => mapper.Map<AssemblyDto>(x));

            return Ok(dtos);
        }
    }
}
