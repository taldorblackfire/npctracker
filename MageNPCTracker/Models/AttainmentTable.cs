using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace MageNPCTracker.Models
{
    public partial class AttainmentTable
    {
        public AttainmentTable()
        {
            ArtifactAttainment = new HashSet<ArtifactAttainment>();
        }

        public int Id { get; set; }
        [Display(Name = "Attainment")]
        public string attainment_name { get; set; }
        [Display(Name = "Arcana")]
        public string attainment_arcana { get; set; }
        [Display(Name = "Dot Rating")]
        public short attainment_level { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }
        public bool ItemUsable { get; set; }

        public virtual ICollection<ArtifactAttainment> ArtifactAttainment { get; set; }
    }
}
