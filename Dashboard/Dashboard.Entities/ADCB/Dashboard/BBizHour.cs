//using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class BBizHour : Base_Dashboard
    {
        private Guid bid;
        [XmlAnyAttribute]
        public Guid Bid
        {
            get { return bid; }
            set { bid = value; }
        }

        private string templateName;
        [XmlAnyAttribute]
        public string TemplateName
        {
            get { return templateName; }
            set { templateName = value; }
        }

        private bool monday;
        [XmlAnyAttribute]
        public bool Monday
        {
            get { return monday; }
            set { monday = value; }
        }
        private bool tuesday;
        [XmlAnyAttribute]
        public bool Tuesday
        {
            get { return tuesday; }
            set { tuesday = value; }
        }
        private bool wednesday;
        [XmlAnyAttribute]
        public bool Wednesday
        {
            get { return wednesday; }
            set { wednesday = value; }
        }
        private bool thursday;
        [XmlAnyAttribute]
        public bool Thursday
        {
            get { return thursday; }
            set { thursday = value; }
        }
        private bool friday;
        [XmlAnyAttribute]
        public bool Friday
        {
            get { return friday; }
            set { friday = value; }
        }
        private bool saturday;
        [XmlAnyAttribute]
        public bool Saturday
        {
            get { return saturday; }
            set { saturday = value; }
        }
        private bool sunday;
        [XmlAnyAttribute]
        public bool Sunday
        {
            get { return sunday; }
            set { sunday = value; }
        }
        private TimeSpan starttime;
        [XmlAnyAttribute]
        public TimeSpan Starttime
        {
            get { return starttime; }
            set { starttime = value; }
        }
        private TimeSpan endtime;
        [XmlAnyAttribute]
        public TimeSpan Endtime
        {
            get { return endtime; }
            set { endtime = value; }
        }
    }

}