using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FelicaMail
{
    public class MailTemplate
    {
        public string Path
        {
            get;
            private set;
        }

        public string Template
        {
            get;
            set;
        }

        public MailTemplate()
        {
            this.Template = string.Empty;
        }

        public void Load(string filePath)
        {
            this.Path = filePath;
            this.Template = File.ReadAllText(filePath);
        }

        public string ApplyTemplate(object args)
        {
            return Nustache.Core.Render.StringToString(
                this.Template,
                args
                );
        }
    }
}
