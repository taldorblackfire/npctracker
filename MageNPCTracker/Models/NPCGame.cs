using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class Npcgame
    {
        public Npcgame()
        {
            MageCabal = new HashSet<MageCabal>();
            Npctable = new HashSet<Npctable>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MageCabal> MageCabal { get; set; }
        public virtual ICollection<Npctable> Npctable { get; set; }
    }
}
