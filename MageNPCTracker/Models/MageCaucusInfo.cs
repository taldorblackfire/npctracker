using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class MageCaucusInfo
    {
        public MageCaucusInfo()
        {
            MageNpctable = new HashSet<MageNpctable>();
        }

        public int MageCaucusId { get; set; }
        public int MageOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public virtual MageOrder MageOrderNavigation { get; set; }
        public virtual ICollection<MageNpctable> MageNpctable { get; set; }
    }
}
