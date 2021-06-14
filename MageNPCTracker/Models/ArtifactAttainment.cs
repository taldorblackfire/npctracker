using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class ArtifactAttainment
    {
        public int Id { get; set; }
        public int AttainmentId { get; set; }
        public int ArtifactId { get; set; }

        public virtual ArtifactTable AtrtifactTable { get; set; }
        public virtual AttainmentTable AttainmentTable { get; set; }
    }
}
