using System.Collections.Generic;
namespace MageNPCTracker.Models
{
    public class ImbuedViewModel
    {
        public ImbuedTable ImbuedInfo { get; set; } = new ImbuedTable();

        public List<ItemEnhancement> ItemEnhancement { get; set; } = new List<ItemEnhancement>();

        public List<ImbuedItemSpell> Spells { get; set; } = new List<ImbuedItemSpell>();
    }
}
