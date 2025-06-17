using System.Xml.Serialization;

namespace MDM.iOS.Entities.Generic
{
    /// <summary>
    /// Entity class used when user makes an authentication request 
    /// </summary>

    [Serializable]
    public class GenericRequestEn
    {
        private string deviceImei;
        [XmlAnyAttribute]
        public string DeviceIMEI
        {
            get { return deviceImei; }
            set { deviceImei = value; }
        }

        private string deviceName;
        [XmlAnyAttribute]
        public string DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }

        private string adPath;
        [XmlAnyAttribute]
        public string AD_Path
        {
            get { return adPath; }
            set { adPath = value; }
        }

        private string adDomain;
        [XmlAnyAttribute]
        public string AD_Domain
        {
            get { return adDomain; }
            set { adDomain = value; }
        }

        private string appVersion;
        [XmlAnyAttribute]
        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }

        private Guid versionSettingID;
        [XmlAnyAttribute]
        public Guid VersionSettingID
        {
            get { return versionSettingID; }
            set { versionSettingID = value; }
        }

        private Guid advertisementID;
        [XmlAnyAttribute]
        public Guid AdvertisementID
        {
            get { return advertisementID; }
            set { advertisementID = value; }
        }

        private int advertisementIndicator;
        [XmlAnyAttribute]
        public int AdvertisementIndicator
        {
            get { return advertisementIndicator; }
            set { advertisementIndicator = value; }
        }

        private string branch_code;
        [XmlAttribute]
        public string BranchCode
        {
            get { return branch_code; }
            set { branch_code = value; }
        }

        private string email;
        [XmlAttribute]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string dateTo;
        [XmlAttribute]
        public string DateTo
        {
            get { return dateTo; }
            set { dateTo = value; }
        }

        private string dateFrom;
        [XmlAttribute]
        public string DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }

        private ReqLoginEn loginEn;
        [XmlAnyAttribute]
        public ReqLoginEn LoginEn
        {
            get
            {
                if (loginEn == null)
                {
                    loginEn = new ReqLoginEn();
                }
                return loginEn;
            }
            set { loginEn = value; }
        }

    }
}
