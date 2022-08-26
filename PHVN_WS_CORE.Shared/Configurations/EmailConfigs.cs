using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace PHVN_WS_CORE.SHARED.Configurations
{
    [Serializable]
    public class EmailConfigs
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public List<EmailObj> EmailObjects { get;set;}
    }

    [Serializable]
    public class EmailObj
    {
        public string AppId { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Body { get; set; }
        public string AttachedFileName{ get; set; }
        public MailPriority Priority { get; set; } = MailPriority.Normal;
    }
}
