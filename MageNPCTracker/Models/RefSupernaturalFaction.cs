using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class RefSupernaturalFaction
    {
        public RefSupernaturalFaction()
        {
            Npctable = new HashSet<Npctable>();
        }

        public int SupernaturalFactionId { get; set; }
        public string Name { get; set; }
        public string SupernaturalTrait { get; set; }

        public virtual ICollection<Npctable> Npctable { get; set; }
    }
}
