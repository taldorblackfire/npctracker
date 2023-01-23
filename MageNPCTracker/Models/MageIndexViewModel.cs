using System.Collections.Generic;
namespace MageNPCTracker.Models
{
    public class MageIndexViewModel
    {
        public List<MageInfo> MageInfo = new List<MageInfo>();
    }

    public class MageInfo
    {
        public int NPCId { get; set; }

        public string ShadowName { get; set; }

        public bool Alive { get; set; }

        public string Cabal { get; set; }

        public string Notes { get; set; }

        public int? CabalId { get; set; }

        public int? OrderStatusId { get; set; }

        public int? ConsiliumStatusId { get; set; }

        public int? LegacyId { get; set; }

        public string ConsiliumTitle { get; set; }

        public string OrderTitle { get; set; }

        public string Order { get; set; }

        public string Legacy { get; set; }

        public int Gnosis { get; set; }

        public string Arcana { get; set; }
    }
}
