using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class MageNpcarcana
    {
        public int MageNpcarcanaId { get; set; }
        public int Npcid { get; set; }
        public int ArcanaId { get; set; }
        public int Level { get; set; }

        public virtual ArcanaTable Arcana { get; set; }
        public virtual Npctable Npc { get; set; }
    }
}
