using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Characteristic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? Elementid { get; set; }

        public virtual Element Element { get; set; }
    }
}
