using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreapediaWebApi.Models
{
    public class FullCharacteristic{
        public FullCharacteristic()
        {

        }
        public int IdParent { get; set; }
        public string NameParent { get; set; }
        public int IdCharacter { get; set; }
        public string NameCharacter { get; set; }
        public string ValueCharacter { get; set; }
    }
    public class MainComponent
    {
        public MainComponent()
        {

        }
        public int Id { get; set; }
        public int Userid { get; set; }
        public string Name { get; set; }
    }
    public class ClientClasses
    {
    }
}
