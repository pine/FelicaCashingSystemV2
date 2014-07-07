using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Embedded;
using System.Diagnostics;
using Raven.Abstractions.Indexing;

namespace FelicaData
{
    abstract public class RavenData
    {
        protected DatabaseManager DatabaseManager { get; set; }
        protected IDocumentStore DocumentStore
        {
            get { return this.DatabaseManager.DocumentStore; }
        }

        /// <summary>
        /// 変更フラグ
        /// </summary>
        private Dictionary<Type, bool> changedFlags = new Dictionary<Type, bool>();

        /// <summary>
        /// データベースを初期化します。
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <exception cref="FelicaData.DatabaseException">データベースの初期化に失敗した場合</exception>
        protected RavenData(DatabaseManager mgr)
        {
            this.DatabaseManager = mgr;
        }

        protected T Create<T>(T data)
            where T : RavenModel
        {
            data.Id = 0; // アップデートにならないように Id を無効化

            this.Store(data);

            return data;
        }

        protected void Update<T>(T data)
            where T : RavenModel
        {
            if (data.Id == 0) { throw new ArgumentException("Id is 0", "data"); }
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

            this.OnChanged(typeof(T));
        }

        protected List<T> Query<T>()
            where T : RavenModel
        {
            return this.Query<T>(x => true);
        }

        protected List<T> Query<T>(Func<T, bool> predicate)
            where T : RavenModel
        {
            // 変更があった場合、書き込みを待機
            if (this.changedFlags.ContainsKey(typeof(T)) &&
                this.changedFlags[typeof(T)])
            {
                return this.QueryBlocking(predicate);
            }

            else
            {
                return this.QueryNonBlocking(predicate);
            }
        }

        /// <summary>
        /// 書き込みを待機してクエリを実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private List<T> QueryBlocking<T>(Func<T, bool> predicate)
            where T: RavenModel
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                var result = session.Query<T>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite()) // 書き込みを待機
                    .Where(predicate)
                    .OrderBy(x => x.Id)
                    .ToList();

                // 変更を書き込み済み
                this.changedFlags[typeof(T)] = false;

                return result;
            }
        }

        /// <summary>
        /// 書き込みを待機せずクエリを実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private List<T> QueryNonBlocking<T>(Func<T, bool> predicate)
            where T: RavenModel
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                return session.Query<T>()
                    .Where(predicate)
                    .OrderBy(x => x.Id)
                    .ToList();
            }
        }

        protected void Delete<T>(T data)
            where T: RavenModel
        {
            if (data == null) { return; }

            bool isDeleted = false;
            using (var session = this.DocumentStore.OpenSession())
            {
                var entity = session.Load<T>(data.Id);

                if (entity != null)
                {
                    session.Delete<T>(entity);
                    session.SaveChanges();
                }
            }

            if (isDeleted)
            {
                this.OnChanged(typeof(T));
            }
        }


        #region Changed Event

        public event EventHandler<Type> Changed;

        protected void OnChanged(Type type)
        {
            this.changedFlags[type] = true;

            if (this.Changed != null)
            {
                this.Changed(this, type);
            }
        }

        #endregion

    }
}
