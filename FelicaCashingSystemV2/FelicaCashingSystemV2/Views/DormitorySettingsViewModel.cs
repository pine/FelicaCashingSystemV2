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
        }

//        private string leaderName = Properties.Settings.Default.
        private string LeaderName
        {
            get;
            set;
        }

        public ICommand SaveCommand { get; private set; }

        private void Save()
        {
            
        }
    }
}
