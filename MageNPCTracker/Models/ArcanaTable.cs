using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class ArcanaTable
    {
        public ArcanaTable()
        {
            MageNpcarcana = new HashSet<MageNpcarcana>();
            RefLegacyRulingArcanaNavigation = new HashSet<RefLegacy>();
            RefLegacySecondaryArcanaNavigation = new HashSet<RefLegacy>();
        }

        public int Id { get; set; }
        public string Arcana { get; set; }
        public string Purview { get; set; }

        public virtual ICollection<MageNpcarcana> MageNpcarcana { get; set; }
        public virtual ICollection<RefLegacy> RefLegacyRulingArcanaNavigation { get; set; }
        public virtual ICollection<RefLegacy> RefLegacySecondaryArcanaNavigation { get; set; }
    }
}
