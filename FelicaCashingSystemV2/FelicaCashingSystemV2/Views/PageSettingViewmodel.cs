using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class PageSettingViewModel : MetroWindowViewModelBase
    {
        public PageSettingViewModel()
        {
            this.SaveCommand = new DelegateCommand(this.Save);
        }

        private void UpdatePageType()
        {
            var page = App.Current.UiData.GetPage(this.pageType);

            if (page != null)
            {
                this.MaxMoney = page.MaxMoney.ToString();
                this.SetMoneyList(page.MoneyTiles);
            }
        }

        private FelicaData.UiPageType pageType = FelicaData.UiPageType.Unknown;
        public FelicaData.UiPageType PageType {
            get { return this.pageType; }
            set {
                this.pageType = value;
                this.OnPropertyChanged("PageType");
                this.UpdatePageType();
            }
        }

        private string maxMoney = "0";
        public string MaxMoney {
            get { return this.maxMoney; }
            set { 
                this.maxMoney = value;
                this.OnPropertyChanged("MaxMoney");
            }
        }

        private string moneyList = string.Empty;
        public string MoneyList
        {
            get { return this.moneyList; }
            set
            {
                this.moneyList = value;
                this.OnPropertyChanged("MoneyList");
            }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage {
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
            this.ErrorMessage = string.Empty;

            int maxMoneyInt = 0;

            try
            {
                maxMoneyInt = Convert.ToInt32(this.MaxMoney);
            }
            catch (Exception)
            {
                this.ErrorMessage = "最大値に入力されている値が不正です。";
                return;
            }

            if (maxMoneyInt < 0)
            {
                this.ErrorMessage = "最大値に負の数は指定できません。";
                return;
            }

            var moneyListInt = new List<int>();

            try
            {
                var moneyListStr = this.MoneyList.Split('\n');

                foreach (var str in moneyListStr)
                {
                    if (!string.IsNullOrWhiteSpace(str))
                    {
                        var val = Convert.ToInt32(str.Trim());

                        if (val <= 0)
                        {
                            this.ErrorMessage = "一覧に、ゼロ以下の数値が入力されています。";
                            return;
                        }

                        if (maxMoneyInt != 0 && val > maxMoneyInt)
                        {
                            this.ErrorMessage = "最大値以上の金額が、一覧で指定されています。";
                            return;
                        }

                        moneyListInt.Add(val);
                    }
                }
            }

            catch (Exception)
            {
                this.ErrorMessage = "金額一覧の値が不正です。半角の数値以外は入力しないでください。";
                return;
            }

            
            // バリデーション通過

            // 更新
            try
            {
                var page = App.Current.UiData.GetPage(this.PageType);
                page.MoneyTiles = moneyListInt.ToArray();
                page.MaxMoney = maxMoneyInt;
                App.Current.UiData.SavePage(page);
            }
            catch (Exception e)
            {
                this.ErrorMessage = e.Message;
                return;
            }

            // 保存成功
            this.ShowMessageBox("設定を保存しました", "保存成功");
                
        }

        private void SetMoneyList(int[] moneyList)
        {
            if (moneyList == null)
            { 
                this.MoneyList = ""; 
            }

            else
            {
                this.MoneyList = string.Join("\n", moneyList);
            }
        }

    }
}
