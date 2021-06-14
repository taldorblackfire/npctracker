using System.Collections.Generic;
namespace MageNPCTracker.Models
{
    public class NPCViewModel
    {
        public Npctable NpcInfo { get; set; }

        public List<Npcimbued> ImbuedList { get; set; } = new List<Npcimbued>();

        public List<Npcartifact> ArtifactList { get; set; } = new List<Npcartifact>();
    }
}
