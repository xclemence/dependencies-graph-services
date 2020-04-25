using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dependencies.Graph.Models
{
    [DebuggerDisplay("Name = {Name}, Version = {Version}")]
    public class Assembly
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool IsNative { get; set; }

        public string Version { get; set; }

        public string Creator { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public bool IsPartial { get; set; }

        public List<string> AssembliesReferenced { get; set; } = new List<string>();
    }
}
