using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FelicaData
{
    [Serializable]
    public sealed class User : RavenModel
    {
        public User()
        {
            this.Name = null;
            this.Email = null;
            this.Password = null;
            this.Money = 0;
            this.IsAdmin = false;
            this.Avatar = null;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Money { get; set; }
        public bool IsAdmin { get; set; }
        public byte[] Avatar { get; set; }

        /// <summary>
        /// パスワードで認証を行います。
        /// </summary>
        /// <param name="password">パスワード</param>
        /// <returns>認証が通った場合、<value>true</value>を返します。</returns>
        public bool Auth(string password)
        {
            return !string.IsNullOrWhiteSpace(this.Password) &&
                this.Password == password;
        }

        /// <summary>
        /// 同じ情報を持つユーザーの別インスタンスを返します。
        /// </summary>
        /// <returns></returns>
        public User Clone()
        {
            var user = (User)this.MemberwiseClone();

            return user;
        }
    }
}
