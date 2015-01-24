using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    class ProfileEditWindowViewModel : MetroWindowViewModelBase
    {
        private FelicaData.User user = App.Current.User.Clone();

        public ProfileEditWindowViewModel()
        {
            this.SaveBasicProfileCommand = new DelegateCommand<PasswordBox[]>(this.SaveBasicProfile);
            this.SelectFileCommand = new DelegateCommand(this.SelectFile);
            this.SaveAvatarCommand = new DelegateCommand(this.SaveAvatar);
        }

        public string Name
        {
            get { return this.user.Name; }
            set
            {
                this.user.Name = value;
                this.OnPropertyChanged("Name");
            }
        }

        public string Email
        {
            get { return this.user.Email; }
            set
            {
                this.user.Email = value;
                this.OnPropertyChanged("Email");
            }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }

        public ImageSource AvatarSource
        {
            get
            {
                if (this.user.Avatar != null)
                {
                    var bitmap = this.user.Avatar.ToBitmap();
                    if (bitmap != null)
                    {
                        return bitmap.ToBitmapSource();
                    }
                }

                return null;
            }
        }

        private Bitmap newAvatar = null;
        private Bitmap NewAvatar
        {
            get { return this.newAvatar; }
            set
            {
                this.newAvatar = value;
                this.OnPropertyChanged("NewAvatarSource");
            }
        }

        public ImageSource NewAvatarSource
        {
            get
            {
                if (this.newAvatar == null) { return null; }
                return this.newAvatar.ToBitmapSource();
            }
        }

        public ICommand SaveBasicProfileCommand { get; private set; }
        private void SaveBasicProfile(PasswordBox[] passwordBoxes)
        {
            this.ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.Name))
            {
                this.ErrorMessage = "名前を正しく入力してください。";
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Email))
            {
                this.ErrorMessage = "メールアドレスを入力してください。";
                return;
            }

            if (!FelicaMail.MailAddress.IsValidEmail(this.Email))
            {
                this.ErrorMessage = "メールアドレスを正しく入力してください。";
                return;
            }

            var password = passwordBoxes[0].Password;

            // 空白のみのパスワードははじく
            // パスワードが空欄な場合は、パスワード以外変更扱いなので、完全に空欄の場合は通すこと
            if (!string.IsNullOrEmpty(password) && string.IsNullOrWhiteSpace(password))
            {
                this.ErrorMessage = "パスワードは空白のみにはできません。";
                passwordBoxes.ClearAndFocus();
                return;
            }

            // パスワードが一致しない場合
            if (password != passwordBoxes[1].Password)
            {
                this.ErrorMessage = "パスワードが一致しません。";
                passwordBoxes.ClearAndFocus();
                return;
            }

            // パスワードを変更する場合 (空白のみ以外の入力が存在する場合)
            if (!string.IsNullOrWhiteSpace(password))
            {
                this.user.PlainPassword = password;
            }

            try
            {
                App.Current.Collections.Users.UpdateUser(this.user);
            }
            catch (FelicaData.DatabaseException e)
            {
                this.ErrorMessage = e.Message;
                return;
            }

            App.Current.UpdateUser();
            
            this.ShowMessageBox("プロフィールの基本情報を変更しました。", "変更完了");
            passwordBoxes.Clear();
        }

        public ICommand SelectFileCommand { get; private set; }
        private void SelectFile()
        {
            this.ErrorMessage = string.Empty;

            var filter = Properties.Settings.Default.ImageFileFilter; // 画像ファイル
            var result = this.ShowOpenFileDialog(filter);

            // キャンセルされた場合
            if (result == null) { return; }

            try
            {
                using (var stream = new FileStream(result.FileName, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        using (var bitmap = new Bitmap(stream))
                        {
                            var resizedImage = BitmapSizeResizer.ResizeSquared(bitmap, Properties.Settings.Default.AvatarSize);
                            this.NewAvatar = resizedImage;
                        }
                    }
                    catch (ArgumentException e)
                    {
                        Debug.WriteLine(e.Message);
                        this.ErrorMessage = "対応していない画像形式です。";
                        return;
                    }
                }
            }

            catch (Exception)
            {
                this.ErrorMessage = "ファイルの読み込みに失敗しました。";
                return;
            }
            
        }

        /// <summary>
        /// 設定したアバターを保存するコマンドです。
        /// </summary>
        public ICommand SaveAvatarCommand { get; private set; }
        private void SaveAvatar()
        {
            this.ErrorMessage = string.Empty;

            if (this.NewAvatar == null)
            {
                this.ErrorMessage = "新しいアバターを指定してください。";
                return;
            }

            var oldAvatar = this.user.Avatar != null ? (byte[])this.user.Avatar.Clone() : null;
            this.user.Avatar = this.NewAvatar.ToBytes();
            
            try
            {
                App.Current.Collections.Users.UpdateUser(this.user);
            }
            catch (FelicaData.DatabaseException e)
            {
                this.ErrorMessage = e.Message;

                this.user.Avatar = oldAvatar;
                this.OnPropertyChanged("AvatarSource");
                return;
            }

            App.Current.UpdateUser();
            this.OnPropertyChanged("AvatarSource");

            this.ShowMessageBox("アバターを更新しました。", "保存成功");
            this.NewAvatar = null;
        }
    }
}
