using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class MageApprenticeTable
    {
        public int ApprenticeNpcid { get; set; }
        public int Npcid { get; set; }
        public int MentorId { get; set; }

        public virtual Npctable Mentor { get; set; }
        public virtual Npctable Npc { get; set; }
    }
}
