using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MageNPCTracker.Models
{
    public partial class ItemEnhancement
    {
        public int Id { get; set; }
        public int? ImbuedItemId { get; set; }
        public int? EnhancedItemId { get; set; }
        [Display(Name = "Item Bonus")]
        public int? EnhancementId { get; set; }
        public string Enhancement { get; set; }
        public int EnhancementCost { get; set; }
        [Display(Name = "Spell")]
        public int? SpellId { get; set; }

        public virtual EnhancedTable EnhancedItem { get; set; }
        public virtual ImbuedTable ImbuedItem { get; set; }
        public virtual SpellTable Spell { get; set; }
        public virtual RefEnhancement RefEnhancement { get; set; }
    }
}
