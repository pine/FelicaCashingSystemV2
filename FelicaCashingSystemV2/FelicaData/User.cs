using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public class User : RavenModel
    {
        public string Name { get; set; }
        public int Money { get; set; }
    }
}
