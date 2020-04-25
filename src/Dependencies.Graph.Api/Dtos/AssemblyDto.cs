using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dependencies.Graph.Api.Dtos
{
    [DebuggerDisplay("Name = {Name}")]
    public class AssemblyDto
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool IsNative { get; set; }

        public string Version { get; set; }

        public string Creator { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public bool IsPartial { get; set; }

        public List<string> AssembliesReferenced { get; set; }
    }
}
