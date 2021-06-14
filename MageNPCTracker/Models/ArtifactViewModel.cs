using System.Collections.Generic;
namespace MageNPCTracker.Models
{
    public class ArtifactViewModel
    {
        public ArtifactTable ArtifactInfo { get; set; } = new ArtifactTable();

        public List<ArtifactAttainment> Attainments { get; set; } = new List<ArtifactAttainment>();
    }
}
