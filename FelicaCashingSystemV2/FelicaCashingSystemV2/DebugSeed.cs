using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2
{
    static class DebugSeed
    {
        public static void Init()
        {
            try
            {
                App.Current.Collections.Users.CreateUser(new FelicaData.User
                {
                    Name = "テスト用ユーザー",
                    Email = "tester@tester.jp",
                    IsAdmin = true,
                    PlainPassword = "tester"
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
                    PlainPassword = "tester"
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
    }
}
