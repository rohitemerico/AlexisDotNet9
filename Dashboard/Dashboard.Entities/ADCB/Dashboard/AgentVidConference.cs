using System.Xml.Serialization;

namespace Dashboard.Entities.ADCB.Dashboard
{
    [Serializable]
    public class AgentVideoConference
    {
        private string jsonsetting;
        [XmlAttribute]
        public string JsonSetting
        {
            get { return jsonsetting; }
            set { jsonsetting = value; }
        }

        private string provider;
        [XmlAttribute]
        public string Provider
        {
            get { return provider; }
            set { provider = value; }
        }

        #region AVAYA PARAMETER

        private string AVserverIP;
        [XmlAttribute]
        public string AVServerIP
        {
            get { return AVserverIP; }
            set { AVserverIP = value; }
        }

        private string AVport;
        [XmlAttribute]
        public string AVPort
        {
            get { return AVport; }
            set { AVport = value; }
        }

        private string AVdomain;
        [XmlAttribute]
        public string AVDomain
        {
            get { return AVdomain; }
            set { AVdomain = value; }
        }

        private string AVextension;
        [XmlAttribute]
        public string AVExtension
        {
            get { return AVextension; }
            set { AVextension = value; }
        }

        private string AVpassword;
        [XmlAttribute]
        public string AVPassword
        {
            get { return AVpassword; }
            set { AVpassword = value; }
        }

        #endregion

        //#region ZOOM PARAMETER

        //private string ZserverIP;
        //[XmlAttribute]
        //public string ZServerIP
        //{
        //    get { return ZserverIP; }
        //    set { ZserverIP = value; }
        //}

        //private string Zport;
        //[XmlAttribute]
        //public string ZPort
        //{
        //    get { return Zport; }
        //    set { Zport = value; }
        //}

        //private string Zdomain;
        //[XmlAttribute]
        //public string ZDomain
        //{
        //    get { return Zdomain; }
        //    set { Zdomain = value; }
        //}

        //#endregion
    }
}
