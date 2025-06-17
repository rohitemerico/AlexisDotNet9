using System.Xml.Serialization;

namespace MDM.iOS.Entities.MDM
{
    /// <summary>
    /// This entity class is used to store the OS update information 
    /// query responses coming from the iPad device as a result of the 
    /// query command "AvailableOSUpdates" sent from MDM server to iPad device. 
    /// </summary>

    public class OSUpdateEn
    {

        private Guid id;
        [XmlAnyAttribute]
        public Guid ID
        {
            get { return id; }
            set { id = value; }
        }


        private string udid;
        [XmlAnyAttribute]
        public string UDID
        {
            get { return udid; }
            set { udid = value; }
        }

        private string humanReadableName;
        [XmlAnyAttribute]
        public string HumanReadableName
        {
            get { return humanReadableName; }
            set { humanReadableName = value; }
        }

        private string productKey;
        [XmlAnyAttribute]
        public string ProductKey
        {
            get { return productKey; }
            set { productKey = value; }
        }

        private string productVersion;
        [XmlAnyAttribute]
        public string ProductVersion
        {
            get { return productVersion; }
            set { productVersion = value; }
        }

        private bool restartRequired;
        [XmlAnyAttribute]
        public bool RestartRequired
        {
            get { return restartRequired; }
            set { restartRequired = value; }
        }


        private bool updateInstalled;
        public bool UpdateInstalled
        {
            get { return updateInstalled; }
            set { updateInstalled = value; }
        }

    }
}
