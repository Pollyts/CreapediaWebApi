using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class User
    {
        public User()
        {
            Projects = new HashSet<Project>();
            Templatefolders = new HashSet<Templatefolder>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Templatefolder> Templatefolders { get; set; }
    }
}
