using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCommonds
{
    public class OpenFileDialogMessage : MessageBase, IOpenFileDialogResult
    {
        public string Filter { get; set; }

        public string FileName { get; set; }
        public bool UserClickedOk { get; set; }
    }
}
