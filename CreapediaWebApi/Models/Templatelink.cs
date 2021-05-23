using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Templatelink
    {
        public int Id { get; set; }
        public int Parenttelementid { get; set; }
        public int Childelementid { get; set; }

        public virtual Templateelement Childelement { get; set; }
        public virtual Templateelement Parenttelement { get; set; }
    }
}
