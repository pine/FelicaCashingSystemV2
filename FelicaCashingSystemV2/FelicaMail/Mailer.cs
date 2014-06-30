using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaMail
{
    public class Mailer
    {
        public string MailFrom { get; private set; }
        public string SmtpHost { get; private set; }
        public int SmtpPort { get; private set; }
        public string SmtpUser { get; private set; }
        public string SmtpPassword { get; private set; }

        public void Setup(
            string mailFrom,
            string smtpHost,
            int smtpPort,
            string smtpUser,
            string smtpPassword
            )
        {
            this.MailFrom = mailFrom;
            this.SmtpHost = smtpHost;
            this.SmtpPort = smtpPort;
            this.SmtpUser = smtpUser;
            this.SmtpPassword = smtpPassword;
        }

        public void Send(
            string to,
            string subject,
            string body
            )
        {
            // メールアドレスの形式を確認する
            if (!MailAddress.IsValidEmail(to))
            {
                return; // 不正なメールアドレス
            }
            
            // メールメッセージを作成する
            using (
                var msg = new System.Net.Mail.MailMessage(
                    this.MailFrom,
                    to, subject, body
                    )
                )

            // SMTP クライアントを作成
            using (
                var sc = new System.Net.Mail.SmtpClient(this.SmtpHost, this.SmtpPort)
                )
            {
                // ユーザー名とパスワード
                sc.Credentials = new System.Net.NetworkCredential(
                    this.SmtpUser,    // ユーザー名
                    this.SmtpPassword // パスワード
                    );

                // SSL は使わない
                // 工科大の SSL 証明書は検証に失敗するので (自己証明書?)
                sc.EnableSsl = false;

                sc.Send(msg);
            }
        }

        public void SendAsTemplate(
            string to,
            string templatePath,
            object args = null
            )
        {
            var template = new MailTemplate();
            template.Load(templatePath);

            var text = template.ApplyTemplate(args);
            var lines = text.Replace("\r", string.Empty).Split('\n');

            var subject = lines[0];
            var body = string.Join("\n\r", lines, 1, lines.Length - 1);

            this.Send(to, subject, body);
        }
    }
}
