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
    }
}
