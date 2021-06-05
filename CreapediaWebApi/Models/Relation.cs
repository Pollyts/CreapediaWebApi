using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Relation
    {
        public int Id { get; set; }
        public int Firstelementid { get; set; }
        public int Secondelementid { get; set; }
        public string Rel1to2 { get; set; }
        public string Rel2to1 { get; set; }

        public virtual Element Firstelement { get; set; }
        public virtual Element Secondelement { get; set; }
    }
}
