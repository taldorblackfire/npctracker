using System.Collections.Generic;
namespace MageNPCTracker.Models
{
    public class MageIndexViewModel
    {
        public List<MageNpcarcana> MageArcana { get; set; } = new List<MageNpcarcana>();

        public List<MageNpctable> MageInfo { get; set; } = new List<MageNpctable>();

        public List<Npctable> NpcInfo { get; set; } = new List<Npctable>();
    }
}
