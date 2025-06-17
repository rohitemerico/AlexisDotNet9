using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class MarketingAutomationTool
    {
        private string name;
        [XmlAttribute]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string Portalurl;
        [XmlAttribute]
        public string PortalUrl
        {
            get { return Portalurl; }
            set { Portalurl = value; }
        }
    }
}
