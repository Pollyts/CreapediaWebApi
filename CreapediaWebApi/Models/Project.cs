using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Project
    {
        public Project()
        {
            Folders = new HashSet<Folder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Folder> Folders { get; set; }
    }
}
