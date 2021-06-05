using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Element
    {
        public Element()
        {
            Characteristics = new HashSet<Characteristic>();
            Elementlinks = new HashSet<Elementlink>();
            RelationFirstelements = new HashSet<Relation>();
            RelationSecondelements = new HashSet<Relation>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public int? Parentfolderid { get; set; }
        public bool Ifpubic { get; set; }

        public virtual Folder Parentfolder { get; set; }
        public virtual ICollection<Characteristic> Characteristics { get; set; }
        public virtual ICollection<Elementlink> Elementlinks { get; set; }
        public virtual ICollection<Relation> RelationFirstelements { get; set; }
        public virtual ICollection<Relation> RelationSecondelements { get; set; }
    }
}
