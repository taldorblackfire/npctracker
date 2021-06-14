using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MageNPCTracker.Models
{
    public partial class ImbuedTable
    {
        public ImbuedTable()
        {
            ItemEnhancement = new HashSet<ItemEnhancement>();
            Npcimbued = new HashSet<Npcimbued>();
            ImbuedItemSpell = new HashSet<ImbuedItemSpell>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Highest Spell Level")]
        public short SpellLevel { get; set; }
        [Display(Name = "Mana Battery Dots")]
        public short? BatteryDots { get; set; }
        public string Description { get; set; }
        public int? CharacterId { get; set; }
        public short Mana { get; set; }
        [Display(Name = "Current Mana")]
        public int? CurrentCharge { get; set; }
        public short Cost { get; set; }

        public virtual ICollection<ItemEnhancement> ItemEnhancement { get; set; }
        public virtual ICollection<Npcimbued> Npcimbued { get; set; }
        public virtual ICollection<ImbuedItemSpell> ImbuedItemSpell { get; set; }
    }
}
