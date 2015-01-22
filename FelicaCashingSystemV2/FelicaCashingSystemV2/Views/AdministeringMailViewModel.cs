using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class MailTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    class AdministeringMailViewModel : AdministeringUserViewModel
    {
        public AdministeringMailViewModel()
        {
            this.Templates.Add(new MailTemplate
            {
                Id = "PaymentRequest",
                Name = "支払い要求"
            });
            
            this.Templates.Add(new MailTemplate
            {
                Id = "Message",
                Name = "任意のメッセージ"
            });

            this.TemplateId = "PaymentRequest";
            this.SendCommand = new DelegateCommand(this.Send);
        }

        private ObservableCollection<MailTemplate> templates = new ObservableCollection<MailTemplate>();
        public ObservableCollection<MailTemplate> Templates
        {
            get { return this.templates; }
            set
            {
                this.templates = value;
                this.OnPropertyChanged("Templates");
            }
        }

        private string templateId = null;
        public string TemplateId
        {
            get {
                if (this.templateId == null)
                {
                    if (this.Templates.Count > 0)
                    {
                        this.templateId = this.Templates[0].Id;
                    }
                }

                return this.templateId;
            }
            set
            {
                this.templateId = value;
                this.OnPropertyChanged("TemplateId");
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

        public ICommand SendCommand { set; get; }
        private void Send()
        {
            Debug.WriteLine("Send mail start");



            Debug.WriteLine("Send mail succeeded");
        }
    }
}
