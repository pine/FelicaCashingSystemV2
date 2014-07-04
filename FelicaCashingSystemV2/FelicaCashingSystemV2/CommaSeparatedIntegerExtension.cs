using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2
{
    public static class CommaSeparatedIntegerExtension
    {
        public static string ToCommaString(this int val)
        {
            // マイナス符号は勝手に付く
            return (val > 0 ? "+" : "") + string.Format("{0:#,0}", val);
        }

        public static string ToCommaStringAbs(this int val)
        {
            return string.Format("{0:#,0}", Math.Abs(val));
        }
    }
}
