using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2
{
    static class Seed
    {
        /// <summary>
        /// シードを書き込みます。
        /// </summary>
        /// <exception cref="FelicaData.DatabaseException">データベースエラー</exception>
        public static void Init()
        {
#if DEBUG
            Init_Debug();
#else
            Init_Release();
#endif
        }

        private static void Init_Debug()
        {
            var settings = FelicaCashingSystemV2.Properties.Settings.Default;

            try
            {
                App.Current.Collections.Users.CreateUser(new FelicaData.User
                {
                    Name = "テスト用ユーザー",
                    Email = "tester@tester.jp",
                    IsAdmin = true,
                    PlainPassword = settings.InitialDebugPassword
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }

            try
            {
                App.Current.Collections.Users.CreateUser(new FelicaData.User
                {
                    Name = "テスト用ユーザー (非管理者)",
                    Email = "tester2@tester.jp",
                    IsAdmin = false,
                    PlainPassword = settings.InitialDebugPassword
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }

            try
            {
                var adminUser = App.Current.Collections.Users.GetUserByEmail("tester@tester.jp");

                App.Current.Collections.Cards.CreateCard(new FelicaData.Card
                {
                    UserId = adminUser.Id,
                    Name = "最初に登録したカード",
                    PlainUid = "TEST-UID-1"
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }

            try
            {
                var adminUser = App.Current.Collections.Users.GetUserByEmail("tester@tester.jp");

                App.Current.Collections.Cards.CreateCard(new FelicaData.Card
                {
                    UserId = adminUser.Id,
                    Name = "ミールカード",
                    PlainUid = "TEST-UID-2"
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }
        }

        private static void Init_Release()
        {
            var adminUsers = App.Current.Collections.Users.GetAdminUsers();

            // 管理者ユーザーが存在しない場合、初期ユーザーを作成する
            if (adminUsers.Count == 0)
            {
                var settings = FelicaCashingSystemV2.Properties.Settings.Default;

                App.Current.Collections.Users.CreateUser(new FelicaData.User
                {
                    Name = settings.InitialUser,
                    Email = settings.InitialEmail,
                    IsAdmin = true,
                    PlainPassword = settings.InitialPassword
                });
            }
        }
    }
}
