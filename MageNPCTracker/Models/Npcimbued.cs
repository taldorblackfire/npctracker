using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class Npcimbued
    {
        public int NpcimbuedId { get; set; }
        public int Npcid { get; set; }
        public int ImbuedId { get; set; }

        public virtual ImbuedTable Imbued { get; set; }
        public virtual Npctable Npc { get; set; }
    }
}
