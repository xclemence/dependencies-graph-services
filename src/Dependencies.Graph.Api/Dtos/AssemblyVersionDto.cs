using System;
using System.Collections.Generic;

namespace Dependencies.Graph.Dto
{
    public class AssemblyVersionDto
    {
        public Guid Id { get; set; }

        public string Version { get; set; }

        public string Creator { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsPartial { get; set; }

        public Guid AssemblyGuid { get; set; }

        public IList<Guid> AssemblyVersionsReferenced { get; set; }
    }
}
