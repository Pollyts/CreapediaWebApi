using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Templatefolder
    {
        public Templatefolder()
        {
            Folders = new HashSet<Folder>();
            InverseParentfolder = new HashSet<Templatefolder>();
            Templateelements = new HashSet<Templateelement>();
        }

        public int Id { get; set; }
        public int? Userid { get; set; }
        public string Name { get; set; }
        public int? ParentfolderId { get; set; }

        public virtual Templatefolder Parentfolder { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
        public virtual ICollection<Templatefolder> InverseParentfolder { get; set; }
        public virtual ICollection<Templateelement> Templateelements { get; set; }
    }
}
