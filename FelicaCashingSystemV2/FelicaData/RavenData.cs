using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Embedded;

namespace FelicaData
{
    abstract class RavenData
    {
        protected IDocumentStore DocumentStore { get; set; }

        protected RavenData(string connectionStringName)
        {
            this.DocumentStore = new EmbeddableDocumentStore
            {
                ConnectionStringName = connectionStringName
            };

            this.DocumentStore.Initialize();
        }

        protected void Create<T>(T data)
            where T : RavenModel
        {
            data.Id = 0;
            this.Store(data);
        }

        protected void Update<T>(T data)
            where T : RavenModel
        {
            this.Store(data);
        }

        private void Store<T>(T data)
            where T : class
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                session.Store(data);
                session.SaveChanges();
            }

            this.OnChanged();
        }

        protected List<T> Query<T>()
            where T : class
        {
            return this.Query<T>(x => true);
        }

        protected List<T> Query<T>(Func<T, bool> predicate)
            where T : class
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                return session.Query<T>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite()) // 書き込みを待機
                    .Where(predicate)
                    .ToList();
            }
        }

        #region Changed Event

        public EventHandler Changed;

        protected void OnChanged()
        {
            if (this.Changed != null)
            {
                this.Changed(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}
