using System;
using System.Collections.Generic;

namespace MageNPCTracker.Models
{
    public partial class SpellArcanaTable
    {
        public int Id { get; set; }
        public int SpellTableId { get; set; }
        public string Arcana { get; set; }
        public byte Level { get; set; }

        public virtual SpellTable SpellTable { get; set; }
    }
}
