//using System.Threading.Tasks;
using System.Xml.Serialization;
using Dashboard.Entities.ADCB.Dashboard;
namespace Dashboard.Entities.ADCB
{
    [Serializable]
    public class DownloadKioskSetting
    {
        private AMasterSettings masterSetting;
        [XmlAttribute]
        public AMasterSettings MasterSetting
        {
            get { return masterSetting; }
            set { masterSetting = value; }
        }

        private MBizHour mbizHour;
        [XmlAttribute]
        public MBizHour MBizHour
        {
            get { return mbizHour; }
            set { mbizHour = value; }
        }

        private MAdvertisement backgroundImg;
        [XmlAttribute]
        public MAdvertisement BackgroundImg
        {
            get { return backgroundImg; }
            set { backgroundImg = value; }
        }

        private List<MAdvertisement> advList;
        [XmlAttribute]
        public List<MAdvertisement> AdvList
        {
            get
            {
                if (advList == null)
                    advList = new List<MAdvertisement>();
                return advList;
            }
            set { advList = value; }
        }

        private List<MAdvertisement> advListAllActive;
        [XmlAttribute]
        public List<MAdvertisement> AdvListAllActive
        {
            get
            {
                if (advListAllActive == null)
                    advListAllActive = new List<MAdvertisement>();
                return advListAllActive;
            }
            set { advListAllActive = value; }
        }

        private MHopper hopper;
        [XmlAttribute]
        public MHopper Hopper
        {
            get { return hopper; }
            set { hopper = value; }
        }

        private MDocument document;
        [XmlAttribute]
        public MDocument Document
        {
            get { return document; }
            set { document = value; }
        }

        private MApp app;
        [XmlAttribute]
        public MApp App
        {
            get { return app; }
            set { app = value; }
        }

        private MKiosk kiosk;
        [XmlAttribute]
        public MKiosk Kiosk
        {
            get { return kiosk; }
            set { kiosk = value; }
        }

        private string serverDateTime;
        [XmlAttribute]
        public string ServerDateTime
        {
            get { return serverDateTime; }
            set { serverDateTime = value; }
        }

        private List<AWrapServ> wrap;
        [XmlAttribute]
        public List<AWrapServ> Wrap
        {
            get
            {
                if (wrap == null)
                    wrap = new List<AWrapServ>();
                return wrap;
            }
            set { wrap = value; }
        }

        private List<AWrapServ> serv;
        [XmlAttribute]
        public List<AWrapServ> Serv
        {
            get
            {
                if (serv == null)
                    serv = new List<AWrapServ>();
                return serv;
            }
            set { serv = value; }
        }

        private List<MDocument> cyLDoc;
        [XmlAttribute]
        public List<MDocument> LDoc
        {
            get
            {
                if (cyLDoc == null)
                    cyLDoc = new List<MDocument>();
                return cyLDoc;
            }
            set { cyLDoc = value; }
        }

        private List<ASecPayCheque> secPayCheque;
        [XmlAttribute]
        public List<ASecPayCheque> SecPayCheque
        {
            get
            {
                if (secPayCheque == null)
                    secPayCheque = new List<ASecPayCheque>();
                return secPayCheque;
            }
            set { secPayCheque = value; }
        }

        private string status;
        [XmlAttribute]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private string statusMessage;
        [XmlAttribute]
        public string StatusMessage
        {
            get { return statusMessage; }
            set { statusMessage = value; }
        }
    }

}