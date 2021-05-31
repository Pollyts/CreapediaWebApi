using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Folder
    {
        public Folder()
        {
            Elements = new HashSet<Element>();
            InverseParentfolder = new HashSet<Folder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Lastupdate { get; set; }
        public int? Folderscount { get; set; }
        public int? Elementscount { get; set; }
        public int? Parentfolderid { get; set; }
        public int Userid { get; set; }

        public virtual Folder Parentfolder { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
        public virtual ICollection<Folder> InverseParentfolder { get; set; }
    }
}
