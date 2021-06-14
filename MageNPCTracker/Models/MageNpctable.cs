using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class MageNpctable
    {
        public int MageNpcid { get; set; }
        public int Npcid { get; set; }
        public int? Order { get; set; }
        public int Path { get; set; }
        public int? Legacy { get; set; }
        public int? OrderStatus { get; set; }
        public int? ConsiliumStatus { get; set; }
        public int? Cabal { get; set; }
        public int? Mentor { get; set; }
        public string SignatureNimbus { get; set; }

        public virtual MageCabal CabalNavigation { get; set; }
        public virtual RefLegacy LegacyNavigation { get; set; }
        public virtual MageOrder OrderNavigation { get; set; }
        public virtual MageCaucusInfo OrderStatusNavigation { get; set; }
        public virtual MageConsiliumTitles ConsiliumStatusNavigation { get; set; }
        public virtual MagePath PathNavigation { get; set; }
    }
}
