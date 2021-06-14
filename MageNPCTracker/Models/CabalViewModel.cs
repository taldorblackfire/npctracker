using System.Collections.Generic;
namespace MageNPCTracker.Models
{
    public class CabalViewModel
    {
        public List<Npcartifact> Artifacts { get; set; } = new List<Npcartifact>();

        public MageCabal CabalInfo { get; set; }

        public List<Npctable> CabalMembers { get; set; }

        public List<Npcimbued> ImbuedItems { get; set; } = new List<Npcimbued>();
    }
}
