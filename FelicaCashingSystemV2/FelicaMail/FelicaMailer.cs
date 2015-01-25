using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaMail
{
    public static class FelicaMailer
    {
        public static void SendRegistered(
            this Mailer mailer,
            string to,
            RegisteredArgs args
            )
        {
            mailer.SendAsTemplate(to, @".\MailTemplates\Registered.txt", args);
        }

        public static void SendAdminMoney(
            this Mailer mailer,
            string to,
            AdminMoneyArgs args
            )
        {
            mailer.SendAsTemplate(to, @".\MailTemplates\AdminMoney.txt", args);
        }

        public static void SendPaymentRequest(
            this Mailer mailer,
            string to,
            PaymentRequestArgs args
            )
        {
            mailer.SendAsTemplate(to, @".\MailTemplates\PaymentRequest.txt", args);
        }

        public static void SendMessage(
            this Mailer mailer,
            string to,
            MessageArgs args
            )
        {
            mailer.SendAsTemplate(to, @".\MailTemplates\Message.txt", args);
        }
    }
}
