using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class DataAnalyticsTool
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

    public class DataAnalyticsSite
    {
        private int siteid;
        [JsonProperty("idsite")]
        public int SiteId
        {
            get { return siteid; }
            set { siteid = value; }
        }
        private string name;
        [JsonProperty("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string mainurl;
        [JsonProperty("main_url")]
        public string MainUrl
        {
            get { return mainurl; }
            set { mainurl = value; }
        }

        private string trackingcode;
        [JsonProperty("tracking_code")]
        public string TrackingCode
        {
            get { return trackingcode; }
            set { trackingcode = value; }
        }
    }
}
