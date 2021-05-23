using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class User
    {
        public User()
        {
            Folders = new HashSet<Folder>();
            Templatefolders = new HashSet<Templatefolder>();
        }

        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool? Mailconfirm { get; set; }

        public virtual ICollection<Folder> Folders { get; set; }
        public virtual ICollection<Templatefolder> Templatefolders { get; set; }
    }
}
