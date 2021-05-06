using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Templateelement
    {
        public Templateelement()
        {
            Characteristictemplates = new HashSet<Characteristictemplate>();
            Elements = new HashSet<Element>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? TemplatefolderId { get; set; }

        public virtual Templatefolder Templatefolder { get; set; }
        public virtual ICollection<Characteristictemplate> Characteristictemplates { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
    }
}
