using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class RefEnhancement
    {
        public RefEnhancement()
        {
            ItemEnhancement = new HashSet<ItemEnhancement>();
        }

        public int Id { get; set; }
        public string Bonus { get; set; }
        public int Cost { get; set; }

        public virtual ICollection<ItemEnhancement> ItemEnhancement { get; set; }
    }
}
