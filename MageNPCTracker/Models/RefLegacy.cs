using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class RefLegacy
    {
        public RefLegacy()
        {
            MageNpctable = new HashSet<MageNpctable>();
        }

        public int LegacyId { get; set; }
        public int? PathId { get; set; }
        public int? OrderId { get; set; }
        public int? SecondOrderId { get; set; }
        public int RulingArcana { get; set; }
        public int? SecondaryArcana { get; set; }
        public string LegacyName { get; set; }
        public string Description { get; set; }
        public bool LeftHanded { get; set; }

        public virtual MageOrder Order { get; set; }
        public virtual MagePath Path { get; set; }
        public virtual ArcanaTable RulingArcanaNavigation { get; set; }
        public virtual MageOrder SecondOrder { get; set; }
        public virtual ArcanaTable SecondaryArcanaNavigation { get; set; }
        public virtual ICollection<MageNpctable> MageNpctable { get; set; }
    }
}
