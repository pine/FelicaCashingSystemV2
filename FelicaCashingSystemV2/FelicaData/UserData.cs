using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public partial class UserData : RavenData
    {
        public UserData(DatabaseManager mgr)
            : base(mgr) { }
    }
}
