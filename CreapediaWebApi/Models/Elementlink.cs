using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Elementlink
    {
        public int Id { get; set; }
        public int Parenttelementid { get; set; }
        public int Childelementid { get; set; }

        public virtual Element Childelement { get; set; }
        public virtual Templateelement Parenttelement { get; set; }
    }
}
