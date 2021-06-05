using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Templateelement
    {
        public Templateelement()
        {
            Elementlinks = new HashSet<Elementlink>();
            Templatecharacteristics = new HashSet<Templatecharacteristic>();
            TemplatelinkChildelements = new HashSet<Templatelink>();
            TemplatelinkParenttelements = new HashSet<Templatelink>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Parentfolderid { get; set; }
        public bool Ifpubic { get; set; }

        public virtual Templatefolder Parentfolder { get; set; }
        public virtual ICollection<Elementlink> Elementlinks { get; set; }
        public virtual ICollection<Templatecharacteristic> Templatecharacteristics { get; set; }
        public virtual ICollection<Templatelink> TemplatelinkChildelements { get; set; }
        public virtual ICollection<Templatelink> TemplatelinkParenttelements { get; set; }
    }
}
