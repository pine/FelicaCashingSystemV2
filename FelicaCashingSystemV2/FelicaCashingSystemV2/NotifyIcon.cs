using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FelicaCashingSystemV2
{
    class NotifyIcon : IDisposable
    {
        public event EventHandler<System.Windows.Forms.MouseEventArgs> Click;
        public event EventHandler ExitClick;

        private System.Windows.Forms.NotifyIcon notifyIcon = null;
        private System.Windows.Forms.ContextMenuStrip menuStrip = null;
        private System.Windows.Forms.ToolStripMenuItem exitItem = null;

        public NotifyIcon(Uri iconUri)
        {
            using (Stream iconStream = Application.GetResourceStream(iconUri).Stream)
            {
                System.Drawing.Icon icon = new System.Drawing.Icon(iconStream);
                this.Initialize(icon);
            }
        }

        public NotifyIcon(System.Drawing.Icon icon)
        {
            this.Initialize(icon);
        }

        private void Initialize(System.Drawing.Icon icon)
        {
            this.notifyIcon = new System.Windows.Forms.NotifyIcon();

            this.notifyIcon.Text = SystemInformation.AppName;
            this.notifyIcon.Icon = icon;
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += this.notifyIcon_MouseClick;
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip();

            this.exitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitItem.Text = "終了 (&X)";
            this.exitItem.Click += this.exitItem_Click;
            this.menuStrip.Items.Add(this.exitItem);
            
            this.notifyIcon.ContextMenuStrip = this.menuStrip;
        }

        protected virtual void OnClick(System.Windows.Forms.MouseEventArgs e)
        {
            if (this.Click != null)
            {
                this.Click(this, e);
            }
        }

        protected virtual void OnExitClick(EventArgs e)
        {
            if (this.ExitClick != null)
            {
                this.ExitClick(this, e);
            }
        }

        private void exitItem_Click(object sender, EventArgs e)
        {
            this.OnExitClick(e);
        }

        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.OnClick(e);
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
        ~NotifyIcon()
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
                this.exitItem.Dispose();
                this.menuStrip.Dispose();
                this.notifyIcon.Visible = false;
                this.notifyIcon.Dispose();
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
