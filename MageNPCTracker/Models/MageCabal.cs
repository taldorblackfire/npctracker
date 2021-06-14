using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MageNPCTracker.Models
{
    public partial class MageCabal
    {
        public MageCabal()
        {
            MageNpctable = new HashSet<MageNpctable>();
        }

        public int MageCabalid { get; set; }
        [Display(Name="Cabal Name")]
        public string CabalName { get; set; }
        [Display(Name = "Right of Emeritus")]
        public bool RightofEmeritus { get; set; }
        [Display(Name = "Right of Sanctuary")]
        public bool RightofSanctuary { get; set; }
        [Display(Name = "Right of Crossing")]
        public bool RightofCrossing { get; set; }
        [Display(Name = "Right of Nemesis")]
        public bool RightofNemesis { get; set; }
        [Display(Name = "Right of Hospitality")]
        public bool RightofHospitality { get; set; }
        public int? GameId { get; set; }

        public virtual Npcgame Game { get; set; }
        public virtual ICollection<MageNpctable> MageNpctable { get; set; }
    }
}
