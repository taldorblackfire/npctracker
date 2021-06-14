using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MageNPCTracker.Models
{
    public partial class ArtifactTable
    {
        public ArtifactTable()
        {
            Npcartifact = new HashSet<Npcartifact>();
            ArtifactAttainment = new HashSet<ArtifactAttainment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Combined Spell Rating")]
        public short Reach { get; set; }
        public string Description { get; set; }
        [Display(Name = "Imperial Surcharge")]
        public bool ImperialSurcharge { get; set; }
        [Display(Name = "Yantra Bonus")]
        public bool YantraBonus { get; set; }
        [Display(Name = "Mana Capacity")]
        public short Mana { get; set; }
        public string Path { get; set; }
        [Display(Name = "Merit Dots")]
        public short Cost { get; set; }
        public short Gnosis { get; set; }
        public int? CharacterId { get; set; }

        public virtual ICollection<Npcartifact> Npcartifact { get; set; }
        public virtual ICollection<ArtifactAttainment> ArtifactAttainment { get; set; }
    }
}
