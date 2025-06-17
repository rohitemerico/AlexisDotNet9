//using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class Alerts
    {

        public string emailTo;
        [XmlAttribute]
        private string EmailTo
        {
            get { return emailTo; }
            set { emailTo = value; }
        }

        public string emailFrom;
        [XmlAttribute]
        private string EmailFrom
        {
            get { return emailFrom; }
            set { emailFrom = value; }
        }

        public string emailCC;
        [XmlAttribute]
        private string EmailCC
        {
            get { return emailCC; }
            set { emailCC = value; }
        }

        public string body;
        [XmlAttribute]
        private string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string subject;
        [XmlAttribute]
        private string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string password;
        [XmlAttribute]
        private string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string host;
        [XmlAttribute]
        private string Host
        {
            get { return host; }
            set { host = value; }
        }

        public bool sSL;
        [XmlAttribute]
        private bool SSL
        {
            get { return sSL; }
            set { sSL = value; }
        }

        public string sms;
        [XmlAttribute]
        private string Sms
        {
            get { return sms; }
            set { sms = value; }
        }

        public string alertType;
        [XmlAttribute]
        private string AlertType
        {
            get { return alertType; }
            set { alertType = value; }
        }


        public string mailContent;
        [XmlAttribute]
        private string MailContent
        {
            get { return mailContent; }
            set { mailContent = value; }
        }
    }

}