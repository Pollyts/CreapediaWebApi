using System;
using System.Collections.Generic;

#nullable disable

namespace CreapediaWebApi.Models
{
    public partial class Library
    {
        public int Id { get; set; }
        public string Typeofcomponent { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Componentid { get; set; }
    }
}
