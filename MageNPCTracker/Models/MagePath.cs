using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class MagePath
    {
        public MagePath()
        {
            MageNpctable = new HashSet<MageNpctable>();
            RefLegacy = new HashSet<RefLegacy>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstRulingArcana { get; set; }
        public string SecondRulingArcana { get; set; }

        public virtual ICollection<MageNpctable> MageNpctable { get; set; }
        public virtual ICollection<RefLegacy> RefLegacy { get; set; }
    }
}
