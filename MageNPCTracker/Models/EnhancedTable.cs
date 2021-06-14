using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class EnhancedTable
    {
        public EnhancedTable()
        {
            ItemEnhancement = new HashSet<ItemEnhancement>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ItemEnhancement> ItemEnhancement { get; set; }
    }
}
