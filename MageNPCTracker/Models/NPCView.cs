using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MageNPCTracker.Models
{
    public class NPCView
    {
        public int NPCId { get; set; }

        public int Gnosis { get; set; }

        public string Arcana { get; set; }

        public int Level { get; set; }

        public bool Alive { get; set; }

        public int? CabalId { get; set; }

        public int? OrderStatusId { get; set; }

        public int? ConsiliumStatusId { get; set; }

        public int? LegacyId { get; set; }

        public string Legacy { get; set; }

        public string ConsiliumTitle { get; set; }

        public string OrderTitle { get; set; }

        public string Order { get; set; }

        public string Cabal { get; set; }

        public string ShadowName { get; set; }

        public string Notes { get; set; }

        public int GameId { get; set; }
    }
}
