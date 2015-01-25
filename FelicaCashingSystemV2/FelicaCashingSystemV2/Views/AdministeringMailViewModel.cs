using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using WpfCommonds;
using FelicaMail;

namespace FelicaCashingSystemV2.Views
{
    class MailTemplate
    {
        public MailTemplateType Id { get; set; }
        public string Name { get; set; }
    }

    enum MailTemplateType
    {
        PaymentRequest,
        Message
    }

    class AdministeringMailViewModel : AdministeringUserViewModel
    {
        public AdministeringMailViewModel()
        {
            this.Templates.Add(new MailTemplate
            {
                Id = MailTemplateType.PaymentRequest,
                Name = "支払い要求"
            });
            
            this.Templates.Add(new MailTemplate
            {
                Id = MailTemplateType.Message,
                Name = "任意のメッセージ"
            });

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

        private MailTemplateType templateId = MailTemplateType.PaymentRequest;
        public MailTemplateType TemplateId
        {
            get {
                return this.templateId;
            }
            set
            {
                this.templateId = value;
                this.OnPropertyChanged("TemplateId");
                this.OnPropertyChanged("IsMessageRequired");
            }
        }

        public bool IsMessageRequired
        {
            get
            {
                return this.TemplateId == MailTemplateType.Message;
            }
        }

        private string message = "";
        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
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

            this.ErrorMessage = string.Empty;

            var email = this.AdministeringUser.Email;
            var name = this.AdministeringUser.Name;
            var adminName = App.Current.User.Name;

            switch (this.TemplateId)
            {
                case MailTemplateType.Message:
                    App.Current.Mailer.SendMessage(
                        email,
                        new MessageArgs
                        {
                            Name = name,
                            AdminName = adminName,
                            Message = this.Message,
                        });
                    break;

                case MailTemplateType.PaymentRequest:
                    var money = this.AdministeringUser.Money;

                    if (money >= 0)
                    {
                        this.ErrorMessage = "利用者の金額がマイナスではありません。";
                        return;
                    }

                    App.Current.Mailer.SendPaymentRequest(
                        email,
                        new PaymentRequestArgs
                        {
                            Name = name,
                            AdminName = adminName,
                            Money = money.ToCommaString(),
                        });
                    break;
            }

            this.ShowMessageBox("ユーザーにメールを送信しました。", "メール送信に成功");
            Debug.WriteLine("Send mail succeeded");
        }
    }
}
