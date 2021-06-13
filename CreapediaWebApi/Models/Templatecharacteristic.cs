using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Templatecharacteristic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Telementid { get; set; }

        public virtual Templateelement Telement { get; set; }
    }
}
