using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class MageConsiliumTitles
    {
        public MageConsiliumTitles()
        {
            MageNpctable = new HashSet<MageNpctable>();
        }

        public int ConsiliumTitleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? SmallEssential { get; set; }
        public bool? MediumEssential { get; set; }
        public bool? LargeEssential { get; set; }
        public bool? UniqueTitle { get; set; }
        public int? MaxNumber { get; set; }
        public int? MinGnosis { get; set; }

        public virtual ICollection<MageNpctable> MageNpctable { get; set; }
    }
}
