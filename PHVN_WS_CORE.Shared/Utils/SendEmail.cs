using PHVN_WS_CORE.SHARED.Configurations;
using System.Net.Mail;

namespace PHVN_WS_CORE.SHARED.Utils
{
    public class SendEmail
    {
        private static SendEmail instance;
        public static SendEmail Instance
        {
            get
            {
                if (instance == null)
                    instance = new SendEmail();
                return SendEmail.instance;
            }
            private set { SendEmail.instance = value; }
        }

        public SendEmail() { }

        public bool Send(EmailConfigs config, EmailObj emailObj)
        {
            MailMessage message = new MailMessage();

            try
            {
                #region Mail Server
                SmtpClient mailClient = new SmtpClient(config.Server);
                mailClient.Port = config.Port;

                System.Net.NetworkCredential cred = new System.Net.NetworkCredential(config.From, config.Password);
                mailClient.EnableSsl = true;
                mailClient.UseDefaultCredentials = false;
                mailClient.Credentials = cred;
                #endregion

                #region Mail detail

                //This DOES work
                message.From = new MailAddress(config.From, "Pizza Hut VietNam");
                message.IsBodyHtml = true;
                mailClient.EnableSsl = true;


                foreach (var address in emailObj.To?.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(address);
                }

                if (emailObj.Cc?.Length > 0)
                {
                    foreach (var address in emailObj.Cc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        message.CC.Add(address);
                    }
                }

                if (emailObj.Bcc?.Length > 0)
                {
                    foreach (var address in emailObj.Bcc.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        message.Bcc.Add(address);
                    }
                }

                message.Priority = emailObj.Priority;
                message.Subject = emailObj.Subject;
                message.Body = emailObj.Body;

                #endregion

                mailClient.Send(message);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
