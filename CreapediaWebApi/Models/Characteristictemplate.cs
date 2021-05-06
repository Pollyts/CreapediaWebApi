using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Characteristictemplate
    {
        public Characteristictemplate()
        {
            Characteristics = new HashSet<Characteristic>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? TemplateelementId { get; set; }

        public virtual Templateelement Templateelement { get; set; }
        public virtual ICollection<Characteristic> Characteristics { get; set; }
    }
}
