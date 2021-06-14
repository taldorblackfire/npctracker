using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MageNPCTracker.Models
{
    public partial class SpellTable
    {
        public SpellTable()
        {
            ItemEnhancement = new HashSet<ItemEnhancement>();
            SpellArcanaTable = new HashSet<SpellArcanaTable>();
            ImbuedItemSpell = new HashSet<ImbuedItemSpell>();
        }

        [Display(Name = "Spell Name")]
        public string SpellName { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        [Display(Name = "Primary Factor")]
        public string PrimaryFactor { get; set; }

        public virtual ICollection<ItemEnhancement> ItemEnhancement { get; set; }
        public virtual ICollection<SpellArcanaTable> SpellArcanaTable { get; set; }
        public virtual ICollection<ImbuedItemSpell> ImbuedItemSpell { get; set; }
    }
}
