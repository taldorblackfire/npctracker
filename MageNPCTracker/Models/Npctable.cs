using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class Npctable
    {
        public Npctable()
        {
            MageApprenticeTableMentor = new HashSet<MageApprenticeTable>();
            MageApprenticeTableNpc = new HashSet<MageApprenticeTable>();
            MageNpcarcana = new HashSet<MageNpcarcana>();
            Npcartifact = new HashSet<Npcartifact>();
            Npcimbued = new HashSet<Npcimbued>();
        }

        public int Npcid { get; set; }
        public string Alias { get; set; }
        public string RealName { get; set; }
        public string Gender { get; set; }
        public int? SupernaturalFaction { get; set; }
        public int? SupernaturalTrait { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public bool Alive { get; set; }
        public int? GameId { get; set; }

        public virtual RefSupernaturalFaction SupernaturalFactionNavigation { get; set; }
        public virtual Npcgame Game { get; set; }
        public virtual ICollection<MageApprenticeTable> MageApprenticeTableMentor { get; set; }
        public virtual ICollection<MageApprenticeTable> MageApprenticeTableNpc { get; set; }
        public virtual ICollection<MageNpcarcana> MageNpcarcana { get; set; }
        public virtual ICollection<Npcartifact> Npcartifact { get; set; }
        public virtual ICollection<Npcimbued> Npcimbued { get; set; }
    }
}
