using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class MageOrder
    {
        public MageOrder()
        {
            MageCaucusInfo = new HashSet<MageCaucusInfo>();
            MageNpctable = new HashSet<MageNpctable>();
            RefLegacyOrder = new HashSet<RefLegacy>();
            RefLegacySecondOrder = new HashSet<RefLegacy>();
        }

        public int Id { get; set; }
        public string OrderName { get; set; }
        public int? RoteSpecialty1 { get; set; }
        public int? RoteSpecialty2 { get; set; }
        public int? RoteSpecialty3 { get; set; }

        public virtual ICollection<MageCaucusInfo> MageCaucusInfo { get; set; }
        public virtual ICollection<MageNpctable> MageNpctable { get; set; }
        public virtual ICollection<RefLegacy> RefLegacyOrder { get; set; }
        public virtual ICollection<RefLegacy> RefLegacySecondOrder { get; set; }
    }
}
