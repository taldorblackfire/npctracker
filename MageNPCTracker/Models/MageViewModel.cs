using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MageNPCTracker.Models
{
    public class MageViewModel
    {
        public List<Npctable> Apprentices { get; set; } = new List<Npctable>();

        public List<Npcartifact> ArtifactList { get; set; } = new List<Npcartifact>();

        public List<Npcimbued> ImbuedList { get; set; } = new List<Npcimbued>();

        public MageNpctable MageInfo { get; set; }

        [EnsureArcanaElement(ErrorMessage = "You must have at least three Arcana Specified to create a Mage.")]
        public List<MageNpcarcana> MageArcana { get; set; } = new List<MageNpcarcana>();

        public Npctable NpcInfo { get; set; }
    }

    public class EnsureArcanaElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IList;
            if (list != null)
            {
                return list.Count > 2;
            }
            return false;
        }
    }
}
