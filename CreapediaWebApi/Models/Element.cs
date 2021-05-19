using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Element
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public DateTime? Lastupdate { get; set; }
        public int? Chatacteristicscount { get; set; }
        public int? Parentfolderid { get; set; }

        public virtual Folder Parentfolder { get; set; }
    }
}
