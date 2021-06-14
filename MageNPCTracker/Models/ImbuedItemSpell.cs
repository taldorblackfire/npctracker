using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class ImbuedItemSpell
    {
        public int ImbuedItemSpellId { get; set; }
        public int ImbuedItemId { get; set; }
        public int SpellId { get; set; }

        public virtual ImbuedTable ImbuedTable { get; set; }
        public virtual SpellTable SpellTable { get; set; }
    }
}
