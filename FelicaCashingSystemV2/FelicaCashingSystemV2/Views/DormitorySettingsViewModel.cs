using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class DormitorySettingsViewModel : MetroWindowViewModelBase
    {
        public DormitorySettingsViewModel()
        {
            this.SaveCommand = new DelegateCommand(this.Save);

            try
            {
                var texts = App.Current.Collections.UiTexts;
                this.DormitoryReportReason = texts.GetText(FelicaData.UiTextType.DormitoryReportReason);
                this.LeaderName = texts.GetText(FelicaData.UiTextType.LeaderName);
                this.LeaderPhoneNumber = texts.GetText(FelicaData.UiTextType.LeaderPhoneNumber);
            }
            catch (FelicaData.DatabaseException e)
            {
                this.ErrorMessage = e.Message;
            }
        }

        private string leaderName = "";
        public string LeaderName
        {
            get { return this.leaderName; }
            set
            {
                this.leaderName = value;
                this.OnPropertyChanged("LeaderName");
            }
        }

        private string leaderPhoneNumber = "";
        public string LeaderPhoneNumber
        {
            get { return this.leaderPhoneNumber; }
            set
            {
                this.leaderPhoneNumber = value;
                this.OnPropertyChanged("LeaderPhoneNumber");
            }
        }

        private string dormitoryReportReason = "";
        public string DormitoryReportReason
        {
            get { return this.dormitoryReportReason; }
            set
            {
                this.dormitoryReportReason = value;
                this.OnPropertyChanged("DormitoryReportReason");
            }
        }

        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }

        public ICommand SaveCommand { get; private set; }

        private void Save()
        {
            try
            {
                var texts = App.Current.Collections.UiTexts;
                texts.UpdateText(FelicaData.UiTextType.DormitoryReportReason, this.DormitoryReportReason);
                texts.UpdateText(FelicaData.UiTextType.LeaderName, this.LeaderName);
                texts.UpdateText(FelicaData.UiTextType.LeaderPhoneNumber, this.LeaderPhoneNumber);
            }
            catch (FelicaData.DatabaseException e)
            {
                this.ErrorMessage = e.Message;
            }

            this.ShowMessageBox("保存に成功しました。", "保存成功");
        }
    }
}
