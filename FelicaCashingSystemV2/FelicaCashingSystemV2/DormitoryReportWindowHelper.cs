using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using RobotClub.DormitoryReport;
using WpfCommonds;

namespace FelicaCashingSystemV2
{
    /// <summary>
    /// 門限超過届けを表示するためのヘルパークラス
    /// </summary>
    static class DormitoryReportWindowHelper
    {
        private static MainForm form = null;

        public static void ShowDormitoryReportWindow(this Window window)
        {
            var data = new DormitoryReportData();
            data.Name = "宮永 咲";

            data.RoomNo = "A150";
            data.PhoneNumber = "090-9999-9999";
            data.LeaderName = "竹井 久";
            data.LeaderPhoneNumber = "090-0000-0000";
            data.Reason = "麻雀部の活動のため";

            if (form == null)
            {
                form = DormitoryReport.CreateWindow();
            }

            form.Icon = WindowIcon.GetIcon();

            form.Show(data);
            form.ShowDialog();
        }
    }
}
