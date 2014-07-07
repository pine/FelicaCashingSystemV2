using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public class UiData : RavenData
    {
        public UiData(DatabaseManager mgr)
            : base(mgr) { }

        public UiPageSettings GetPage(UiPageType type)
        {
            var page = this.Query<UiPageSettings>(x => type == x.Type).FirstOrDefault();

            if (page == null)
            {
                page = new UiPageSettings { Type = type };
                this.Create(page);
            }

            return page;
        }

        public void SavePage(UiPageSettings page)
        {
            this.Update(page);
        }
    }
}
