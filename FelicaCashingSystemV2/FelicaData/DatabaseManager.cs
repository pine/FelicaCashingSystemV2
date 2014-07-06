using Raven.Client;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public class DatabaseManager : IDisposable
    {
        internal IDocumentStore DocumentStore { get; set; }

        /// <summary>
        /// データベースを初期化します。
        /// </summary>
        /// <param name="connectionStringName"></param>
        /// <exception cref="FelicaData.DatabaseException">データベースの初期化に失敗した場合</exception>
        public DatabaseManager(string connectionStringName)
        {
            this.DocumentStore = new EmbeddableDocumentStore
            {
                ConnectionStringName = connectionStringName
            };

            try
            {
                this.DocumentStore.Initialize(); // 初期化は時間が掛かる
            }
            catch (InvalidOperationException e)
            {
                Debug.WriteLine(e);
                throw new DatabaseException("データベースの初期化に失敗しました。");
            }
        }

        
        #region Dispose Finalize パターン
 
        /// <summary>
        /// 既にDisposeメソッドが呼び出されているかどうかを表します。
        /// </summary>
        private bool disposed = false;
 
        /// <summary>
        /// ConsoleApplication1.DisposableClass1 によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }
 
        /// <summary>
        /// ConsoleApplication1.DisposableClass1 クラスのインスタンスがGCに回収される時に呼び出されます。
        /// </summary>
        ~DatabaseManager()
        {
            this.Dispose(false);
        }
 
        /// <summary>
        /// ConsoleApplication1.DisposableClass1 によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。 </param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }
            this.disposed = true;
 
            if (disposing)
            {
                // マネージ リソースの解放処理をこの位置に記述します。
                this.DocumentStore.Dispose();
            }
            // アンマネージ リソースの解放処理をこの位置に記述します。
        }
 
        /// <summary>
        /// 既にDisposeメソッドが呼び出されている場合、例外をスローします。
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">既にDisposeメソッドが呼び出されています。</exception>
        protected void ThrowExceptionIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
 
        /// <summary>
        /// Dispose Finalize パターンに必要な初期化処理を行います。
        /// </summary>
        private void InitializeDisposeFinalizePattern()
        {
            this.disposed = false;
        }
 
        #endregion
    }
}
