using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Characteristic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? ElementId { get; set; }
        public int? CharacteristictemplateId { get; set; }

        public virtual Characteristictemplate Characteristictemplate { get; set; }
        public virtual Element Element { get; set; }
    }
}
