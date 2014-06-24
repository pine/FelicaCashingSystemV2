using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public class Card : RavenModel
    {
        public string Uid { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
    }
}
