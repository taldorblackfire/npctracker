using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class Npcartifact
    {
        public int NpcartifactId { get; set; }
        public int Npcid { get; set; }
        public int ArtifactId { get; set; }

        public virtual ArtifactTable Artifact { get; set; }
        public virtual Npctable Npc { get; set; }
    }
}
