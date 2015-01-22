using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using RobotClub.DormitoryReport;
using WpfCommonds;
using System.Diagnostics;

namespace FelicaCashingSystemV2
{
    /// <summary>
    /// 門限超過届けを表示するためのヘルパークラス
    /// </summary>
    static class DormitoryReportWindowHelper
    {
        private static MainForm form = null;

        public static bool ShowDormitoryReportWindow(this Window window)
        {
            var data = new DormitoryReportData();
            var user = App.Current.User;
            if (user == null) { return false; }

            data.Name = user.Name;
            data.RoomNo = user.DormitoryRoomNumber;
            data.PhoneNumber = user.PhoneNumber;

            try
            {
                var texts = App.Current.Collections.UiTexts;
                data.LeaderName = texts.GetText(FelicaData.UiTextType.LeaderName);
                data.LeaderPhoneNumber = texts.GetText(FelicaData.UiTextType.LeaderPhoneNumber);
                data.Reason = texts.GetText(FelicaData.UiTextType.DormitoryReportReason);
            }
            catch (FelicaData.DatabaseException e)
            {
                Debug.WriteLine(e);
                return false;
            }

            if (form == null)
            {
                form = DormitoryReport.CreateWindow();
            }

            form.Icon = WindowIcon.GetIcon();

            form.Show(data);
            form.ShowDialog();

            return true;
        }
    }
}
